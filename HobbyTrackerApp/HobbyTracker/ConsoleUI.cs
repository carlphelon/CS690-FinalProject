namespace HobbyTracker;

using System.IO;
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
                        
            if(action== "Add project") {

                string command;

                do {

                    string projectName = userInput("Enter project title: ").Trim();

                    string projectDetails = userInput("Enter project details: ").Trim();

                    string projectCategory = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Please select category")
                            .AddChoices(new[] {
                                "Blog idea", "Drawing", "Short story"
                            }
                        )
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
                            AnsiConsole.MarkupLine("[darkorange]No date entered, default to 365 days [/]");
                        }

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
                filteringService.Showfilteredprojects();
                 
            } else if(action=="Remove") {
                var projectitles = userOrganizer.GetAllProjects()
                    .Select(p => p.projectName)
                    .Distinct()
                    .ToList();

                if (projectitles.Count()==0) {
                    AnsiConsole.MarkupLine($"[yellow]No projects, add some to get started! [/]");
                } else {
                   
                    var projectremove = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Select a project to remove:")
                        .AddChoices(projectitles)
                    );
                    userOrganizer.RemoveProject(projectremove);
                }

            }else if(action=="Edit project") {  
                editingService.EditProject();

            }else if(action=="Archive") {  
                var projectList = userOrganizer.GetAllProjects().Where(p => !p.isArchived).ToList();

                if(projectList.Count() == 0) {
                    AnsiConsole.MarkupLine($"[yellow]No projects are available[/]");
                } else {
                    var projectToarchive = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select project to archive:")
                            .AddChoices(projectList.Select(p => p.projectName)
                        )
                    );

                    var project = projectList.FirstOrDefault(p => p.projectName == projectToarchive);

                    if(project != null) {
                        project.isArchived = true;
                        userOrganizer.SaveAlltoFile();
                        AnsiConsole.MarkupLine($"[green]{project.projectName} was archived[/]");
                    }  
                }

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
  