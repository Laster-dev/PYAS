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

namespace test
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控件的背景色
        /// </summary>
        private Color _backColorUC = Color.FromArgb(250, 190, 190); //声明一个颜色变量 透明色 用于初始化 控件背景色
        [Description("控件的背景色")]  //新建控件说明 用来描述控件的作用
        public Color BackColorUC  //创建一个属性名 用于在属性窗口中显示出来
        {
            get { return _backColorUC; }  //返回 颜色变量 初始化背景色 为透明
            set                            //set 是当用户在属性窗口设置BackColorUC属性 选择颜色的时候执行
            {
                _backColorUC = value;  //获取用户在属性窗口中 选择的颜色 赋值给这个颜色变量 
                this.BackColor = _backColorUC;  //将用户选择的颜色赋值给 控件的背景颜色
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
       
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;   //实例化Graphics 对象g
           Color FColor = Color.White; //颜色1
           Color TColor = BackColorUC;  //颜色2
           Brush b = new LinearGradientBrush(this.ClientRectangle, FColor, TColor, LinearGradientMode.Horizontal);  //实例化刷子，第一个参数指示上色区域，第二个和第三个参数分别渐变颜色的开始和结束，第四个参数表示颜色的方向。
           g.FillRectangle(b, this.ClientRectangle);  //进行上色
        }
    }
}
