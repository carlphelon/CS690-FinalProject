namespace HobbyTracker;

using static System.Console;
using System.Dynamic;

public class User {
    public string userName { get; }

    public User(string name) {
        this.userName = name;
    }
}
public class Category {
    public string projectCategory { get; }

    public Category(string type) {
        this.projectCategory = type;
    }
}

public enum Progress {
    notStarted,
    Started,
    inProgress,
    Finished
}
public enum Priority {
    highPriority=1,
    mediumPriority=2,
    lowPriority=3
}

public class ProjectData {
    public string projectName { get; set; }
    public string projectDescription { get; set; }
    public Category Category { get; set; }
    public Progress Progress { get; set; }
    public Priority Priority { get; set;}
    public DateTime projectCompletionDate { get; set; }
    public User User { get; set; }
    
    public bool isArchived { get; set; } = false;

    public ProjectData(string projectname, string projectdescription, Category category, Progress progress, Priority priority, DateTime projectcompletiondate, User user, bool isarchived = false) {
        this.projectName = projectname;
        this.projectDescription = projectdescription;
        this.Category = category;
        this.Progress = progress;
        this.Priority = priority;
        this.projectCompletionDate = projectcompletiondate;
        this.User = user;
        this.isArchived = isarchived;                      
    }   
}
