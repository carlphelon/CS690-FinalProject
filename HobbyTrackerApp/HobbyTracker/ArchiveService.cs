namespace HobbyTracker;

using Spectre.Console;
using System.Linq;

public class ArchiveService {

    private readonly ProjectOrganizer projectOrganizer;
    private readonly User? loginChoice;

    public ArchiveService(ProjectOrganizer projectOrganizer, User loginChoice) {
        this.projectOrganizer = projectOrganizer;
        this.loginChoice = loginChoice;
    }

    public void ArchiveProject() {

        var projectList = projectOrganizer.GetAllProjects().Where(p => !p.IsArchived).ToList(); 

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

                    projectOrganizer.SaveAlltoFile(); 

                    AnsiConsole.MarkupLine($"[green]{project.ProjectName} was archived[/]"); 

                }   
            } 
    }
}