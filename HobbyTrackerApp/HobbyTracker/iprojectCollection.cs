namespace HobbyTracker;

public interface iprojectCollection {
        
        List<ProjectData> LoadProjects();
        void SaveAllProjects(List<ProjectData> projects);
        void SaveONEProject(ProjectData project);
    }