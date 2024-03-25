using PYAS.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Xml.Linq;

namespace PYAS.ChildForm
{
    public partial class Form扫描 : Form
    {
       
        public Form扫描()
        {
            InitializeComponent();
          
        }


        public void WriteLog(string message,string type, Color color, int cout)
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => WriteLog(message, type, color, cout)));
            }
            else
            {


                Panel panelscan = new Panel();
                Label labelscan = new Label();
                Label labeltype = new Label();
                PictureBox pictureBoxscan = new PictureBox();

                panelscan.Size = new Size(545, 40);//panel
                pictureBoxscan.Location = new Point(2, 2);//位置
                pictureBoxscan.Size = new Size(36, 36);//大小
                pictureBoxscan.SizeMode = PictureBoxSizeMode.Zoom;
                switch (cout)
                {
                    case 0:
                        pictureBoxscan.Image = Properties.Resources._2;
                        panelscan.Paint += panel0_Paint;
                        break;
                    case 1:
                        pictureBoxscan.Image = Properties.Resources._1;
                        panelscan.Paint += panel1_Paint;
                        break;
                    case 2:
                        pictureBoxscan.Image = Properties.Resources._3;
                        panelscan.Paint += panel2_Paint;
                        break;
                }
                labelscan.AutoSize = false;
                labelscan.Font = new Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                labelscan.Location = new Point(49, 1);
                labelscan.Size = new Size(450, 20);
                labelscan.Text = message;
                labelscan.ForeColor = color;

                labeltype.AutoSize = true;
                labeltype.Font = new Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                labeltype.Location = new Point(49, 20);
                labeltype.Size = new Size(50, 20);
                labeltype.Text = type;
                labeltype.ForeColor = color;


                panelscan.Controls.Add(labelscan);//添加描述
                panelscan.Controls.Add(labeltype);//添加文件路径
                panelscan.Controls.Add(pictureBoxscan);//添加图片
                //panelscan.Size = new Size(565, 40);


                flowLayoutPanel1.Controls.Add(panelscan);//添加控件

            }
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


        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            foreach (Control mControl in this.flowLayoutPanel1.Controls)
            {
                flowLayoutPanel1.Controls.Remove(mControl);
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择一个目录作为路径：";
            dialog.ShowNewFolderButton = false;

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                // 确保文件夹存在  
                if (Directory.Exists(folderPath))
                {
                    int i = 0;
                    int j = 0;
                    int k = 0;
                    int l = 0;
                    // 遍历文件夹下的所有文件  
                    foreach (string filePath in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".exe") || s.EndsWith(".dll")))
                    {
                        
                        await Task.Run(() =>
                        {
                            // 输出文件的完整路径  
                            // WriteLog("开始扫描："+filePath);
                            string xml = _360.Api360CloudScan(_360.GetMD5HashFromFile(filePath));
                            i++;

                            XElement Xmlbody = XElement.Parse(xml);
                            string e_level = Xmlbody.Descendants("e_level").FirstOrDefault()?.Value;
                            string malware = Xmlbody.Descendants("malware").FirstOrDefault()?.Value;
                            string cout = "";
                            if (e_level != null)
                            {
                                string type = "";
                                if (malware != null) {
                                    type = malware;
                                }
                                double temp = double.Parse(e_level);
                                int res = (int)temp;

                                if (res >= 50)
                                {
                                    j++;
                                    cout = $"危险{type}";
                                    WriteLog((filePath) ,cout , Color.Red, 2);
                                }
                                else if (res <= 20)
                                {
                                    l++;
                                    cout = "安全";
                                    WriteLog((filePath),cout, Color.Green, 0);
                                }
                                else if (res == 0)
                                {
                                    cout = "扫描失败";
                                    WriteLog(filePath, cout, Color.Green, 2);
                                }
                                else
                                {
                                    k++;
                                    cout = "可疑";
                                    WriteLog((filePath), cout, Color.FromArgb(214, 157, 133), 1);
                                }
                            }
                            label2.Invoke(new Action(() => label2.Text = $"已扫描{i}项    危险：{j}   可疑：{k}   安全：{l}"));
                            


                            // WriteLog(str);


                        });
                    }

                    MessageBox.Show("扫描完毕");
                }
                else
                {
                    MessageBox.Show("指定的文件夹不存在。");
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        
            Color TColor = Color.FromArgb(214, 157, 133);  //颜色2
            Pen pen = new Pen(TColor, 1);

            // 获取绘图面板的图形
            Graphics g = e.Graphics;

            // 画出边界
            g.DrawRectangle(pen, 0, 0, ((Panel)sender).Width - 1, ((Panel)sender).Height - 1);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

            Color TColor = Color.Red;  //颜色2

            Pen pen = new Pen(TColor, 1);

            // 获取绘图面板的图形
            Graphics g = e.Graphics;

            // 画出边界
            g.DrawRectangle(pen, 0, 0, ((Panel)sender).Width - 1, ((Panel)sender).Height - 1);
        }
        private void panel0_Paint(object sender, PaintEventArgs e)
        {
          
            Color TColor = Color.Green;  //颜色2
            Pen pen = new Pen(TColor, 1);

            // 获取绘图面板的图形
            Graphics g = e.Graphics;

            // 画出边界
            g.DrawRectangle(pen, 0, 0, ((Panel)sender).Width - 1, ((Panel)sender).Height - 1);
        }
    }
}
