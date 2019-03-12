using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public partial class FormResize : Form
    {
        public int GridWidth
        {
            get { return int.Parse(mtbWidth.Text); }
        }

        public int GridHeight
        {
            get { return int.Parse(mtbHeight.Text); }
        }

        public FormResize(int w, int h)
        {
            InitializeComponent();

            tbSize.Text = string.Format("width:{0}, height{1}", w, h);
        }
    }
}
