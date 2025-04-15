namespace HobbyTracker;

using System.IO;
using Spectre.Console;

public class ConsoleUI {
    saveFile filesave;

    public ConsoleUI() {
        filesave = new saveFile("project-data.txt");
    }

    public void View() {

        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("What would you like to do?")
            .AddChoices(new[] {
                "View projects", "Add project", "Edit project", "Archive", 
                "Remove"
            }));
        
        
        if(action== "Add project") {

            string command;

            do {

                string projectName = userInput("Enter project title: ");

                string projectDetails = userInput("Enter project details: ");

                string projectCategory = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Please select category")
                        .AddChoices(new[] {
                            "Blog idea", "Drawing", "Short story"
                    }));

                filesave.AppendLine(projectName+" ("+projectCategory+")"+": "+projectDetails);

                command = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to do?")
                        .AddChoices(new[] {
                            "Add another project", "Exit"
                    }));

            } while(command!="Exit");
        }
    }

        public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}