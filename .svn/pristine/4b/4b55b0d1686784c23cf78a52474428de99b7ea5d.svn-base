using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for SummaryPage.xaml
    /// The Summary page displays the results of the game, namely:
    /// the player's legal words; the opponent's legal words; common words played by both;
    /// the player's illegal words; and the opponent's illegal words.
    /// </summary>
    public partial class SummaryPage : Page
    {
        BoggleClientModel model;

        /// <summary>
        /// Constructor for the Summary page.
        /// </summary>
        /// <param name="model"></param>
        public SummaryPage( BoggleClientModel model, string yourScore, string theirScore, string yourLegalCount, string yourLegalWords, 
                            string theirLegalCount, string theirLegalWords, string commonCount, string commonWords, string yourIllegalCount, 
                            string yourIllegalWords, string theirIllegalCount, string theirIllegalWords)
        {
            InitializeComponent();
            this.model = model;

            //Update GUI
            Dispatcher.Invoke(() =>
            {
                //Scores
                YourScoreTextBlock.Text = yourScore;
                TheirScoreTextBlock.Text = theirScore;

                //Your legals
                YourLegalCountTextBlock.Text = yourLegalCount;
                YourLegalWordsTextBlock.Text = yourLegalWords;

                //Their legals
                TheirLegalCountTextBlock.Text = theirLegalCount;
                TheirLegalWordsTextBlock.Text = theirLegalWords;

                //Common words
                CommonCountTextBlock.Text = commonCount;
                CommonWordsTextBlock.Text = commonWords;

                //Your illegals
                YourIllegalCountTextBlock.Text = yourIllegalCount;
                YourIllegalWordsTextBlock.Text = yourIllegalWords;

                //Their illegals
                TheirIllegalCountTextBlock.Text = theirIllegalCount;
                TheirIllegalWordsTextBlock.Text = theirIllegalWords;
            });

        }

        /// <summary>
        /// User wants to play again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgainButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => 
            {
                StartupPage start = new StartupPage(model);
                NavigationService.Navigate(start);
            });
            
        }
    }
}
