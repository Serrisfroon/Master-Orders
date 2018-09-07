//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
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
using System.Xml.Linq;

namespace Stream_Info_Handler
{
    public partial class frm_score_images : Form
    {
        public frm_score_images()
        {
            InitializeComponent();

            if (global_values.score1_image1 != @"")
            {
                pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);
            }
            if (global_values.score1_image2 != @"")
            {
                pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);
            }
            if (global_values.score1_image3 != @"")
            {
                pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);
            }

            if (global_values.score2_image1 != @"")
            {
                pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);
            }
            if (global_values.score2_image2 != @"")
            {
                pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);
            }
            if (global_values.score2_image3 != @"")
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
                XDocument xml = XDocument.Load(@"C:\Users\Public\Stream Info Handler\settings.xml");
                xml.Root.Element("image-scoring").Element("player1-1").ReplaceWith(new XElement("player1-1", global_values.score1_image1));
                xml.Save(@"C:\Users\Public\Stream Info Handler\settings.xml");
            }
        }

        private void btn_score1_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image2 = openFileDialog1.FileName;
                pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);
                XDocument xml = XDocument.Load(@"C:\Users\Public\Stream Info Handler\settings.xml");
                xml.Root.Element("image-scoring").Element("player1-2").ReplaceWith(new XElement("player1-2", global_values.score1_image2));
                xml.Save(@"C:\Users\Public\Stream Info Handler\settings.xml");
            }
        }

        private void btn_score1_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image3 = openFileDialog1.FileName;
                pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);
                XDocument xml = XDocument.Load(@"C:\Users\Public\Stream Info Handler\settings.xml");
                xml.Root.Element("image-scoring").Element("player1-3").ReplaceWith(new XElement("player1-3", global_values.score1_image3));
                xml.Save(@"C:\Users\Public\Stream Info Handler\settings.xml");
            }
        }

        private void btn_score2_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image1 = openFileDialog1.FileName;
                pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);
                XDocument xml = XDocument.Load(@"C:\Users\Public\Stream Info Handler\settings.xml");
                xml.Root.Element("image-scoring").Element("player2-1").ReplaceWith(new XElement("player2-1", global_values.score2_image1));
                xml.Save(@"C:\Users\Public\Stream Info Handler\settings.xml");
            }
        }

        private void btn_score2_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image2= openFileDialog1.FileName;
                pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);
                XDocument xml = XDocument.Load(@"C:\Users\Public\Stream Info Handler\settings.xml");
                xml.Root.Element("image-scoring").Element("player2-2").ReplaceWith(new XElement("player2-2", global_values.score2_image2));
                xml.Save(@"C:\Users\Public\Stream Info Handler\settings.xml");
            }
        }

        private void btn_score2_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image3 = openFileDialog1.FileName;
                pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);
                XDocument xml = XDocument.Load(@"C:\Users\Public\Stream Info Handler\settings.xml");
                xml.Root.Element("image-scoring").Element("player2-3").ReplaceWith(new XElement("player2-3", global_values.score2_image3));
                xml.Save(@"C:\Users\Public\Stream Info Handler\settings.xml");
            }
        }
    }
}
