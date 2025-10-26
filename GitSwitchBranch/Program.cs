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
            .DefaultConfigsWithAppDescription("Switch git branches interractively")
            .PositionalArgument<string>()
            .WithDescription("The branch name to search")
            .WithExamples("master", "development")
            .IsOptional()
            .Call(branchNameToSearch => SelectBranch(gitClient, view, branchNameToSearch))
            .Parse(args);
    }

    private static void SelectBranch(GitClient.GitClient gitClient, BaseView view, string branchNameToSearch)
    {
        var branches = branchNameToSearch != null ? SearchBranch(gitClient, branchNameToSearch) : GetBranches(gitClient);

        if (branches.Count == 0)
        {
            Console.WriteLine("No branches to display");
            Environment.Exit(-3);
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
            Environment.Exit(-1);
        }
    }

    private static void CheckRepository(GitClient.GitClient client)
    {
        if (!client.IsRepository())
        {
            Console.WriteLine($"'{Environment.CurrentDirectory}' is not a git repository. Quitting");
            Environment.Exit(-2);
        }
    }

    private static List<Branch> GetBranches(GitClient.GitClient client)
    {
        return client.GetAllBranches().ToList();
    }

    private static List<Branch> SearchBranch(GitClient.GitClient client, string branchNameToSearch)
    {
        if (string.IsNullOrEmpty(branchNameToSearch))
            throw new ArgumentNullException(nameof(branchNameToSearch));

        return client.GetAllBranches()
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
}
