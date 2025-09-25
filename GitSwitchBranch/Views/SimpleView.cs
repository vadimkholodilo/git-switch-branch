using GitSwitchBranch.Models;

namespace GitSwitchBranch.Views;

public class SimpleView(int width, int height) : BaseView(width, height)
{
    /// <summary>
    /// Displays a list of branches and gets a selected branch from a user
    /// </summary>
    /// <param name="branches">List of branches</param>
    /// <returns>0-based branch index</returns>
    /// <exception cref="ArgumentException">If the list of branches is empty</exception>
    public override int DisplayBranchesAndGetBranchIndex(IReadOnlyList<Branch> branches)
    {
        ArgumentNullException.ThrowIfNull(branches);
        if (branches.Count == 0)
            throw new ArgumentException("The list of branches cannot be empty", nameof(branches));

        Console.WriteLine("Branches: ");
        for(var i = 0; i < branches.Count; i++)
        {
            DisplayBranch(branches[i], i + 1);
        }

        return GetBranchIndexFromUser(branches.Count);
    }

    private void DisplayBranch(Branch branch, int index) =>
        Console.WriteLine($"{index}. {(branch.IsActive ? "*" : " ")} {branch.Name}");

    private int GetBranchIndexFromUser(int numBranches)
    {
        while (true)
        {
            Console.WriteLine($"Enter branch index from 1 to {numBranches} or 'q' to quit:");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Your input is empty");
                continue;
            }

            if (input.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                break;

            if (!int.TryParse(input, out var branchIndex))
            {
                Console.WriteLine("Index must be an integer");
                continue;
            }

            if (branchIndex <= 0 || branchIndex > numBranches)
            {
                Console.WriteLine($"Index {branchIndex} is out of range");
                continue;
            }

            return branchIndex - 1;
        }

        return -1;
    }
}