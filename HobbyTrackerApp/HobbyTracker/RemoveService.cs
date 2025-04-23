namespace HobbyTracker;

using Spectre.Console;
using System.Linq;

public class RemoveService {

    private readonly ProjectOrganizer projectOrganizer;
    private readonly User loginChoice;

    public RemoveService(ProjectOrganizer projectOrganizer, User loginChoice) {
        this.projectOrganizer = projectOrganizer;
        this.loginChoice = loginChoice;
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
        
    }     
}