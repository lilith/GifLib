using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GifUI
{
 
    public partial class WaterMark : Form
    {
        public WaterMark()
        {
            InitializeComponent();          
        }

        WaterMarkText _text = new WaterMarkText();
        public WaterMarkText WaterMarkText
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            _text.Text = richTextBox1.Text;
            this._text.ForceColor = this.richTextBox1.ForeColor;
            this._text.Font = fontDialog1.Font;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.richTextBox1.Font = fontDialog1.Font;               
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.richTextBox1.ForeColor = colorDialog1.Color;                
            }
        }
    }
}
