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

        List<Branch> branches =
        [
            new Branch("master", true),
            new Branch("development", false),
            new Branch("hotfix", false)
        ];

        CheckRepository();

        var selectedBranchIndex = view.DisplayBranchesAndGetBranchIndex(branches);

        if (selectedBranchIndex == -1)
        {
            Console.WriteLine("Bye");
            Environment.Exit(0);
        }

        CheckoutBranch(branches[selectedBranchIndex]);
    }

    private static void CheckRepository()
    {
        if (!GitUtils.IsRepository(Environment.CurrentDirectory))
        {
            Console.WriteLine("Current directory is not a git repository. Quitting");
            Environment.Exit(-1);
        }
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
