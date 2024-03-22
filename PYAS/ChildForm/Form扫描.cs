using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PYAS.ChildForm
{
    public partial class Form扫描 : Form
    {
        public Form扫描()
        {
            InitializeComponent();

        }


        public void WriteLog(string message, Color color, float fontSize)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, fontSize);
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(message + Environment.NewLine);
            richTextBox1.ScrollToCaret(); // 确保滚动到最新日志  
        }

        public void WriteLog(string message)
        {
            WriteLog(message, Color.Black, richTextBox1.Font.Size); // 使用默认颜色和大小  
        }

        private void Form扫描_Load(object sender, EventArgs e)
        {
            GraphicsPath gp = new GraphicsPath();
            float radius = 15; // 调整圆角的大小
            gp.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            gp.AddArc(pictureBox1.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            gp.AddArc(pictureBox1.Width - (radius * 2), pictureBox1.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddArc(0, pictureBox1.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.CloseFigure();
            Region rg = new Region(gp);
            pictureBox1.Region = rg;
            pictureBox2.Region = rg;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择一个目录作为路径：";
            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = Environment.SpecialFolder.ApplicationData;

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                // 确保文件夹存在  
                if (Directory.Exists(folderPath))
                {
                    WriteLog($"开始扫描{folderPath}",Color.Black,12);
                    // 遍历文件夹下的所有文件  
                    foreach (string filePath in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
                    {
                        // 输出文件的完整路径  
                        //Console.WriteLine(filePath);
                    }
                }
                else
                {
                    Console.WriteLine("指定的文件夹不存在。");
                }
            }
        }
    }
}
