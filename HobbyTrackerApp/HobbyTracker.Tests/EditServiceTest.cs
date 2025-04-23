namespace HobbyTracker.Tests;

using HobbyTracker;
using System;
using Xunit;
using Progress = Progress;

public class EditingServiceTest {

    [Fact]
    public void editFields() {
        var testservice = new EditingService(new ProjectOrganizer(new ProjectCollection()));
        var project = new ProjectData("oldprojectname", "old description", new Category("Short story"), Progress.InProgress, Priority.HighPriority, DateTime.Today, new User("Sophia"));

        var options = new EditOptions {

            NewName= "newname", 
            NewDescription="new description",
            NewCategory ="Blog idea",
            NewProgress = Progress.Finished,
            NewPriority = Priority.LowPriority,
            NewProjectCompletionDate = DateTime.Today.AddDays(14)
        };

        testservice.ApplyEdit(project, options);

        Assert.Equal("newname", project.ProjectName);
        Assert.Equal("new description", project.ProjectDescription);
        Assert.Equal("Blog idea", project.Category.ProjectCategory);
        Assert.Equal(Progress.Finished, project.Progress);
        Assert.Equal(Priority.LowPriority, project.Priority);
        Assert.Equal(DateTime.Today.AddDays(14), project.ProjectCompletionDate);
    
    }
}