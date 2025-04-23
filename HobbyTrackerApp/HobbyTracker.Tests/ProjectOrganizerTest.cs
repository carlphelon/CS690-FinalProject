namespace HobbyTracker.Tests;

using HobbyTracker;
using System;
using Xunit;
using Progress = Progress;
using System.Linq;

public class ProjectOrganizerTests {
    private ProjectOrganizer? testprojectorganizer = null;

    [Fact]
    public void Test_SaveONEProject() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category = new Category("Blog idea");
        var testproject = new ProjectData("test name", "test description for testing purposes", category, Progress.InProgress, Priority.HighPriority, DateTime.Today.AddDays(365), user );

        //save to organizer - do the thing
        testprojectorganizer.SaveProject(testproject);

        Assert.Single(storage.savedprojects);
        Assert.Equal("test name", storage.savedprojects[0].ProjectName);
    }
    [Fact]
    public void Test_SaveMULTIPLEProjects() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category = new Category("Blog idea");
        var testproject2 = new ProjectData("testing 123", "test test test", category, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );
        var testproject3 = new ProjectData("testing 123", "test test test", category, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );

        //save to organizer
        testprojectorganizer.SaveProject(testproject2);
        testprojectorganizer.SaveProject(testproject3);

        Assert.Equal(2,testprojectorganizer.GetAllProjects().Count());
    } 

    [Fact]
    public void Test_filteringbycategory() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category1 = new Category("Blog idea");
        var category2 = new Category("Short Story");
        var testproject2 = new ProjectData("testing 123", "test test test", category1, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );
        var testproject3 = new ProjectData("testing 456", "test^3", category2, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );
        
        testprojectorganizer.SaveProject(testproject2);
        testprojectorganizer.SaveProject(testproject3);

        //filter - do the thing
        var filteredprojects = testprojectorganizer.FilterByCategory("Short Story");

        Assert.Single(filteredprojects);
        Assert.Equal("testing 456", filteredprojects.First().ProjectName);
    }
    [Fact]
    public void Test_filterNoresult() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category1 = new Category("Blog idea");
        var testproject1 = new ProjectData("testing 123", "test test test", category1, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );

        testprojectorganizer.SaveProject(testproject1);

        //filter - choose non existing priority
        var filteredprojects = testprojectorganizer.FilterByPriority("Medium Priority");

        Assert.Empty(filteredprojects);
    }  
    [Fact]
    public void Test_RemoveProject() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category1 = new Category("Blog idea");
        var category2 = new Category("Short Story");
        var testproject1 = new ProjectData("testing 123", "test test test", category1, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );
        var testproject2 = new ProjectData("testing 456", "test^3", category2, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );

        testprojectorganizer.SaveProject(testproject1);
        testprojectorganizer.SaveProject(testproject2);

        //remove project - do the thing
        testprojectorganizer.RemoveProject("testing 456");

        Assert.Single(storage.savedprojects);
        Assert.DoesNotContain(storage.savedprojects, p => p.ProjectName =="testing 456");
    }  
    [Fact]
    public void Test_FilterByPriority() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category1 = new Category("Blog idea");
        var category2 = new Category("Short Story");
        var testproject1 = new ProjectData("testing 123", "test test test", category1, Progress.NotStarted, Priority.MediumPriority, DateTime.Today.AddDays(310), user );
        var testproject2 = new ProjectData("testing 456", "test^3", category2, Progress.Started, Priority.LowPriority, DateTime.Today.AddDays(310), user );
        var testproject3 = new ProjectData("testing 789", "test*2-1", category2, Progress.InProgress, Priority.MediumPriority, DateTime.Today.AddDays(310), user );

        testprojectorganizer.SaveProject(testproject1);
        testprojectorganizer.SaveProject(testproject2);
        testprojectorganizer.SaveProject(testproject3);


        //filter - do the thing
        var filteredprojectlist = testprojectorganizer.FilterByPriority("MediumPriority");

        Assert.Equal(2, filteredprojectlist.Count());
        Assert.All(filteredprojectlist, p => Assert.Equal(Priority.MediumPriority, p.Priority));
    }  
}
