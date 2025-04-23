namespace HobbyTracker;
using Spectre.Console;
using System;
using System.Collections.Generic;

public class EditingService {
    
    private readonly ProjectOrganizer? projectorganizer;

    public EditingService(ProjectOrganizer? projectOrganizer) {
        
        projectorganizer = projectOrganizer ??  throw new ArgumentNullException(nameof(projectOrganizer));

    }

    public void EditProject() {

        var listofprojects = projectorganizer.GetAllProjects()
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

            var project = projectorganizer.GetAllProjects().FirstOrDefault(p => p.projectName == projectToedit);

            if(project!=null) {
                
                string field;
                    
                    do {
                     
                        field = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select field to edit:")
                                .AddChoices(new[] { "Project Name", "Description", "Category", "Progress", "Priority", "Target completion date", "Exit" })
                        );
                            
                        if (field=="Project Name") {
                            
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
                                    
                            var newcategory = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title($"Current category: {project.Category.projectCategory}. Select new category")
                                    .AddChoices(new[] {"Blog idea", "Drawing", "Short story"})
                            );
                                    
                            project.Category= new Category(newcategory);

                        }else if(field == "Progress") {
                                    
                            var newprogress = AnsiConsole.Prompt(
                                new SelectionPrompt<Progress>()
                                    .Title($"Current progress:{project.Progress} . Select new status")
                                    .AddChoices(Enum.GetValues<Progress>())
                            );
                                    
                            project.Progress= newprogress;  
                                
                        }else if(field == "Priority") {
                                    
                            var newpriority = AnsiConsole.Prompt(
                                new SelectionPrompt<Priority>()
                                    .Title($"Current priority: {project.Priority}. Select new status")
                                    .AddChoices(Enum.GetValues<Priority>())
                            );
                            
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
                projectorganizer.SaveAlltoFile();
                AnsiConsole.MarkupLine($"[green]{project.projectName} was updated and saved [/]");
            }
        }
    }
    public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}