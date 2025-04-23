namespace HobbyTracker;

using static System.Console;
using System.Dynamic;

public class User {
    public string UserName { get; }

    public User(string name) {
        UserName = name;
    }
}
public class Category {
    public string ProjectCategory { get; }

    public Category(string type) {
        ProjectCategory = type;
    }
}

public enum Progress {
    NotStarted,
    Started,
    InProgress,
    Finished
}
public enum Priority {
    HighPriority=1,
    MediumPriority=2,
    LowPriority=3
}

public class ProjectData {
    public string ProjectName { get; set; }
    public string ProjectDescription { get; set; }
    public Category Category { get; set; }
    public Progress Progress { get; set; }
    public Priority Priority { get; set;}
    public DateTime ProjectCompletionDate { get; set; }
    public User User { get; set; }
    
    public bool IsArchived { get; set; } = false;

    public ProjectData(string projectname, string projectdescription, Category category, Progress progress, Priority priority, DateTime projectcompletiondate, User user, bool isarchived = false) {
        this.ProjectName = projectname;
        this.ProjectDescription = projectdescription;
        this.Category = category;
        this.Progress = progress;
        this.Priority = priority;
        this.ProjectCompletionDate = projectcompletiondate;
        this.User = user;
        this.IsArchived = isarchived;                      
    }   
}
