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

                    string projectName = userInput("Enter project title: ").Trim();

                    string projectDetails = userInput("Enter project details: ").Trim();

                    string projectCategory = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Please select category")
                            .AddChoices(new[] {
                                "Blog idea", "Drawing", "Short story"
                        }));

                    //add progress.priority,compl etion date, arbitrary default info
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
                
                FilterProjects();

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
                AnsiConsole.MarkupLine($"[yellow] In Version 2[/]");

            }else if(action=="Archive") {  
                AnsiConsole.MarkupLine($"[yellow] In Version 2[/]");

            }else if(action=="Exit"){
                break;
            }    
        }
    }

        private void FilterProjects() {
            var filterChoice = AnsiConsole.Prompt (
                new SelectionPrompt<string>()
                .Title("Select the view you would like to see:")
                .AddChoices(new[] {"All projects", "By category", "By priority", "By progress"}));
            
            List<ProjectData> filteredProjects = new List<ProjectData>();

            if (filterChoice == "By progress") { 
                var progressFilter = AnsiConsole.Prompt( 
                    new SelectionPrompt<string>() 
                        .Title("Select progress:") 
                        .AddChoices(Enum.GetNames(typeof(Progress)))); 
                filteredProjects = userOrganizer.FilterByProgress(progressFilter); 
            }  else if (filterChoice == "By category") { 
                var categoryFilter = AnsiConsole.Prompt( 
                    new SelectionPrompt<string>() 
                        .Title("Select category:") 
                        .AddChoices(new[] { "Blog idea", "Drawing", "Short story" })); 
                filteredProjects = userOrganizer.FilterByCategory(categoryFilter); 
            } else if (filterChoice == "By priority") { 
                var priorityFilter = AnsiConsole.Prompt( 
                    new SelectionPrompt<string>() 
                        .Title("Select priority:") 
                        .AddChoices(Enum.GetNames(typeof(Priority)))); 
                filteredProjects = userOrganizer.FilterByPriority(priorityFilter);
            }  else if (filterChoice == "All projects")  { 
                filteredProjects = userOrganizer.GetAllProjects(); 
            } 
            if (filteredProjects.Count > 0)  { 
                userOrganizer.projectTableformatting(filteredProjects); 
            } else { 
                AnsiConsole.MarkupLine("[yellow]No projects match the selected filter.[/]"); 
            } 

        } 
          
        public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}