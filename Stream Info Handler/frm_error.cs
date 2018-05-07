//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only
//Copyright 2018, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_error : Form
    {
        public frm_error(string message)
        {
            InitializeComponent();
            lbl_message.Text = message;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
