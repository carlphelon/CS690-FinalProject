namespace HobbyTracker;

using Spectre.Console;
using System.Linq;
using System;
using System.Collections.Generic;

public class ProjectOrganizer { 

    private List<ProjectData> projects; 
    private readonly IProjectCollection storage;

        public ProjectOrganizer(IProjectCollection storage) {
        
            this.storage=storage;
            projects= storage.LoadProjects();
        }

    public void SaveProject(ProjectData project) {
        storage.SaveONEProject(project);
        projects.Add(project);
    }

    public void SaveAlltoFile() {
        storage.SaveAllProjects(projects);
    }
    
    public void DisplayALLProjects() {
        
        if (projects.Count== 0) {
            
            AnsiConsole.MarkupLine("[yellow]No projects to display.[/]");
            return;
        }

        projectTableformatting(projects);
    }
    public List<ProjectData> FilterByProgress(string progress) { 
        return projects.Where(p => p.Progress.ToString() == progress).ToList(); 
    } 

    public List<ProjectData> FilterByCategory(string category) { 
        return projects.Where(p => p.Category.ProjectCategory == category).ToList(); 
    } 

    public List<ProjectData> FilterByPriority(string priority) { 
        return projects.Where(p => p.Priority.ToString() == priority).ToList(); 
    } 

    public List<ProjectData> GetAllProjects() => new(projects);
    
    //clean formatting of project display
    public void projectTableformatting(List<ProjectData> listofprojects, string tableTitle = "[bold]See your projects below[/]" ) {

        //initialize table
        var table = new Table() {
            Border = TableBorder.Square,
            Title = new TableTitle(tableTitle)
        };

        //build out table
        table.AddColumn("[bold]Project Title[/]");
        table.AddColumn("Category");
        table.AddColumn("Progress");
        table.AddColumn("Priority");
        table.AddColumn("Target Completion Date");
        table.AddColumn("Description");
        table.AddColumn("Archived?");                  

        foreach (var project in listofprojects) {
            
            table.AddRow(
                $"[bold]{project.ProjectName}[/]",
                project.Category?.ProjectCategory ?? "N/A",
                project.Progress.ToString(),
                project.Priority.ToString(),
                project.ProjectCompletionDate.ToShortDateString(),
                project.ProjectDescription ?? "",
                project.IsArchived ? "Yes" : "No"
            );
        }
        AnsiConsole.Write(table);
    }

    public void RemoveProject(string projectTitle) {
        var projectToRemove = projects.FirstOrDefault(p => p.ProjectName.Equals(projectTitle, StringComparison.OrdinalIgnoreCase));

    if (projectToRemove!= null) {
        projects.Remove(projectToRemove);
        SaveAlltoFile();
        AnsiConsole.MarkupLine($"[red]Removed: {projectTitle} [/]");
    } else {
        AnsiConsole.MarkupLine($"[red]No project found [/]");
    }
    }
}


