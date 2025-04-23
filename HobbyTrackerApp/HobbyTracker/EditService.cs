namespace HobbyTracker;
using Spectre.Console;
using System;
using System.Collections.Generic;

public class EditOptions {
    public string? NewName { get; set;}
    public string? NewDescription { get; set;}
    public string? NewCategory { get; set;}
    public Progress? NewProgress { get; set;}
    public Priority? NewPriority { get; set;}
    public DateTime? NewProjectCompletionDate { get; set;}
}
        
public class EditingService {
    
    private readonly ProjectOrganizer projectOrganizer;

    public EditingService(ProjectOrganizer projectOrganizer) {
        
        this.projectOrganizer = projectOrganizer ??  throw new ArgumentNullException(nameof(projectOrganizer));

    }

    public void ApplyEdit(ProjectData project, EditOptions options) {

        if (!string.IsNullOrWhiteSpace(options.NewName)) {
            project.ProjectName = options.NewName;
        }
        if (!string.IsNullOrWhiteSpace(options.NewDescription)) {
            project.ProjectDescription = options.NewDescription;
        }
        if (!string.IsNullOrWhiteSpace(options.NewCategory)) {
            project.Category = new Category(options.NewCategory);
        }
        if (options.NewProgress.HasValue) {
            project.Progress = options.NewProgress.Value;
        }
        if (options.NewPriority.HasValue) {
            project.Priority = options.NewPriority.Value;
        }
        if (options.NewProjectCompletionDate.HasValue) {
            project.ProjectCompletionDate = options.NewProjectCompletionDate.Value;
        }

    }

    public void EditProject() {

        var listofProjects = projectOrganizer.GetAllProjects()
            .Select(p => p.ProjectName)
            .Distinct()
            .ToList();

        if(listofProjects.Count() == 0) {
            AnsiConsole.MarkupLine($"[yellow]No projects are available[/]");
            return;
        
        } else {
                    
            var projectToEdit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select project to edit:")
                    .AddChoices(listofProjects)
            );

            var project = projectOrganizer.GetAllProjects().FirstOrDefault(p => p.ProjectName == projectToEdit);

            if(project!=null) {
                
                string field;
                    
                    do {
                     
                        field = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select field to edit:")
                                .AddChoices(new[] { "Project Name", "Description", "Category", "Progress", "Priority", "Target Completion Date", "Done" })
                        );
                            
                        if (field=="Project Name") {
                            
                            string? newname = userInput($"Current name: {project.ProjectName}. Enter new name or press enter to skip");
                            if (!string.IsNullOrEmpty(newname)) {
                                
                                project.ProjectName = newname;
                            }

                        }else if(field == "Description") {
                            
                            string? newdescription = userInput($"Current description: {project.ProjectDescription}. Enter new description or press enter to skip");
                            if (!string.IsNullOrEmpty(newdescription)) {
                                
                                project.ProjectDescription = newdescription;
                            }
                                
                        }else if(field == "Category") {
                                    
                            var newcategory = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title($"Current category: {project.Category.ProjectCategory}. Select new category")
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
                                
                        }else if(field == "Target Completion Date") {
                                    
                            string? userdate = userInput($"Current target date: {project.ProjectCompletionDate.ToShortDateString()}. Enter new date (MM/DD/YYYY) or press enter to skip:");
                                if(!string.IsNullOrEmpty(userdate)) {
                                if (DateTime.TryParse(userdate, out DateTime newdate)) {
                                    project.ProjectCompletionDate = newdate;
                                        }else {
                                            AnsiConsole.MarkupLine($"[red]Invalid format[/]");
                                        }
                                    }              
                                }
                            } while (field!= "Done") ; 
                projectOrganizer.SaveAlltoFile();
                AnsiConsole.MarkupLine($"[green]{project.ProjectName} was updated and saved [/]");
            }
        }
    }
    public static string? userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}