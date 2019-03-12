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
    public partial class FormShift : Form
    {
        public int RightShift
        {
            get { return (int)nudRight.Value; }
        }

        public int DownShift
        {
            get { return (int)nudDown.Value; }
        }

        public FormShift()
        {
            InitializeComponent();
        }
    }
}
