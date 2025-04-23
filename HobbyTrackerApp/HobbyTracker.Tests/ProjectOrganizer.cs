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
        var testproject = new ProjectData("test name", "test description for testing purposes", category, Progress.inProgress, Priority.highPriority, DateTime.Today.AddDays(365), user );

        //save to organizer - do the thing
        testprojectorganizer.saveProject(testproject);

        Assert.Single(storage.savedprojects);
        Assert.Equal("test name", storage.savedprojects[0].projectName);
    }
    [Fact]
    public void Test_SaveMULTIPLEProjects() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category = new Category("Blog idea");
        var testproject2 = new ProjectData("testing 123", "test test test", category, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );
        var testproject3 = new ProjectData("testing 123", "test test test", category, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );

        //save to organizer
        testprojectorganizer.saveProject(testproject2);
        testprojectorganizer.saveProject(testproject3);

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
        var testproject2 = new ProjectData("testing 123", "test test test", category1, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );
        var testproject3 = new ProjectData("testing 456", "test^3", category2, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );
        
        testprojectorganizer.saveProject(testproject2);
        testprojectorganizer.saveProject(testproject3);

        //filter - do the thing
        var filteredprojects = testprojectorganizer.FilterByCategory("Short Story");

        Assert.Single(filteredprojects);
        Assert.Equal("testing 456", filteredprojects.First().projectName);
    }
    [Fact]
    public void Test_filterNoresult() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category1 = new Category("Blog idea");
        var testproject1 = new ProjectData("testing 123", "test test test", category1, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );

        testprojectorganizer.saveProject(testproject1);

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
        var testproject1 = new ProjectData("testing 123", "test test test", category1, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );
        var testproject2 = new ProjectData("testing 456", "test^3", category2, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );

        testprojectorganizer.saveProject(testproject1);
        testprojectorganizer.saveProject(testproject2);

        //remove project - do the thing
        testprojectorganizer.RemoveProject("testing 456");

        Assert.Single(storage.savedprojects);
        Assert.DoesNotContain(storage.savedprojects, p => p.projectName =="testing 456");
    }  
    [Fact]
    public void Test_FilterByPriority() {
        
        //initiate variables
        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        var user = new User("Sophia");
        var category1 = new Category("Blog idea");
        var category2 = new Category("Short Story");
        var testproject1 = new ProjectData("testing 123", "test test test", category1, Progress.notStarted, Priority.mediumPriority, DateTime.Today.AddDays(310), user );
        var testproject2 = new ProjectData("testing 456", "test^3", category2, Progress.Started, Priority.lowPriority, DateTime.Today.AddDays(310), user );
        var testproject3 = new ProjectData("testing 789", "test*2-1", category2, Progress.inProgress, Priority.mediumPriority, DateTime.Today.AddDays(310), user );

        testprojectorganizer.saveProject(testproject1);
        testprojectorganizer.saveProject(testproject2);
        testprojectorganizer.saveProject(testproject3);


        //filter - do the thing
        var filteredprojectlist = testprojectorganizer.FilterByPriority("mediumPriority");

        Assert.Equal(2, filteredprojectlist.Count());
        Assert.All(filteredprojectlist, p => Assert.Equal(Priority.mediumPriority, p.Priority));
    }  
}
