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
}