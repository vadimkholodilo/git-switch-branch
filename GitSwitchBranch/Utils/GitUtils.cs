using System.Diagnostics;
using GitSwitchBranch.Models;

namespace GitSwitchBranch.Utils;

public static class GitUtils
{
    public static bool IsRepository(string currentDirectoryPath)
    {
        try
        {
            return Directory.Exists(Path.Combine(currentDirectoryPath, ".git"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public static bool IsGitAvailable()
    {
        try
        {
            ExecuteGitCommand("version", "");
            return true;
        }
        catch (GitNotFoundException)
        {
            return false;
        }
    }

    public static IEnumerable<Branch> GetAllBranches(bool includeRemote = false)
    {
        var arguments = includeRemote ? "-a" : "";
        var output = ExecuteGitCommand("branch", arguments);

        return GitParser.ParseBranches(output);
    }

    private static string ExecuteGitCommand(string command, string arguments)
    {
        const int timeoutMs = 10000;
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"{command} {arguments}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!process.Start())
            {
                throw new GitNotFoundException();
            }
        
            if (!process.WaitForExit(timeoutMs))
            {
                process.Kill();
                throw new GitCommandTimeoutException();
            }
        
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
        
            if (process.ExitCode != 0 || !string.IsNullOrEmpty(error))
            {
                throw new GitCommandExecutionException($"Git command failed: {error}");
            }
        
            return output;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            throw new GitNotFoundException();
        }
    }
}