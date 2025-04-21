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

                    
                    var progress = AnsiConsole.Prompt(
                        new SelectionPrompt<Progress>()
                            .Title("Please select progress")
                            .AddChoices(Enum.GetValues<Progress>())
                            );
                              
                    var priority =AnsiConsole.Prompt(
                        new SelectionPrompt<Priority>()
                            .Title("Please select priority")
                            .AddChoices(Enum.GetValues<Priority>())
                            );
                    string userDate = userInput("Enter a target completion date (MM/DD/YYY) or enter to skip").Trim();
                    DateTime completionDate;
                    if (!DateTime.TryParse(userDate, out completionDate)) {
                            completionDate = DateTime.Now.AddDays(365);
                            AnsiConsole.MarkupLine("[darkorange]No date entered, default to 365 days [/]");
                    }

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
                EditProject();

            }else if(action=="Archive") {  
                var projectList = userOrganizer.GetAllProjects().Where(p => !p.isArchived).ToList();

                if(projectList.Count() == 0) {
                    AnsiConsole.MarkupLine($"[yellow]No projects are available[/]");
                } else {
                    var projectToarchive = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select project to archive:")
                            .AddChoices(projectList.Select(p => p.projectName))
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

        private void FilterProjects() {
            var filterChoice = AnsiConsole.Prompt (
                new SelectionPrompt<string>()
                .Title("Select the view you would like to see:")
                .AddChoices(new[] {"All projects", "By category", "By priority", "By progress", "Archived"}));
            
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
            } else if (filterChoice == "Archived") {
                filteredProjects = userOrganizer.GetAllProjects().Where(p => p.isArchived).ToList();
            } else if (filterChoice == "All projects")  { 
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

    public void EditProject() {

        var listofprojects = userOrganizer.GetAllProjects()
            .Select(p => p.projectName)
            .Distinct()
            .ToList();

         if(listofprojects.Count() == 0) {
                    AnsiConsole.MarkupLine($"[yellow]No projects are available[/]");
                    return;
                } else {
                    
                        var projectToedit = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select project to edit:")
                                .AddChoices(listofprojects)
                        );

                        var project = userOrganizer.GetAllProjects().FirstOrDefault(p => p.projectName == projectToedit);


                        if(project!=null) {
                            string field;
                            do {
                                //Select field to edit
                                field = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                    .Title("Select field to edit:")
                                    .AddChoices(new[] { "Project Name", "Description", "Category", "Progress", "Priority", "Target completion date", "Exit" })
                                );
                            
                                if (field=="Project Name") {
                                    //Edit name
                                    string newname = userInput($"Current name: {project.projectName}. Enter new name or press enter to skip");
                                    if (!string.IsNullOrEmpty(newname)) {
                                        project.projectName = newname;
                                    }
                                }else if(field == "Description") {
                                    string newdescription = userInput($"Current description: {project.projectDescription}. Enter new description or press enter to skip");
                                    if (!string.IsNullOrEmpty(newdescription)) {
                                        project.projectDescription = newdescription;
                                    }
                                }else if(field == "Category") {
                                    string newcategory = AnsiConsole.Prompt(
                                        new SelectionPrompt<string>()
                                        .Title($"Current category: {project.Category.projectCategory}. Select new category")
                                        .AddChoices(new[] {"Blog idea", "Drawing", "Short story"}));
                                    project.Category= new Category(newcategory);

                                }else if(field == "Progress") {
                                    var newprogress = AnsiConsole.Prompt(
                                        new SelectionPrompt<Progress>()
                                        .Title($"Current progress: . Select new status")
                                        .AddChoices(Enum.GetValues<Progress>())
                                        );
                                    project.Progress= newprogress;  
                                }else if(field == "Priority") {
                                    var newpriority = AnsiConsole.Prompt(
                                        new SelectionPrompt<Priority>()
                                        .Title($"Current priority: {project.Priority}. Select new status")
                                        .AddChoices(Enum.GetValues<Priority>()));
                                    project.Priority= newpriority;
                                }else if(field == "Target completion date") {
                                    string userdate = userInput($"Current target date: {project.projectCompletionDate.ToShortDateString()}. Enter new date (MM/DD/YYYY) or press enter to skip:");
                                    if(!string.IsNullOrEmpty(userdate)) {
                                        if (DateTime.TryParse(userdate, out DateTime newdate)) {
                                            project.projectCompletionDate = newdate;
                                        }else {
                                            AnsiConsole.MarkupLine($"[red]Invalid format[/]");
                                        }
                                    }              
                                }
                            } while (field!= "Exit") ; 

    }}}}
  