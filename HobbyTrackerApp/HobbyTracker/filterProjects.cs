namespace HobbyTracker;
using Spectre.Console;
using System;
using System.Collections.Generic;

public class FilteringService {
    
    private readonly ProjectOrganizer? projectorganizer;

    public FilteringService(ProjectOrganizer? projectOrganizer) {
        
        projectorganizer = projectOrganizer;
    }

    public void Showfilteredprojects() {
        
        if(projectorganizer ==null) {
            AnsiConsole.MarkupLine($"[red]Projects not available [/]");
            return;
        }

        var filterChoice = AnsiConsole.Prompt ( 
            new SelectionPrompt<string>() 
            .Title("Select the view you would like to see:") 
            .AddChoices(new[] {"All projects", "By category", "By priority", "By progress", "Archived"})
        );
        
        List<ProjectData> filteredProjects = new(); 

        if (filterChoice == "By progress") {  
            var progressFilter = AnsiConsole.Prompt(  
                new SelectionPrompt<string>()  
                    .Title("Select progress:")  
                    .AddChoices(Enum.GetNames(typeof(Progress))));  

            filteredProjects = projectorganizer.FilterByProgress(progressFilter);  

        }else if (filterChoice == "By category") {  

            var categoryFilter = AnsiConsole.Prompt(  
                new SelectionPrompt<string>()  
                    .Title("Select category:")  
                    .AddChoices(new[] { "Blog idea", "Drawing", "Short story" }));  

            filteredProjects = projectorganizer.FilterByCategory(categoryFilter);  

        } else if (filterChoice == "By priority") {  

            var priorityFilter = AnsiConsole.Prompt(  
                new SelectionPrompt<string>()  
                    .Title("Select priority:")  
                    .AddChoices(Enum.GetNames(typeof(Priority))));  

            filteredProjects = projectorganizer.FilterByPriority(priorityFilter); 

        } else if (filterChoice == "Archived") { 

            filteredProjects = projectorganizer.GetAllProjects().Where(p => p.isArchived).ToList(); 

        } else if (filterChoice == "All projects")  {  

            filteredProjects = projectorganizer.GetAllProjects();  
        }  
             
        if (filteredProjects.Count > 0)  {  

            projectorganizer.projectTableformatting(filteredProjects);  

        } else {  
                AnsiConsole.MarkupLine("[yellow]No projects match the selected filter.[/]");  
        }  
        
    }   
}
