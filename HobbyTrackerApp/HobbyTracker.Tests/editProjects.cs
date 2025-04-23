namespace HobbyTracker.Tests;

using HobbyTracker;
using System;
using Xunit;
using Progress = Progress;

public class EditingServiceTest {

    [Fact]
    public void editFields() {
        var testservice = new EditingService(new ProjectOrganizer(new ProjectCollection()));
        var project = new ProjectData("oldprojectname", "old description", new Category("Short story"), Progress.inProgress, Priority.highPriority, DateTime.Today, new User("Sophia"));

        var options = new editOptions {

            newName= "newname", 
            newDescription="new description",
            newCategory ="Blog idea",
            newProgress = Progress.Finished,
            newPriority = Priority.lowPriority,
            newprojectCompletionDate = DateTime.Today.AddDays(14)
        };

        testservice.ApplyEdit(project, options);

        Assert.Equal("newname", project.projectName);
        Assert.Equal("new description", project.projectDescription);
        Assert.Equal("Blog idea", project.Category.projectCategory);
        Assert.Equal(Progress.Finished, project.Progress);
        Assert.Equal(Priority.lowPriority, project.Priority);
        Assert.Equal(DateTime.Today.AddDays(14), project.projectCompletionDate);
    
    }
}