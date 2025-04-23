namespace HobbyTracker.Tests;

using HobbyTracker;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using Xunit;
using Progress = Progress;

public class FilteringServiceTest {

    private readonly FilteringService? filteringService;
    private readonly ProjectOrganizer? testprojectorganizer;

    public FilteringServiceTest() {

        var storage = new ProjectCollection();
        var testprojectorganizer = new ProjectOrganizer(storage);
        filteringService = new FilteringService(testprojectorganizer);
        
        var user = new User("Sophia");
        var category = new Category("Blog idea");
        var testproject = new ProjectData("test name", "test description for testing purposes", category, Progress.inProgress, Priority.highPriority, DateTime.Today.AddDays(365), user );

        //save to organizer - do the thing
        testprojectorganizer.saveProject(testproject);
    }

    [Fact]
    public void Test_FilterByProgress() {

        var filtered = filteringService.FilterByProgress("inProgress");
        Assert.Single(filtered);
        Assert.Equal("test name", filtered[0].projectName);
    }

    [Fact]
    public void Test_FilterByCategory() {

        var filtered = filteringService.FilterByCategory("Blog idea");
        Assert.Single(filtered);
        Assert.Equal("test name", filtered[0].projectName);
    }

    [Fact]
    public void Test_FilterByPriority() {

        var filtered = filteringService.FilterByPriority("highPriority");
        Assert.Single(filtered);
        Assert.Equal("test name", filtered[0].projectName);
    }

}