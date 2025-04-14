namespace HobbyTracker;

using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Please select action (add, edit, archive, remove): ");
        string action = Console.ReadLine();

        if(action== "add") {

            string command;

            do {

                Console.WriteLine("Enter project title: ");
                string projectName = Console.ReadLine();

                Console.WriteLine("Enter project details: ");
                string projectDetails = Console.ReadLine();

                Console.WriteLine("Select project category (Blog idea, Drawing, Short story): ");
                string projectCategory = Console.ReadLine();

                File.AppendAllText("project-data.txt",projectName+" ("+projectCategory+")"+": "+projectDetails+Environment.NewLine);

                Console.WriteLine("Add another project or exit: " );
                command = Console.ReadLine();

            } while(command!="exit");
        }
    }
}
