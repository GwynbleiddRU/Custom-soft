﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class CanvasSize : Form
    {
        public CanvasSize(int width, int height)
        {
            InitializeComponent();
            numericUpDown1.Value = width;
            numericUpDown2.Value = height;
        }
        public int SizeHeight { get; set; }
        public int SizeWidth { get; set; }

        private void CanvasSize_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            SizeHeight = (int)numericUpDown2.Value;
            SizeWidth = (int)numericUpDown1.Value;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();

        }
    }
}
