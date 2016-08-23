using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC
{
    public partial class Form1 : Form
    {
        public ChatClientModel model;

        public Form1()
        {
            InitializeComponent();
            model = new ChatClientModel();
            model.IncomingLineEvent += MessageReceived;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.Connect("localhost", 2000, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            model.SendMessage(textBox3.Text);
        }

        private void MessageReceived(String line)
        {
            textBox2.Invoke(new Action(() => { textBox2.Text += line + "\r\n"; }));
            model.receivedText = line;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            model.closeSocket();
        }
    }
}
