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

        //set up sophias projects - store file be tween logins
        if(loginScreen== "Sophia's Projects") {

            loginChoice = new User("Sophia");
            userOrganizer = new ProjectOrganizer("Sophia-Projects.txt");
        
        } else if(loginScreen=="Guest Projects") {  //guest acct setup - temp file

            loginChoice = new User("Guest");
            userOrganizer = new ProjectOrganizer();
        }   
        
        ConsoleUI appUI = new ConsoleUI(loginChoice,userOrganizer);
        appUI.View();
    

        Console.WriteLine("Have a wonderful day :)");
    }
}


