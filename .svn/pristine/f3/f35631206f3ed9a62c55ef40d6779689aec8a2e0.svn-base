using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BoggleClient;

namespace BoggleClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Make a Boggle server to connect to
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleServer bs = new BoggleServer(60, dictionary);
            // Make a web server to connect to
            WebServer ws = new WebServer();

            // Make console window
            ConsoleManager.Show();

            //Make another client to destroy in a duel to the death of Boggle
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
