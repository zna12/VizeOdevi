﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _21015222028_GP_BaharDonemiVizeOdevi
{
    public partial class frmAraba : Form
    {
        public frmAraba()
        {
            InitializeComponent();
        }

        public arac arac = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            propertyGrid1.SelectedObject = arac;
        }

        private void OkClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK; 
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            DialogResult= DialogResult.Cancel;
        }
    }
}
