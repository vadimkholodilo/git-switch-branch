using GitSwitchBranch.Models;

namespace GitSwitchBranch.Views;

public abstract class BaseView
{
    protected readonly int Width;
    protected readonly int Height;

    protected BaseView(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentException("Width must be positive", nameof(width));

        if (height <= 0)
            throw new ArgumentException("Height must be positive", nameof(height));

        Width = width;
        Height = height;
    }

    /// <summary>
    /// Displays a list of branches and gets a selected branch from a user
    /// </summary>
    /// <param name="branches">List of branches</param>
    /// <returns>0-based branch index</returns>
    /// <exception cref="ArgumentException">If the list of branches is empty</exception>
    public abstract int DisplayBranchesAndGetBranchIndex(IReadOnlyList<Branch> branches);
}