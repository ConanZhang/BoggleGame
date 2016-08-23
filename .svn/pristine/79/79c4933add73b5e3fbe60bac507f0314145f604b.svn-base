// Assignment implementation written by April Martin & Conan Zhang
// for CS3500 Assignment #8. November, 2014.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BoggleClient
{
    /// <summary>
    /// Launches Boggle Server using command line parameters.
    /// </summary>
    class BoggleLauncher
    {
        static void Main(string[] args)
        {
            //If we don't receive enough or receive too many arguments
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Please submit only 2 or 3 arguments! >:D");
                Console.ReadLine();
            }
            else
            {
                int arg0 = 0;
                // Only start a new game if the time is a valid integer.
                if (!int.TryParse(args[0], out arg0))
                {
                    Console.WriteLine("Please enter an integer for the time. -____-");
                    Console.ReadLine();
                }
                else if (arg0 <= 0)
                {
                    Console.WriteLine("You can't play in negative time. Duh.");
                    Console.ReadLine();
                }
                else
                {

                    try
                    {
                        HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(args[1]));
                        if (args.Length == 2)
                        {
                            new BoggleServer(arg0, dictionary);
                        }
                        else if (args.Length == 3)
                        {
                            new BoggleServer(arg0, dictionary, args[2]);
                        }

                        Console.ReadLine();
                    }
                    catch
                    {
                        Console.WriteLine("Your file is screwed up stupid. =P");
                        Console.ReadLine();
                    }
                }
            }
            
        }
    }
}
