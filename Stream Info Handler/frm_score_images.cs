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
    public partial class frm_score_images : Form
    {
        public frm_score_images()
        {
            InitializeComponent();

            if (global_values.score1_image1 != @"file")
            {
                pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);
            }
            if (global_values.score1_image2 != @"file")
            {
                pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);
            }
            if (global_values.score1_image3 != @"file")
            {
                pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);
            }

            if (global_values.score2_image1 != @"file")
            {
                pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);
            }
            if (global_values.score2_image2 != @"file")
            {
                pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);
            }
            if (global_values.score2_image3 != @"file")
            {
                pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);
            }
        }

        private void btn_score1_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image1 = openFileDialog1.FileName;
                pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\score1_image1.txt", global_values.score1_image1);
            }
        }

        private void btn_score1_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image2 = openFileDialog1.FileName;
                pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\score1_image2.txt", global_values.score1_image2);
            }
        }

        private void btn_score1_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image3 = openFileDialog1.FileName;
                pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\score1_image3.txt", global_values.score1_image3);
            }
        }

        private void btn_score2_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image1 = openFileDialog1.FileName;
                pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\score2_image1.txt", global_values.score2_image1);
            }
        }

        private void btn_score2_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image2= openFileDialog1.FileName;
                pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\score2_image2.txt", global_values.score2_image2);
            }
        }

        private void btn_score2_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image3 = openFileDialog1.FileName;
                pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\score2_image3.txt", global_values.score2_image3);
            }
        }
    }
}
