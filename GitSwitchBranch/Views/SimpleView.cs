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

        // Print header with styling
        Console.WriteLine();
        Console.WriteLine("\u001b[96m┌─────────────────────────────────────┐\u001b[0m"); // Cyan box drawing
        Console.WriteLine("\u001b[96m│\u001b[0m      \u001b[92mSelect a Git Branch\u001b[0m           \u001b[96m│\u001b[0m"); // Cyan border, green title
        Console.WriteLine("\u001b[96m└─────────────────────────────────────┘\u001b[0m");
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
            // Active branch with highlight
            Console.WriteLine($"\u001b[93m{index,2}.\u001b[0m \u001b[92m*\u001b[0m \u001b[97m{branch.Name}\u001b[0m \u001b[90m(current)\u001b[0m");
        }
        else
        {
            // Inactive branch
            Console.WriteLine($"\u001b[97m{index,2}.\u001b[0m   \u001b[96m{branch.Name}\u001b[0m");
        }
    }

    private int GetBranchIndexFromUser(int numBranches)
    {
        while (true)
        {
            Console.Write($"\u001b[96m┌─[\u001b[92mgit-switch-branch\u001b[96m]\u001b[0m\n");
            Console.Write($"\u001b[96m└──\u001b[93m▶\u001b[0m Enter branch index \u001b[97m(\u001b[93m1-{numBranches}\u001b[97m)\u001b[0m or \u001b[91m'q'\u001b[0m to quit: ");

            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("\u001b[91m❌ Your input is empty\u001b[0m");
                continue;
            }

            if (input.Equals("q", StringComparison.InvariantCultureIgnoreCase))
            {
                break;
            }

            if (!int.TryParse(input, out int branchIndex))
            {
                Console.WriteLine($"\u001b[91m❌ Index must be an integer\u001b[0m");
                continue;
            }

            if (branchIndex <= 0 || branchIndex > numBranches)
            {
                Console.WriteLine($"\u001b[91m❌ Index \u001b[93m{branchIndex}\u001b[91m is out of range\u001b[0m");
                continue;
            }

            Console.WriteLine($"\u001b[92m✅ Selected branch: \u001b[97m{branchIndex}\u001b[0m");
            return branchIndex - 1;
        }

        return -1;
    }
}