using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoggleClient
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// The Startup page is the first page the user sees. It allows them to enter a name and IP address,
    /// to connect to the server, and to cancel an in-progress connection.
    /// </summary>
    public partial class StartupPage : Page
    {
        BoggleClientModel model;

        /// <summary>
        /// Constructs the Startup page. 
        /// </summary>
        /// <param name="model"></param>
        public StartupPage(BoggleClientModel model)
        {
            InitializeComponent();
            this.model = model;
            this.model.StartEvent += StartReceived;
            this.model.IgnoringEvent += IgnoringReceived;
        }

        /// <summary>
        /// When the player clicks the Connect button:
        /// Connect to the server using the name and IP address entered by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            model.Connect(IPTextBox.Text, NameTextBox.Text);
            Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = "Searching for an opponent.";
            });
        }

        /// <summary>
        /// When the player clicks the Disconnect button:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = "Cancelling connection.";
            });
            model.closeSocket();
        }
        
        /// <summary>
        /// Got a message that did not meet protocol.
        /// </summary>
        /// <param name="line"></param>
        private void IgnoringReceived(String line)
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = "Invalid input received.";
            });
        }

        /// <summary>
        /// When the model receives a message and sends an IncomingLineEvent:
        /// If it's the START message, navigate to the Game page.        
        /// <param name="line"></param>
        /// </summary>
        private void StartReceived(String boardSetup, String opponent)
        {
            Dispatcher.BeginInvoke((Action)(() =>  {
                GamePage game = new GamePage(model, boardSetup, opponent);

                if (NavigationService != null)
                {
                    NavigationService.Navigate(game);
                }
            }));
        }
    }
}
