namespace HobbyTracker.Tests;

using HobbyTracker;

public class saveFileTests

{
    saveFile filesave;
    string testFileName;

    public saveFileTests() {
        testFileName = "test-doc.txt";
        filesave = new saveFile(testFileName);
    }
    
    [Fact]
    public void Test_saveFile_Append()
    {
        filesave.AppendLine("Cheese is good!");
        var contentFromFile = File.ReadAllText(testFileName);
        Assert.Equal("Cheese is good!"+Environment.NewLine, contentFromFile);
    }
}