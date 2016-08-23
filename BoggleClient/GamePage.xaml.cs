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
    /// Interaction logic for GamePage.xaml
    /// The Game page is where the player actually plays Boggle. 
    /// It displays the remaining time, the current scores, the opponent's name, and the Boggle board. 
    /// It also contains a textbox where the user enters words.
    /// </summary>
    public partial class GamePage : Page
    {
        BoggleClientModel model;

        //Scores to send to summary
        string yourFinalScore;
        string theirFinalScore;

        /// <summary>
        /// Constructs the GamePage
        /// </summary>
        /// <param name="model"></param>
        public GamePage(BoggleClientModel model, String boardSetup, String opponent)
        {
            InitializeComponent();
            this.model = model;
            this.model.TerminatedEvent += terminatedReceived;
            this.model.StopEvent += stopReceived;
            this.model.IgnoringEvent += ignoringReceived;
            this.model.ScoreEvent += scoreReceived;
            this.model.TimeEvent += timeReceived;

            yourFinalScore = "";
            theirFinalScore = "";

            // Make an array out of the TextBlock controls,
            // then loop over it to set their Text values equal to the boardSetup string.
            TextBlock[] blocks = new TextBlock[]{   block1a, block1b, block1c, block1d, 
                                                    block2a, block2b, block2c, block2d, 
                                                    block3a, block3b,block3c, block3d,
                                                    block4a, block4b, block4c, block4d  };

            Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < 16; i++)
                {
                    if (boardSetup[i] == 'Q')
                    {
                        blocks[i].Text = "QU";
                        blocks[i].FontSize = 27;
                    }
                    else
                    {
                        blocks[i].Text = boardSetup[i].ToString();
                    }
                }

                // Display the opponent's name
                TheirNameTextBlock.Text = opponent;
            });
        }

        // When the player presses Enter with the textbox selected:
        // Send a new WORD message
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                model.SendMessage("WORD " + InputTextBox.Text);
                //Erase text box every time user sends a word
                Dispatcher.Invoke(() =>
                {
                    InputTextBox.Text = "";
                    StatusTextBlock.Text = "";
                });    
            }
        }

        // When the server sends a TIME update:
        // Update the time readout
        private void timeReceived(string time)
        {
            Dispatcher.Invoke(() =>
            {
                TimeTextBlock.Text = time;
            });
        }

        // When the server sends a SCORE update:
        // Update the score readout and the variables storing the score
        private void scoreReceived(string yourScore, string theirScore)
        {
            yourFinalScore = yourScore;
            theirFinalScore = theirScore;

            Dispatcher.Invoke(() =>
            {
                YourScoreTextBlock.Text = yourScore;
                TheirScoreTextBlock.Text = theirScore;
            });
        }

        // When the server sends an IGNORING message:
        // Display an error message
        private void ignoringReceived(string obj)
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = "Invalid input received.";
            });
        }

        // When the server sends a STOP message:
        // Close the socket and create a summary page, using the summary information given by the event.
        // Navigate to the summary.
        private void stopReceived(  string yourLegalCount, string yourLegalWords, string theirLegalCount, string theirLegalWords,
                                    string commonCount, string commonWords, string yourIllegalCount, string yourIllegalWords, 
                                    string theirIllegalCount, string theirIllegalWords)
        {
            this.model.closeSocket();

            Dispatcher.Invoke (() => 
            {
                SummaryPage sum = new SummaryPage(  model, yourFinalScore, theirFinalScore, yourLegalCount, yourLegalWords, theirLegalCount, theirLegalWords, 
                                                    commonCount, commonWords, yourIllegalCount, yourIllegalWords, theirIllegalCount, theirIllegalWords);
                if (NavigationService != null)
                {
                    NavigationService.Navigate(sum);
                }
            });
        }

        // When the server sends a TERMINATED message:
        // Show a messagebox giving the user the option of starting a new match. Depending on their choice,
        // either navigate to the startup page or close the window.
        private void terminatedReceived(string obj)
        {
            MessageBoxResult result = MessageBox.Show("Your opponent is dead. Would you like to destroy another opponent in a duel to the death?",
                                                      "Opponent Died", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                Dispatcher.Invoke(() =>
                {
                    StartupPage start = new StartupPage(model);
                    // Hacky solution, but what must be done is a necessary evil.
                    if (NavigationService != null)
                    {
                        NavigationService.Navigate(start);
                    }
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    Window parent = Window.GetWindow(this);
                    parent.Close();
                });
            }       
        }


        // When the player clicks the Disconnect button:
        // Show a messagebox scolding the player for their rudeness, and giving them the chance to start a new match.
        // Depending on their choice, either navigate to the startup page or close the window.
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            this.model.closeSocket();

            MessageBoxResult result = MessageBox.Show("You are an inconsiderate jerk who left your opponent to commit seppuku. SHAMEFUL DISPLAY. Do you wish to troll another helpless soul?",
                                                      "You suck.", MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                Dispatcher.Invoke(() =>
                {
                    StartupPage start = new StartupPage(model);
                    if (NavigationService != null)
                    {
                        NavigationService.Navigate(start);
                    }
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    Window parent = Window.GetWindow(this);
                    parent.Close();
                });
            }

            
        }

        // trimKeyword: helper function that eliminates the keyword from the server's message.
        private string trimKeyword(string input)
        {
            // Otherwise, remove the keyword to isolate the content.
            int index = input.IndexOf(' ');
            return input.Substring(index + 1);
        }

    }
}
