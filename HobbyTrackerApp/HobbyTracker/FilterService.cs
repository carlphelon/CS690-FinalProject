namespace HobbyTracker;
using Spectre.Console;
using System;
using System.Collections.Generic;

public class FilteringService {
    
    private readonly ProjectOrganizer projectOrganizer;

    public FilteringService(ProjectOrganizer? projectOrganizer) {
        
        this.projectOrganizer = projectOrganizer ??  throw new ArgumentNullException(nameof(projectOrganizer));
    }

    public List<ProjectData> FilterByProgress(string progress) => projectOrganizer.FilterByProgress(progress);
    public List<ProjectData> FilterByCategory(string category) => projectOrganizer.FilterByCategory(category);
    public List<ProjectData> FilterByPriority(string priority) => projectOrganizer.FilterByPriority(priority);
    public List<ProjectData> GetArchived(string progress) => projectOrganizer.FilterByProgress(progress);
    public List<ProjectData> FilterByProgress() => projectOrganizer.GetAllProjects().Where(p => p.IsArchived).ToList();
    public List<ProjectData> GetAllProjects() => projectOrganizer.GetAllProjects();


    public void Showfilteredprojects() {
        
        if(projectOrganizer ==null) {
            AnsiConsole.MarkupLine($"[red]Projects not available [/]");
            return;
        }

        var filterChoice = AnsiConsole.Prompt ( 
            new SelectionPrompt<string>() 
            .Title("Select the view you would like to see:") 
            .AddChoices(new[] {"All projects", "By category", "By priority", "By progress", "Archived"})
        );
        
        List<ProjectData> filteredProjects = new(); 

        switch (filterChoice) {
        
            case "By progress": 
                var progressFilter = AnsiConsole.Prompt(  
                    new SelectionPrompt<string>()  
                        .Title("Select progress:")  
                        .AddChoices(Enum.GetNames(typeof(Progress))));  
                filteredProjects = projectOrganizer.FilterByProgress(progressFilter);
                break;  

            case "By category":  
                var categoryFilter = AnsiConsole.Prompt(  
                    new SelectionPrompt<string>()  
                        .Title("Select category:")  
                        .AddChoices(new[] { "Blog idea", "Drawing", "Short story" }));  
                filteredProjects = projectOrganizer.FilterByCategory(categoryFilter); 
                break;  

            case "By priority": 
                var priorityFilter = AnsiConsole.Prompt(  
                    new SelectionPrompt<string>()  
                        .Title("Select priority:")  
                        .AddChoices(Enum.GetNames(typeof(Priority))));  
                filteredProjects = projectOrganizer.FilterByPriority(priorityFilter);
                break;  

            case "Archived":
                filteredProjects = projectOrganizer.GetAllProjects().Where(p => p.IsArchived).ToList(); 
                break; 

            case "All projects":
                filteredProjects = projectOrganizer.GetAllProjects();
                break;   
        }  
             
        if (filteredProjects.Count > 0)  {  

            projectOrganizer.projectTableformatting(filteredProjects);   
            
        } else {  
                AnsiConsole.MarkupLine("[yellow]No projects match the selected filter.[/]");  
        }  
        
    }   
}
