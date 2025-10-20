using GitSwitchBranch.Models;

namespace GitSwitchBranch.Utils;

public static class GitParser
{
    public static IEnumerable<Branch> ParseBranches(string? output)
    {
        if (string.IsNullOrEmpty(output))
            return [];

        var activeBranchFound = false;
        List<Branch> result = [];
        var lines = output.Split(["\r", "\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var branchName = line.Trim();
            var isActive = branchName[0] == '*';
            if (isActive && activeBranchFound)
                throw new ArgumentNullException("Active branch was already found in the output");

            activeBranchFound = isActive;
            result.Add(new Branch(branchName, isActive));
        }

        return result;
    }
}