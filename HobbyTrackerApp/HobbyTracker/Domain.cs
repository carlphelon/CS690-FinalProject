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
    public string projectName { get; }
    public string projectDescription { get; }
    public Category Category { get; }
    public Progress Progress { get; }
    public Priority Priority { get; }
    public DateTime projectCompletionDate { get; }
    public User User { get; }

    public ProjectData(string projectname, string projectdescription, Category category, Progress progress, Priority priority, DateTime projectcompletiondate, User user) {
        this.projectName = projectname;
        this.projectDescription = projectdescription;
        this.Category = category;
        this.Progress = progress;
        this.Priority = priority;
        this.projectCompletionDate = projectcompletiondate;
        this.User = user;
    }
}
