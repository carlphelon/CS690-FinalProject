namespace HobbyTracker;

using Spectre.Console;
using System.IO;
using System;
using System.Collections.Generic;

public class ProjectOrganizer {
    //initialize variables
    private readonly string file;
    private List<ProjectData> projects;
    private const string Delimiter = "|||";

    //create organizer and pull from file
    public ProjectOrganizer(string file) {
        this.file=file;
        this.projects= new List<ProjectData>();
        
        LoadfromFile();
    }

    public ProjectOrganizer() : this("Guest-Project.txt") {}

    private void LoadfromFile() {

        //problem handle
        if (!File.Exists(file)) return;
        
        var filelines=File.ReadAllLines(file);
        
        foreach (var line in filelines) {
            var sections = line.Split(Delimiter);
            
            //add skip for malformed lines
            if (sections.Length <6) {
                AnsiConsole.MarkupLine($"[red]Skipping malformed line: {line}[/]");
                continue;
            }
            var projectName = sections[0];
            var projectDescription = sections[1].Trim();
            var category =  new Category(sections[2].Trim());
            var progress = Enum.Parse<Progress>(sections[3].Trim());
            var priority = Enum.Parse<Priority>(sections[4].Trim());
            var completionDate = DateTime.Parse(sections[5].Trim());
            var isArchived = sections.Length > 6 && bool.TryParse(sections[6].Trim(), out bool parsed) ? parsed : false;    

            var project = new ProjectData(projectName, projectDescription, category, progress, priority, completionDate, new User("Sophia"), isArchived);
            projects.Add(project);
        }
    }

    public void saveProject(ProjectData project) {
    
    //add isacrchived                    at end 
    var projectLines = string.Join(Delimiter, new[] {
        project.projectName, 
        project.projectDescription,
        project.Category.projectCategory,
        project.Progress.ToString(),
        project.Priority.ToString(),
        project.projectCompletionDate.ToShortDateString(),
        project.isArchived.ToString()
    });

    File.AppendAllText(file, projectLines + Environment.NewLine);
    projects.Add(project);
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

    public void SaveAlltoFile() {
        var alllines = projects.Select(p=> string.Join(Delimiter, new[] {
            p.projectName, 
            p.projectDescription,
            p.Category.projectCategory,
            p.Progress.ToString(),
            p.Priority.ToString(),
            p.projectCompletionDate.ToShortDateString()
    })
    );
        File.WriteAllLines(file, alllines);
    }
}




