﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        Thread th;
        public Form2()
        {
            InitializeComponent();
            textBox.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(OpenNewForm1);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void OpenNewForm1(object obj)
        {
            Application.Run(new RegistrationAndLoginForm());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
