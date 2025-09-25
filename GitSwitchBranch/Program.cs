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

        List<Branch> branches =
        [
            new Branch("master", true),
            new Branch("development", false),
            new Branch("hotfix", false)
        ];

        var selectedBranchIndex = view.DisplayBranchesAndGetBranchIndex(branches);

        if (selectedBranchIndex == -1)
        {
            Console.WriteLine("Bye");
            Environment.Exit(0);
        }

        CheckoutBranch(branches[selectedBranchIndex]);
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
