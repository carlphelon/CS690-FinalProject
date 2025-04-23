namespace HobbyTracker.Tests;

using System;
using System.IO;
using System.Collections.Generic;

public class ProjectCollection: IProjectCollection {
    
    public List<ProjectData> savedprojects = new();
    public bool saveallcalled = false;

    public List<ProjectData> LoadProjects() {

        return new List<ProjectData>(savedprojects);
    }

    public void SaveAllProjects(List<ProjectData> projects) { 
        
        saveallcalled = true;
        savedprojects= new List<ProjectData>(projects);
    }

    public void SaveONEProject(ProjectData project) {
        
        savedprojects.Add(project);
        
    }
}