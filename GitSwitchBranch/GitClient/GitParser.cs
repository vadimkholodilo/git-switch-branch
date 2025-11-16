using GitSwitchBranch.Models;

namespace GitSwitchBranch.GitClient;

public static class GitParser
{
    public static IEnumerable<Branch> ParseBranches(string? output)
    {
        if (string.IsNullOrEmpty(output))
        {
            return [];
        }

        bool activeBranchFound = false;
        List<Branch> result = [];
        string[] lines = output.Split(["\r", "\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string branchName = line.Trim();
            bool isActive = branchName[0] == '*';
            if (isActive && activeBranchFound)
            {
                throw new ArgumentNullException("Active branch was already found in the output");
            }

            activeBranchFound = isActive;
            result.Add(new Branch(branchName.TrimStart('*', ' '), isActive));
        }

        return result;
    }
}