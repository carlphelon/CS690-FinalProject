namespace HobbyTracker;

using System.IO;
using System;
using System.Collections.Generic;

public class ProjectOrganizer {
    //initialize variables
    private readonly string file;
    private List<ProjectData> projects;

    //create organizer and pull from file
    public ProjectOrganizer(string file) {
        this.file=file;
        this.projects= new List<ProjectData>();
        
        LoadfromFile();
    }

    public ProjectOrganizer() : this("Guest-Project.txt") {}

    private void LoadfromFile() {
        
        var filelines=File.ReadAllLines(file);
        
        foreach (var line in filelines) {
            var sections = line.Split(',');

            var projectName = sections[0];
            var projectDescription = sections[1];
            var category =  new Category(sections[2]);
            var progress = Enum.Parse<Progress>(sections[3]);
            var priority = Enum.Parse<Priority>(sections[4]);
            var completionDate = DateTime.Parse(sections[5]);

            var project = new ProjectData(projectName, projectDescription, category, progress, priority, completionDate, new User("Sophia"));
            projects.Add(project);
        }
    }

    public void saveProject(ProjectData project) {
    
    var projectLines = $"{project.projectName} , {project.projectDescription} , {project.Category.projectCategory} , {project.Progress} , {project.Priority} , {project.projectCompletionDate}";

    File.AppendAllText(file, projectLines + Environment.NewLine);
    projects.Add(project);
    }

    public void displayALLProjects() {
    foreach (var project in projects) {
        Console.WriteLine($"{project.projectName} , {project.projectDescription} , {project.Category.projectCategory} , {project.Progress} , {project.Priority} , {project.projectCompletionDate}");
    }
    }
}




