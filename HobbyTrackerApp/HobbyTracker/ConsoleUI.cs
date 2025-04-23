namespace HobbyTracker;

using System.IO;
using System.IO.Compression;
using Spectre.Console;

public class ConsoleUI {
    
    private readonly User loginChoice;
    private readonly ProjectOrganizer userOrganizer;
    private readonly FilteringService filteringService;
    private readonly EditingService editingService;
    private readonly AddService addService;
    private readonly ArchiveService archiveService;
    private readonly RemoveService removeService;


    public ConsoleUI(User loginChoice, ProjectOrganizer userOrganizer) {
        this.loginChoice =loginChoice;
        this.userOrganizer = userOrganizer;
        this.filteringService = new FilteringService(userOrganizer);
        this.editingService = new EditingService(userOrganizer);

    }

    public void View() {
        
        while (true) {
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices(new[] {
                    "Add project", "View projects", "Edit project", "Archive", "Remove", "Exit"
                })
            );
                        
            switch(action) {

                case "Add project":
                    addService.AddProject();
                    break;

                case "View projects":
                    filteringService.Showfilteredprojects();
                    break;
                
                case "Edit project":
                    editingService.EditProject();
                    break;

                case "Archive":
                    archiveService.ArchiveProject();
                    break;
                
                case "Remove":
                    removeService.RemoveProject();
                    break;
                
                case "Exit":
                    return;
            }
        }
    }  
     
    public static string userInput(string message) {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
}