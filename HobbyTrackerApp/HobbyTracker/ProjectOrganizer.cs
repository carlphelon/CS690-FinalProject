namespace HobbyTracker;

using Spectre.Console;
using System.IO;
using System;
using System.Collections.Generic;

public class ProjectOrganizer {
    
    private List<ProjectData> projects;
    private readonly iprojectCollection storage;

    public ProjectOrganizer(iprojectCollection storage) {
        this.storage=storage;
        this.projects= storage.LoadProjects();
    }

    public void saveProject(ProjectData project) {
        storage.SaveONEProject(project);
        projects.Add(project);
    }

    public void SaveAlltoFile() {
        storage.SaveAllProjects(projects);
    }
    
    public void displayALLProjects() {
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
        return projects.Where(p => p.Category.projectCategory == category).ToList(); 
    } 

    public List<ProjectData> FilterByPriority(string priority) { 
        return projects.Where(p => p.Priority.ToString() == priority).ToList(); 
    } 

    public List<ProjectData> GetAllProjects() { 
        return new List<ProjectData>(projects);

    } 
    

    //clean formatting of project display
    public void projectTableformatting(List<ProjectData> listofprojects, string tabletitle = "See your projects below" ) {
        //maybe make this its own codespace in next version:)

        //initialize table
        var table = new Table();
        table.Border(TableBorder.Square);
        table.Title = new TableTitle(tabletitle);

        //build out table
        table.AddColumn("[bold]Project Title[/]");
        table.AddColumn("Category");
        table.AddColumn("Progress");
        table.AddColumn("Priority");
        table.AddColumn("Target Completion Date");
        table.AddColumn("Description");
        //table.AddColumn("Archived");                  ver2

        foreach (var project in listofprojects) {
            table.AddRow(
                $"[bold]{project.projectName}[/]",
                project.Category?.projectCategory ?? "N/A",
                project.Progress.ToString(),
                project.Priority.ToString(),
                project.projectCompletionDate.ToShortDateString(),
                project.projectDescription ?? ""
            );
        }
        AnsiConsole.Write(table);
    }

    public void RemoveProject(string projecttitle) {
        var projectremove = projects.FirstOrDefault(p => p.projectName.Equals(projecttitle, StringComparison.OrdinalIgnoreCase));

    if (projectremove!= null) {
        projects.Remove(projectremove);
        SaveAlltoFile();
        AnsiConsole.MarkupLine($"[red]Removed: {projecttitle} [/]");
    } else {
        AnsiConsole.MarkupLine($"[red]No project found [/]");
    }
    }
}


