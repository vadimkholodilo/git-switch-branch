using GitSwitchBranch.Models;
using GitSwitchBranch.Utils;
using GitSwitchBranch.Views;

namespace GitSwitchBranch;

class Program
{
    private const int DefaultWidth = 80;
    private const int DefaultHeight = 40;

    static void Main(string[] args)
    {
        BaseView view = new SimpleView(DefaultWidth, DefaultHeight);

        CheckIfGitIsAvailable();
        CheckRepository();

        var branches = GetBranches();
        var selectedBranchIndex = view.DisplayBranchesAndGetBranchIndex(branches);

        if (selectedBranchIndex == -1)
        {
            Console.WriteLine("Bye");
            Environment.Exit(0);
        }

        CheckoutBranch(branches[selectedBranchIndex]);
    }

    private static void CheckIfGitIsAvailable()
    {
        if (!GitUtils.IsGitAvailable())
        {
            Console.WriteLine("Git was not found on your system. It is either not installed or not in your PATH, quitting");
            Environment.Exit(-1);
        }
    }

    private static void CheckRepository()
    {
        if (!GitUtils.IsRepository(Environment.CurrentDirectory))
        {
            Console.WriteLine("Current directory is not a git repository. Quitting");
            Environment.Exit(-2);
        }
    }

    private static List<Branch> GetBranches()
    {
        return GitUtils.GetAllBranches().ToList();
    }

    private static void CheckoutBranch(Branch branch)
    {
        if (branch.IsActive)
        {
            Console.WriteLine($"You're already on {branch.Name}");
            return;
        }

        Console.WriteLine($"Checked out {branch.Name}");
    }
}
