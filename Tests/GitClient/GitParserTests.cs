using System.Text;

using GitSwitchBranch.GitClient;
using GitSwitchBranch.Models;

namespace Tests.GitClient;

public class GitParserTests
{
    [Fact]
    public void ParseBranches_ShouldReturnEmptyList_WhenOutputIsNullOrEmpty()
    {
        List<Branch> nullOutputResult = GitParser.ParseBranches(null).ToList();
        List<Branch> emptyOutputResult = GitParser.ParseBranches("").ToList();

        Assert.Empty(nullOutputResult);
        Assert.Empty(emptyOutputResult);
    }

    [Theory]
    [InlineData("\r")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void ParseBranches_ShouldParseSuccessfully_WhenOutputContainsDifferentLineEndings(string eol)
    {
        StringBuilder sb = new();
        sb.Append("master").Append(eol);
        sb.Append("development").Append(eol);

        List<Branch> result = GitParser.ParseBranches(sb.ToString()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("master", result[0].Name);
        Assert.Equal("development", result[1].Name);
    }

    [Fact]
    public void ParseBranches_ShouldParseActiveBranch_WhenBranchNameStartsWithStar()
    {
        StringBuilder sb = new();
        sb.AppendLine("* master");
        sb.AppendLine("development");

        List<Branch> result = GitParser.ParseBranches(sb.ToString()).ToList();

        Assert.Equal(2, result.Count);
        Branch masterBranch = result[0];
        Branch developmentBranch = result[1];
        Assert.Equal("master", masterBranch.Name);
        Assert.True(masterBranch.IsActive);
        Assert.Equal("development", developmentBranch.Name);
    }

    [Fact]
    public void ParseBranches_ShouldTrimBranchName()
    {
        StringBuilder sb = new();
        sb.AppendLine("      master            ");
        sb.AppendLine("                development                 ");

        List<Branch> result = GitParser.ParseBranches(sb.ToString()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("master", result[0].Name);
        Assert.Equal("development", result[1].Name);
    }

    [Fact]
    public void ParseBranches_ShouldThrowArgumentException_WhenMoreThanOneActiveBranchWasFoundInOutput()
    {
        StringBuilder sb = new();
        sb.AppendLine("* master");
        sb.AppendLine("* development");

        Assert.Throws<ArgumentNullException>(() => GitParser.ParseBranches(sb.ToString()));
    }
}