namespace HobbyTracker;

using System;
using System.IO;
using System.Collections.Generic;

public class ProjectCollection: IProjectCollection {

    private readonly string file;
    private const string Delimiter = "|||";

    public ProjectCollection(string fileName) {

        file = fileName;
    }

    public List<ProjectData> LoadProjects() {

        var projects = new List<ProjectData>();

        if (!File.Exists(file)) {
            return projects;
        }
        
        var filelines=File.ReadAllLines(file);
        
        foreach (var line in filelines) {
            
            var sections = line.Split(Delimiter);
            
            //skip malformed lines
            if (sections.Length <6) {
                Console.WriteLine($"Skipping malformed line: {line}");
                continue;
            }

            var projectName = sections[0];
            var projectDescription = sections[1].Trim();
            var category =  new Category(sections[2].Trim());
            var progress = Enum.Parse<Progress>(sections[3].Trim());
            var priority = Enum.Parse<Priority>(sections[4].Trim());
            var completionDate = DateTime.Parse(sections[5].Trim());
            var isArchived = sections.Length > 6 && bool.TryParse(sections[6].Trim(), out bool parsed) ? parsed : false;    

            projects.Add(new ProjectData(projectName, projectDescription, category, progress, priority, completionDate, new User("Sophia"), isArchived));    
        }
        return projects;
    }

    public void SaveAllProjects(List<ProjectData> projects) {
        
        
        var projectLines = new List<string>();
        foreach(var p in projects) {

            projectLines.Add(string.Join(Delimiter, new[] {
                p.ProjectName, 
                p.ProjectDescription,
                p.Category.ProjectCategory,
                p.Progress.ToString(),
                p.Priority.ToString(),
                p.ProjectCompletionDate.ToShortDateString(),
                p.IsArchived.ToString()
            }));
        }
        File.WriteAllLines(file, projectLines);     
    }
    
    public void SaveONEProject(ProjectData p) {
        
        var projectLines= string.Join(Delimiter, new[] {
                p.ProjectName, 
                p.ProjectDescription,
                p.Category.ProjectCategory,
                p.Progress.ToString(),
                p.Priority.ToString(),
                p.ProjectCompletionDate.ToShortDateString(),
                p.IsArchived.ToString()
            });       
        File.AppendAllText(file, projectLines + Environment.NewLine); 
    }

    
}