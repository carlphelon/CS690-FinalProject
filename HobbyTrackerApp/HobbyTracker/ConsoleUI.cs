namespace HobbyTracker;

using System.IO;
using Spectre.Console;

public class ConsoleUI {
    
    private readonly User loginChoice;
    private readonly ProjectOrganizer userOrganizer;

    public ConsoleUI(User loginChoice, ProjectOrganizer userOrganizer) {
        this.loginChoice =loginChoice;
        this.userOrganizer = userOrganizer;

    }

    public void View() {
        
        while (true) {
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices(new[] {
                     "Add project", "View projects", "Edit project", "Archive", 
                    "Remove", "Exit"
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

                    //add progress.priority,completion date, arbitrary default info
                    var progress = Progress.notStarted;
                    var priority = Priority.mediumPriority;
                    var completionDate = DateTime.Now.AddDays(14);

                    //
                    var category = new Category(projectCategory);
                    var newProject = new ProjectData(projectName, projectDetails, category, progress, priority, completionDate, loginChoice);

                    userOrganizer.saveProject(newProject);

                    command = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("What would you like to do?")
                            .AddChoices(new[] {
                                "Add another project", "Exit"
                        }));

                } while(command!="Exit");
            } else if(action=="View projects") {
                
                userOrganizer.displayALLProjects();

            }else if(action=="Exit"){
                break;
            }    
        }
    }

        public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}