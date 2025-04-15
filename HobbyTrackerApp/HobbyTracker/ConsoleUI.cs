namespace HobbyTracker;

using System.IO;

public class ConsoleUI {
    saveFile filesave;

    public ConsoleUI() {
        filesave = new saveFile("project-data.txt");
    }

    public void View() {
        string action = userInput("Please select action (add, edit, archive, remove): ");

        if(action== "add") {

            string command;

            do {

                string projectName = userInput("Enter project title: ");

                string projectDetails = userInput("Enter project details: ");

                string projectCategory = userInput("Select project category (Blog idea, Drawing, Short story): ");

                filesave.AppendLine(projectName+" ("+projectCategory+")"+": "+projectDetails);

                command = userInput("Add another project or exit: " );

            } while(command!="exit");
        }
    }

        public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}