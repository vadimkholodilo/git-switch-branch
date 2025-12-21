using GitSwitchBranch.Models;

namespace GitSwitchBranch.Views;

public class SimpleView(int width, int height) : BaseView(width, height)
{
    /// <summary>
    ///     Displays a list of branches and gets a selected branch from a user
    /// </summary>
    /// <param name="branches">List of branches</param>
    /// <returns>0-based branch index</returns>
    public override int DisplayBranchesAndGetBranchIndex(IReadOnlyList<Branch> branches)
    {
        if (branches is null || branches.Count == 0)
        {
            throw new ArgumentNullException(nameof(branches));
        }

        // Print header
        Console.WriteLine();
        Console.WriteLine("┌─────────────────────────────────────┐");
        Console.WriteLine("│      Select a Git Branch           │");
        Console.WriteLine("└─────────────────────────────────────┘");
        Console.WriteLine();

        for (int i = 0; i < branches.Count; i++)
        {
            DisplayBranch(branches[i], i + 1);
        }

        Console.WriteLine();
        return GetBranchIndexFromUser(branches.Count);
    }

    private void DisplayBranch(Branch branch, int index)
    {
        if (branch.IsActive)
        {
            // Active branch
            Console.WriteLine($"{index,2}. * {branch.Name} (current)");
        }
        else
        {
            // Inactive branch
            Console.WriteLine($"{index,2}.   {branch.Name}");
        }
    }

    private int GetBranchIndexFromUser(int numBranches)
    {
        while (true)
        {
            Console.Write($"┌─[git-switch-branch]\n");
            Console.Write($"└──▶ Enter branch index (1-{numBranches}) or 'q' to quit: ");

            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("❌ Your input is empty");
                continue;
            }

            if (input.Equals("q", StringComparison.InvariantCultureIgnoreCase))
            {
                break;
            }

            if (!int.TryParse(input, out int branchIndex))
            {
                Console.WriteLine($"❌ Index must be an integer");
                continue;
            }

            if (branchIndex <= 0 || branchIndex > numBranches)
            {
                Console.WriteLine($"❌ Index {branchIndex} is out of range");
                continue;
            }

            Console.WriteLine($"✅ Selected branch: {branchIndex}");
            return branchIndex - 1;
        }

        return -1;
    }
}