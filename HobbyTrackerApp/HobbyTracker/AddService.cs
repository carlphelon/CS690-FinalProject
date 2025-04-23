namespace HobbyTracker;

using Spectre.Console;
using System;

public class AddService {

    private readonly ProjectOrganizer projectOrganizer;
    private readonly User loginChoice;

    public AddService(ProjectOrganizer projectOrganizer, User loginChoice) {
        this.projectOrganizer = projectOrganizer;
        this.loginChoice = loginChoice;
    }

    public void AddProject() {
    string command;

        do {

            string projectName = ConsoleUI.userInput("Enter project title: ").Trim();

            string projectDetails = ConsoleUI.userInput("Enter project details: ").Trim();

            string projectCategory = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select category")
                    .AddChoices(new[] {
                        "Blog idea", "Drawing", "Short story"
                    })
                );
                   
            var progress = AnsiConsole.Prompt(
                new SelectionPrompt<Progress>()
                    .Title("Please select progress")
                    .AddChoices(Enum.GetValues<Progress>()
                )
            );
                              
            var priority =AnsiConsole.Prompt(
                new SelectionPrompt<Priority>()
                    .Title("Please select priority")
                    .AddChoices(Enum.GetValues<Priority>()
                )
            );
                    
            string userDate = ConsoleUI.userInput("Enter a target completion date (MM/DD/YYY) or enter to skip").Trim();
                    
            DateTime completionDate;
                    
            if (!DateTime.TryParse(userDate, out completionDate)) {
                    completionDate = DateTime.Now.AddDays(365);
                    AnsiConsole.MarkupLine("[darkorange]Incorrect formant or no date entered, default to 365 days [/]");
                }

            var category = new Category(projectCategory);
            var newProject = new ProjectData(projectName, projectDetails, category, progress, priority, completionDate, loginChoice);

            projectOrganizer.SaveProject(newProject);

            command = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                     .AddChoices(new[] {
                        "Add another project", "Exit"
                    })
            );

        } while(command!="Exit");
     }
}