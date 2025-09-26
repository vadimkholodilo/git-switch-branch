namespace GitSwitchBranch.Utils;

/// <summary>
/// Exception thrown when Git is not installed or not found in PATH.
/// </summary>
public class GitNotFoundException : Exception
{
    public GitNotFoundException() : base("Git is not installed or not found in PATH.") { }

    public GitNotFoundException(string message) : base(message) { }

    public GitNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a Git command execution fails.
/// </summary>
public class GitCommandExecutionException : Exception
{
    public GitCommandExecutionException(string message) : base(message) { }

    public GitCommandExecutionException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a Git command times out.
/// </summary>
public class GitCommandTimeoutException : Exception
{
    public GitCommandTimeoutException() : base("Git command timed out.") { }

    public GitCommandTimeoutException(string message) : base(message) { }

    public GitCommandTimeoutException(string message, Exception innerException) : base(message, innerException) { }
}