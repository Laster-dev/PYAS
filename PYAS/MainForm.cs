using PYAS.ChildForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PYAS
{
    public partial class MainForm : Form
    {
        Form activeform;
        Form状态 form状态 = new Form状态();
        Form扫描 form扫描 = new Form扫描();
        Form工具 form工具 = new Form工具();
        Form保护 form保护 = new Form保护();
        public MainForm()
        {
            InitializeComponent();
            contextMenuStrip1.Renderer = new CustomContextMenuStripRenderer();
            activeform = form状态;
            activeform.TopLevel = false;
            activeform.Dock = DockStyle.Fill;
            activeform.Parent = this.panel6;
            activeform.Show();

        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr HWND,int wmsg,int wparam,int lparam);
        private async void StartFadeInAnimation()
        {
            while (Opacity < 1)
            {
                Opacity += 0.05;
                await Task.Delay(30); // 添加延迟以控制动画速度
            }
        }
        private async void StartFadeInAnimation2()
        {
            while (Opacity > 0.7)
            {
                Opacity -= 0.05;
                await Task.Delay(10); // 添加延迟以控制动画速度
            }
        }

        protected override CreateParams CreateParams
        {
            get 
            { 
                CreateParams cp = base.CreateParams;
                cp.Style = 0x20000;
                return cp;
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //ReleaseCapture();
            //SendMessage(this.Handle, 0x112, 0xf012, 0);
            StartFadeInAnimation2();
            Cursor = Cursors.SizeAll;
            lastClick = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            StartFadeInAnimation();
            Cursor = Cursors.Default;
        }
        // 变量用于保存鼠标按下时的坐标
        private Point lastClick;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // 检查左键是否按下
            if (e.Button == MouseButtons.Left)
            {
                // 移动窗口
                Left += e.X - lastClick.X;
                Top += e.Y - lastClick.Y;
            }
        }
 
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor=Color.White;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = panel1.BackColor;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.White;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = panel1.BackColor;
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.White;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackColor = panel1.BackColor;
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(237, 237, 237);
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(230,230,230);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //this.Close();
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //GraphicsPath gp = new GraphicsPath();
            //gp.AddEllipse(0, 0, pictureBox1.Width - 3, pictureBox1.Height - 3);
            //Region rg = new Region(gp);
            //pictureBox1.Region = rg;
            //pictureBox2.Region = rg;
            //pictureBox3.Region = rg;



            GraphicsPath gp = new GraphicsPath();
            float radius = 15; // 调整圆角的大小
            gp.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            gp.AddArc(panel2.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            gp.AddArc(panel2.Width - (radius * 2), panel2.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddArc(0, panel2.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.CloseFigure();
            Region rg = new Region(gp);
            panel2.Region = rg;
            panel3.Region = rg;
            panel4.Region = rg;
            panel5.Region = rg;

/*
            GraphicsPath gp1 = new GraphicsPath();
            float radius1 = 15; // 调整圆角的大小
            gp1.AddArc(0, 0, radius1 * 2, radius1 * 2, 180, 90);
            gp1.AddArc(this.Width - (radius1 * 2), 0, radius1 * 2, radius1 * 2, 270, 90);
            gp1.AddArc(this.Width - (radius1 * 2), this.Height - (radius1 * 2), radius1 * 2, radius1 * 2, 0, 90);
            gp1.AddArc(0, this.Height - (radius1 * 2), radius1 * 2, radius1 * 2, 90, 90);
            gp1.CloseFigure();
            Region rg1 = new Region(gp1);
            this.Region = rg1;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            Graphics graphics = this.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
*/
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            

            float radius = 15; // 圆角的大小
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1); // 减1是为了确保边界在窗体内
            int diameter = (int)radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90); // 左上角
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90); // 右上角
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90); // 右下角
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90); // 左下角
            path.CloseFigure();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(Color.Gray, 2)) // 定义灰色线条，线宽为2
            {
                e.Graphics.DrawPath(pen, path);
            }

        }

  
        private void panel2_Click(object sender, EventArgs e)
        {
            if (activeform != form状态)
            {
                activeform.Hide();
                activeform = form状态;
                activeform.TopLevel = false;
                activeform.Dock = DockStyle.Fill;
                activeform.Parent = this.panel6;
                activeform.Show();
            }
            else
            {
                return;
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            if (activeform != form扫描)
            {
                activeform.Hide();
                activeform = form扫描;
                activeform.TopLevel = false;
                activeform.Dock = DockStyle.Fill;
                activeform.Parent = this.panel6;
                activeform.Show();
            }
            else
            {
                return;
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            if (activeform != form工具)
            {
                activeform.Hide();
                activeform = form工具;
                activeform.TopLevel = false;
                activeform.Dock = DockStyle.Fill;
                activeform.Parent = this.panel6;
                activeform.Show();
            }
            else
            {
                return;
            }
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            if (activeform != form保护)
            {
                activeform.Hide();
                activeform = form保护;
                activeform.TopLevel = false;
                activeform.Dock = DockStyle.Fill;
                activeform.Parent = this.panel6;
                activeform.Show();
            }
            else
            {
                return;
            }
        }
    }
    //contextMenuStrip1.Renderer = new CustomContextMenuStripRenderer();
    public class CustomContextMenuStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                if (e.Item.Selected) // 如果当前项被选中（鼠标悬停）
                {
                    // 设置鼠标悬停时的背景色
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(210, 210, 210)), e.Item.ContentRectangle);
                }
                else
                {

                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), e.Item.ContentRectangle);
                    // 设置默认的背景色
                   // base.OnRenderMenuItemBackground(e);
                }
            }
        }
    }
}
