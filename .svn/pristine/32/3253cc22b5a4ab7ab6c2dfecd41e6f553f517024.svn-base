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
    /// Interaction logic for MainWindow.xaml
    /// The main window contains a Frame control, which navigates between the pages in the
    /// program: the Startup page, the Game page, and the Summary page.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructs the main window and navigates to the Startup page
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            BoggleClientModel model = new BoggleClientModel();
            StartupPage start = new StartupPage(model);
            NavFrame.Navigate(start);
            // Hide the back/forward buttons on the Frame.
            this.NavFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
    }
}
