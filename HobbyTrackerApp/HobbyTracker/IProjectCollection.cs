namespace HobbyTracker;

public interface IProjectCollection {
        
        List<ProjectData> LoadProjects();
        void SaveAllProjects(List<ProjectData> projects);
        void SaveONEProject(ProjectData project);
    }