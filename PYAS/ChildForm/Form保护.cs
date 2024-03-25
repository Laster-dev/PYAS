using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PYAS.ChildForm
{
    public partial class Form保护 : Form
    {
        public Form保护()
        {
            InitializeComponent();
        }

        private void Form保护_Load(object sender, EventArgs e)
        {
            GraphicsPath gp = new GraphicsPath();
            float radius = 10; // 调整圆角的大小
            gp.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            gp.AddArc(pictureBox1.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            gp.AddArc(pictureBox1.Width - (radius * 2), pictureBox1.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddArc(0, pictureBox1.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.CloseFigure();
            Region rg = new Region(gp);
            pictureBox1.Region = rg;
            pictureBox2.Region = rg;
            pictureBox3.Region = rg;
            pictureBox4.Region = rg;
            pictureBox5.Region = rg;
            /*panel1.Region = rg;
            panel2.Region = rg;
            panel3.Region = rg;
            panel4.Region = rg;
            panel5.Region = rg;*/
        }
    }
}
