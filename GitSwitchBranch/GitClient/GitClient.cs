using System.Diagnostics;
using GitSwitchBranch.Models;

namespace GitSwitchBranch.GitClient;

public class GitClient
{
    private readonly string _currentDirectoryPath;

    public GitClient(string currentDirectoryPath)
    {
        _currentDirectoryPath = currentDirectoryPath ?? throw new ArgumentNullException(nameof(currentDirectoryPath));
    }

    public bool IsRepository()
    {
        try
        {
            return Directory.Exists(Path.Combine(_currentDirectoryPath, ".git"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool IsGitAvailable()
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

    public IEnumerable<Branch> GetAllBranches(bool includeRemote = false)
    {
        var arguments = includeRemote ? "-a" : "";
        var output = ExecuteGitCommand("branch", arguments);

        return GitParser.ParseBranches(output);
    }

    public void CheckoutBranch(string branchName)
    {
        if (string.IsNullOrEmpty(branchName))
            throw new ArgumentNullException(nameof(branchName));

        ExecuteGitCommand("checkout", branchName);
    }

    private string ExecuteGitCommand(string command, string arguments)
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
                CreateNoWindow = true,
                WorkingDirectory = _currentDirectoryPath
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

            if (process.ExitCode != 0 && !string.IsNullOrEmpty(error))
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