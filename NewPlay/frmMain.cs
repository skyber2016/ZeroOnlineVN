using NewPlay.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewPlay
{
    public partial class frmMain : Form
    {
        private string PathAutoP = AppDomain.CurrentDomain.BaseDirectory + "AutoP.exe";
        private string FileName = AppDomain.CurrentDomain.BaseDirectory + "Zero.exe";
        private string FileVersion = AppDomain.CurrentDomain.BaseDirectory + "version.dat";
        private string Office = "http://zeroonlinevn.com";
        private string Reg = "http://id.zeroonlinevn.com";
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (File.Exists(this.PathAutoP))
            {
                Process.Start(this.PathAutoP);
                Application.Exit();
            }
            else
            {
                MessageBox.Show($"Không tìm thấy file {this.PathAutoP}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateLabel(string text, DateTime time, string link, int index)
        {
            var i = index * 20;
            var label = new Label
            {
                Location = new Point(243, 253 + i),
                BackColor = Color.Transparent,
                Text = text,
                Width = 200,
                ForeColor = Color.FromArgb(131, 137, 160),
                Font = new Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            label.MouseEnter += (o, e) =>
            {
                var l = (Label)o;
                l.ForeColor = Color.White;
            };
            label.MouseLeave += (o, e) =>
            {
                var l = (Label)o;
                l.ForeColor = Color.FromArgb(131, 137, 160);
            };
            label.Cursor = Cursors.Hand;
            var labelTime = new Label
            {
                Location = new Point(566, 253 + i),
                BackColor = Color.Transparent,
                Text = $"[{time.ToString("dd/MM")}]",
                ForeColor = Color.FromArgb(131, 137, 160),
                Font = new Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };

            labelTime.MouseEnter += (o, e) =>
            {
                var l = (Label)o;
                l.ForeColor = Color.White;
            };
            labelTime.Cursor = Cursors.Hand;
            labelTime.MouseLeave += (o, e) =>
            {
                var l = (Label)o;
                l.ForeColor = Color.FromArgb(131, 137, 160);
            };
            label.Click += (o, e) => Process.Start(link);
            labelTime.Click += (o, e) => Process.Start(link);
            this.Controls.Add(label);
            this.Controls.Add(labelTime);

        }
        private void LoadVersion()
        {
            if (File.Exists(this.FileVersion))
            {
                this.lblVersion.Text = File.ReadAllText(this.FileVersion);
            }
            else
            {
                this.lblVersion.Text = "0";
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.MouseDown += FrmMain_MouseDown;
            this.LoadVersion();

            var bitmap = NewPlay.Properties.Resources.Enter001;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStart.BackgroundImage = bitmap;

            var bitmapHigh = NewPlay.Properties.Resources.high01;
            bitmapHigh.MakeTransparent(Color.Fuchsia);
            this.btnStartHigh.BackgroundImage = bitmapHigh;

            var bitmapQuit = NewPlay.Properties.Resources.Exit01;
            bitmapQuit.MakeTransparent(Color.Fuchsia);
            this.btnQuit.BackgroundImage = bitmapQuit;

            var bitmapOffice = NewPlay.Properties.Resources.Office01;
            bitmapOffice.MakeTransparent(Color.Fuchsia);
            this.btnOffice.BackgroundImage = bitmapOffice;

            var bitmapReg = NewPlay.Properties.Resources.Team01;
            bitmapReg.MakeTransparent(Color.Fuchsia);
            this.btnReg.BackgroundImage = bitmapReg;

            this.LoadPost();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Enter002;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStart.BackgroundImage = bitmap;
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Enter001;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStart.BackgroundImage = bitmap;
        }

        private void btnStart_MouseDown(object sender, MouseEventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Enter003;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStart.BackgroundImage = bitmap;
        }

        private void btnStart_MouseUp(object sender, MouseEventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Enter002;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStart.BackgroundImage = bitmap;
        }

        private void btnStartHigh_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.FileName))
            {
                MessageBox.Show($"Không tìm thấy file {this.FileName}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var process = new Process();
            process.StartInfo.FileName = this.FileName;
            process.StartInfo.Arguments = "blacknull";
            process.Start();
            this.WindowState = FormWindowState.Minimized;
            Thread.Sleep(2000);
            Application.Exit();
        }

        private void btnStartHigh_MouseDown(object sender, MouseEventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.high03;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStartHigh.BackgroundImage = bitmap;
        }

        private void btnStartHigh_MouseEnter(object sender, EventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.high02;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStartHigh.BackgroundImage = bitmap;
        }

        private void btnStartHigh_MouseLeave(object sender, EventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.high01;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStartHigh.BackgroundImage = bitmap;
        }

        private void btnStartHigh_MouseUp(object sender, MouseEventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.high02;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnStartHigh.BackgroundImage = bitmap;
        }

        private void btnQuit_MouseDown(object sender, MouseEventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Exit03;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnQuit.BackgroundImage = bitmap;
        }

        private void btnQuit_MouseEnter(object sender, EventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Exit02;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnQuit.BackgroundImage = bitmap;
        }

        private void btnQuit_MouseLeave(object sender, EventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Exit01;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnQuit.BackgroundImage = bitmap;
        }

        private void btnQuit_MouseUp(object sender, MouseEventArgs e)
        {
            var bitmap = NewPlay.Properties.Resources.Exit02;
            bitmap.MakeTransparent(Color.Fuchsia);
            this.btnQuit.BackgroundImage = bitmap;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOffice_MouseDown(object sender, MouseEventArgs e)
        {
            var bitmapOffice = NewPlay.Properties.Resources.Office03;
            bitmapOffice.MakeTransparent(Color.Fuchsia);
            this.btnOffice.BackgroundImage = bitmapOffice;
        }

        private void btnOffice_MouseEnter(object sender, EventArgs e)
        {
            var bitmapOffice = NewPlay.Properties.Resources.Office02;
            bitmapOffice.MakeTransparent(Color.Fuchsia);
            this.btnOffice.BackgroundImage = bitmapOffice;
        }

        private void btnOffice_MouseLeave(object sender, EventArgs e)
        {
            var bitmapOffice = NewPlay.Properties.Resources.Office01;
            bitmapOffice.MakeTransparent(Color.Fuchsia);
            this.btnOffice.BackgroundImage = bitmapOffice;
        }

        private void btnOffice_MouseUp(object sender, MouseEventArgs e)
        {
            var bitmapOffice = NewPlay.Properties.Resources.Office02;
            bitmapOffice.MakeTransparent(Color.Fuchsia);
            this.btnOffice.BackgroundImage = bitmapOffice;
        }

        private void btnOffice_Click(object sender, EventArgs e)
        {
            Process.Start(this.Office);
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            Process.Start(this.Reg);
        }

        private void btnReg_MouseDown(object sender, MouseEventArgs e)
        {
            var bitmapReg = NewPlay.Properties.Resources.Team03;
            bitmapReg.MakeTransparent(Color.Fuchsia);
            this.btnReg.BackgroundImage = bitmapReg;
        }

        private void btnReg_MouseEnter(object sender, EventArgs e)
        {
            var bitmapReg = NewPlay.Properties.Resources.Team02;
            bitmapReg.MakeTransparent(Color.Fuchsia);
            this.btnReg.BackgroundImage = bitmapReg;
        }

        private void btnReg_MouseLeave(object sender, EventArgs e)
        {
            var bitmapReg = NewPlay.Properties.Resources.Team01;
            bitmapReg.MakeTransparent(Color.Fuchsia);
            this.btnReg.BackgroundImage = bitmapReg;
        }

        private void btnReg_MouseUp(object sender, MouseEventArgs e)
        {
            var bitmapReg = NewPlay.Properties.Resources.Team02;
            bitmapReg.MakeTransparent(Color.Fuchsia);
            this.btnReg.BackgroundImage = bitmapReg;
        }

        private void LoadPost()
        {
            new Thread(()=>
            {
                var service = new PostService();
                var resp = service.GetPosts();
                var datas = resp;
                var link = "https://zeroonlinevn.com/details/all/";
                this.Invoke((MethodInvoker)delegate ()
                {
                    for (int i = 0; i < datas.Count; i++)
                    {
                        var data = datas[i];
                        this.CreateLabel(data.title.rendered, data.modified, $"{link}{data.slug}", i);
                    }
                });
                
            }).Start();
            
        }

    }
}
