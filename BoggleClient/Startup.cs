using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boggle_Client
{
    public partial class Startup : Form
    {
        public BoggleClientModel model;

        public Startup()
        {
            InitializeComponent();
            model = new BoggleClientModel();
            model.IncomingLineEvent += MessageReceived;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            model.Connect(ipTextbox.Text, "PLAY " + nameTextbox.Text);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }

        private void MessageReceived(String line)
        {
            Regex pattern = new Regex(@"^START");
            if (pattern.IsMatch(line))
            {
                // Call the next window and close this one
                BoggleGame newGame = new BoggleGame(model);
                newGame.Show();
                this.Invoke(new Action(() => { this.Close(); }));
            }
        }

    }
}
