using FluentArgs;
using GitSwitchBranch.Models;
using GitSwitchBranch.Views;

namespace GitSwitchBranch;

class Program
{
    private const int DefaultWidth = 80;
    private const int DefaultHeight = 40;

    static void Main(string[] args)
    {
        BaseView view = new SimpleView(DefaultWidth, DefaultHeight);
        var gitClient = new GitClient.GitClient(Environment.CurrentDirectory);

        CheckIfGitIsAvailable(gitClient);
        CheckRepository(gitClient);
        FluentArgsBuilder.New()
            .DefaultConfigsWithAppDescription("Switch git branches interactively")
            .Flag("-r", "--remote")
                .WithDescription("Include remote branches in the listing")
            .PositionalArgument<string>()
                .WithDescription("The branch name to search")
                .WithExamples("master", "development")
                .IsOptional()
            // Use a curried delegate as required by FluentArgs: flag => positional => action
            // Note: FluentArgs provides curried parameters in reverse registration order
            // (last registered argument is provided first to the Call), so the lambda below
            // takes the positional branchName first, then the flag value.
            .Call(branchNameToSearch => includeRemote =>
            {
                SelectBranch(gitClient, view, branchNameToSearch, includeRemote);
            })
            .Parse(args ?? Array.Empty<string>());
    }

    private static void SelectBranch(GitClient.GitClient gitClient, BaseView view, string? branchNameToSearch, bool includeRemote = false)
    {
        var branches = branchNameToSearch != null ? SearchBranch(gitClient, branchNameToSearch) : GetBranches(gitClient, includeRemote);

        if (branches.Count == 0)
        {
            Console.WriteLine("No branches to display");
            Exit(ExitCode.NoBranches);
        }

        if (branches.Count == 1)
        {
            CheckoutBranch(gitClient, branches[0]);
            Environment.Exit(0);
        }

        var selectedBranchIndex = view.DisplayBranchesAndGetBranchIndex(branches);

        if (selectedBranchIndex == -1)
        {
            Console.WriteLine("Bye");
            Environment.Exit(0);
        }

        CheckoutBranch(gitClient, branches[selectedBranchIndex]);
    }

    private static void CheckIfGitIsAvailable(GitClient.GitClient client)
    {
        if (!client.IsGitAvailable())
        {
            Console.WriteLine("Git was not found on your system. It is either not installed or not in your PATH, quitting");
            Exit(ExitCode.GitNotInstalled);
        }
    }

    private static void CheckRepository(GitClient.GitClient client)
    {
        if (!client.IsRepository())
        {
            Console.WriteLine($"'{Environment.CurrentDirectory}' is not a git repository. Quitting");
            Exit(ExitCode.NotRepository);
        }
    }

    private static List<Branch> GetBranches(GitClient.GitClient client, bool includeRemote)
    {
        return client.GetAllBranches(includeRemote).ToList();
    }

    private static List<Branch> SearchBranch(GitClient.GitClient client, string branchNameToSearch)
    {
        if (string.IsNullOrEmpty(branchNameToSearch))
            throw new ArgumentNullException(nameof(branchNameToSearch));

        return client.GetAllBranches(true)
            .Where(b => b.Name.Contains(branchNameToSearch, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private static void CheckoutBranch(GitClient.GitClient client, Branch branch)
    {
        if (branch.IsActive)
        {
            Console.WriteLine($"You're already on {branch.Name}");
            return;
        }

        client.CheckoutBranch(branch.Name);
        Console.WriteLine($"Checked out {branch.Name}");
    }

    private static void Exit(ExitCode exitCode = ExitCode.Success) => Environment.Exit((int)exitCode);
}
