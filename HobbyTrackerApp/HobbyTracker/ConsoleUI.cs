namespace HobbyTracker;

using System.IO;
using System.IO.Compression;
using Spectre.Console;

public class ConsoleUI {
    
    private readonly User loginChoice;
    private readonly ProjectOrganizer userOrganizer;
    private readonly FilteringService filteringService;
    private readonly EditingService editingService;

    public ConsoleUI(User loginChoice, ProjectOrganizer userOrganizer) {
        this.loginChoice =loginChoice;
        this.userOrganizer = userOrganizer;
        this.filteringService = new FilteringService(userOrganizer);
        this.editingService = new EditingService(userOrganizer);

    }

    public void View() {
        
        while (true) {
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices(new[] {
                    "Add project", "View projects", "Edit project", "Archive", "Remove", "Exit"
                })
            );
                        
            switch(action) {

                case "Add project":
                    AddProjectService();
                    break;

                case "View projects":
                    filteringService.Showfilteredprojects();
                    break;
                
                case "Edit project":
                    editingService.EditProject();
                    break;

                case "Archive":
                    ArchiveProject();
                    break;
                
                case "Remove":
                    RemoveProject();
                    break;
                
                case "Exit":
                    return;
            }
        }
    }

private void AddProjectService() {
    string command;

        do {

            string projectName = userInput("Enter project title: ").Trim();

            string projectDetails = userInput("Enter project details: ").Trim();

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
                    
            string userDate = userInput("Enter a target completion date (MM/DD/YYY) or enter to skip").Trim();
                    
            DateTime completionDate;
                    
            if (!DateTime.TryParse(userDate, out completionDate)) {
                    completionDate = DateTime.Now.AddDays(365);
                    AnsiConsole.MarkupLine("[darkorange]Incorrect formant or no date entered, default to 365 days [/]");
                }

            var category = new Category(projectCategory);
            var newProject = new ProjectData(projectName, projectDetails, category, progress, priority, completionDate, loginChoice);

            userOrganizer.SaveProject(newProject);

            command = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                     .AddChoices(new[] {
                        "Add another project", "Exit"
                    })
            );

        } while(command!="Exit");
     }

    private void ArchiveProject() {

        var projectList = userOrganizer.GetAllProjects().Where(p => !p.IsArchived).ToList(); 

        if(projectList.Count() == 0) { 

                AnsiConsole.MarkupLine($"[yellow]No projects are available[/]"); 
                return;

            } else { 

                var chosenProject = AnsiConsole.Prompt( 
                    new SelectionPrompt<string>() 
                        .Title("Select project to archive:") 
                        .AddChoices(projectList.Select(p => p.ProjectName) 
                    ) 
                ); 

                var project = projectList.FirstOrDefault(p => p.ProjectName == chosenProject); 
                
                if(project != null) { 

                    project.IsArchived = true; 

                    userOrganizer.SaveAlltoFile(); 

                    AnsiConsole.MarkupLine($"[green]{project.ProjectName} was archived[/]"); 

                }   

        } 
    }
    
    private void RemoveProject() {
        
        var projectList = userOrganizer.GetAllProjects();

        if(projectList.Count() == 0) {

            AnsiConsole.MarkupLine($"[yellow]No projects stored, add a project to begin [/]");
            return;
        }

        var projectToRemove = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select a project to remove:")
            .AddChoices(projectList.Select(p => p.ProjectName))
        );
    }      
    public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}