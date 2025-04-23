namespace HobbyTracker;

using Spectre.Console;
using System.Linq;

public class RemoveService {

    private readonly ProjectOrganizer projectOrganizer;

    public RemoveService(ProjectOrganizer projectOrganizer) {
        this.projectOrganizer = projectOrganizer;
    }

    public void RemoveProject() {
        
        var projectList = projectOrganizer.GetAllProjects();

        if(projectList.Count() == 0) {

            AnsiConsole.MarkupLine($"[yellow]No projects stored, add a project to begin [/]");
            return;
        }

        var projectToRemove = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select a project to remove:")
            .AddChoices(projectList.Select(p => p.ProjectName))
        );

        projectOrganizer.RemoveProject(projectToRemove);
        AnsiConsole.MarkupLine($"[red]{projectToRemove} was removed [/]");
    }     
}