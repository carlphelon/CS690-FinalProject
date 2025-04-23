namespace HobbyTracker;

using System.IO;
using Spectre.Console;

class Program
{
    //Refactor here and move UI
    static void Main(string[] args)
    {
        //start up UI
        var loginScreen = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Welcome to Hobby Tracker, select option to continue:")
            .AddChoices(new[] {
                "Sophia's Projects", "Guest Projects"
            }));
        
        //initialize variables
        User? loginChoice = null;
        ProjectOrganizer? userOrganizer = null;

        //set up sophias projects - store file be 
        // tween logins
        
        switch(loginScreen) {

            case "Sophia's Projects":
            
                loginChoice = new User("Sophia");
                var sophiaStorage = new ProjectCollection("Sophia-Projects.txt");
                userOrganizer = new ProjectOrganizer(sophiaStorage);
                break;

            case "Guest Projects":

                loginChoice = new User("Guest");
                var guestStorage = new ProjectCollection("Guest-Projects.txt");
                userOrganizer = new ProjectOrganizer(guestStorage);
                break;
        }
        

        ConsoleUI appUI = new ConsoleUI(loginChoice,userOrganizer);
        appUI.View();
    

        Console.WriteLine("Have a wonderful day :)");
    }
}


