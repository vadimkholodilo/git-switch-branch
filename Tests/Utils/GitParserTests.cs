using System.Text;
using GitSwitchBranch.Utils;

namespace Tests.Utils;

public class GitParserTests
{
    [Fact]
    public void ParseBranches_ShouldReturnEmptyList_WhenOutputIsNullOrEmpty()
    {
        var nullOutputResult = GitParser.ParseBranches(null).ToList();
        var emptyOutputResult = GitParser.ParseBranches("").ToList();

        Assert.Empty(nullOutputResult);
        Assert.Empty(emptyOutputResult);
    }

    [Theory]
    [InlineData("\r")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void ParseBranches_ShouldParseSuccessfully_WhenOutputContainsDifferentLineEndings(string eol)
    {
        var sb = new StringBuilder();
        sb.Append("master").Append(eol);
        sb.Append("development").Append(eol);

        var result = GitParser.ParseBranches(sb.ToString()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("master", result[0].Name);
        Assert.Equal("development", result[1].Name);
    }

    [Fact]
    public void ParseBranches_ShouldParseActiveBranch_WhenBranchNameStartsWithStar()
    {
        var sb = new StringBuilder();
        sb.AppendLine("* master");
        sb.AppendLine("development");

        var result = GitParser.ParseBranches(sb.ToString()).ToList();

        Assert.Equal(2, result.Count);
        var masterBranch = result[0];
        var developmentBranch = result[1];
        Assert.Equal("master", masterBranch.Name);
        Assert.True(masterBranch.IsActive);
        Assert.Equal("development", developmentBranch.Name);
    }

    [Fact]
    public void ParseBranches_ShouldTrimBranchName()
    {
        var sb = new StringBuilder();
        sb.AppendLine("      master            ");
        sb.AppendLine("                development                 ");

        var result = GitParser.ParseBranches(sb.ToString()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("master", result[0].Name);
        Assert.Equal("development", result[1].Name);
    }

    [Fact]
    public void ParseBranches_ShouldThrowArgumentException_WhenMoreThanOneActiveBranchWasFoundInOutput()
    {
        var sb = new StringBuilder();
        sb.AppendLine("* master");
        sb.AppendLine("* development");

        Assert.Throws<ArgumentNullException>(() => GitParser.ParseBranches(sb.ToString()));
    }
}