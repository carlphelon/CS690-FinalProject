namespace HobbyTracker;

using System.IO;

public class saveFile {
    string fileName;

    public saveFile(string fileName) {
        this.fileName = fileName;
        File.Create(this.fileName).Close();
    }

    public void AppendLine(string line) {
        File.AppendAllText(this.fileName, line + Environment.NewLine);
    }
}