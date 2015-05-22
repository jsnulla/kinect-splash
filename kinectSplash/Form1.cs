using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kinectSplash
{
    public partial class kinectSplash : Form
    {
        int screenWidth;
        int screenHeight;
        bool isClosing = false;

        // Set Window Position
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        // Window Z Order
        const int
        HWND_TOP = 0,
        HWND_BOTTOM = 1,
        HWND_TOPMOST = -1,
        HWND_NOTTOPMOST = -2;

        // Window Position Flags
        const int
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOREDRAW = 0x0008,
        SWP_NOACTIVATE = 0x0010,
        SWP_DRAWFRAME = 0x0020,
        SWP_FRAMECHANGED = 0x0020,
        SWP_SHOWWINDOW = 0x0040,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        SWP_DEFERERASE = 0x2000,
        SWP_ASYNCWINDOWPOS = 0x4000;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr FindWindow(string strClassName, string strWindowName);

        public kinectSplash()
        {
            InitializeComponent();
            screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;

            this.Width = screenWidth;
            this.Height = screenHeight / 4;

            this.Left = 0;
            this.Top = screenHeight - this.Height;

            pictureBox2.Left = 20;
            pictureBox2.Top = 12;
            pictureBox2.Height = this.Height - 20;

            label1.Top = 12;
            label1.Left = pictureBox2.Left + 50;

            timer1.Interval = 25;
            timer1.Start();

            timer2.Interval = 100;
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.isClosing == false && this.Opacity < .70)
                this.Opacity += 0.025;

            else if (this.isClosing == true && this.Opacity > 0)
                this.Opacity -= 0.05;
            else if (this.isClosing == true && this.Opacity <= 0)
                Application.Exit();
        }
        
        private void kinectSplash_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isClosing = true;
            if (this.Opacity > 0){
                e.Cancel = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!this.isClosing)
            {
                ShowOnTop();
                UpdateImage();
            }
        }

        private void ShowOnTop() {
            IntPtr handle = IntPtr.Zero;

            try // Try to keep CMS on top
            {
                handle = FindWindow("WindowsForms10.Window.8.app.0.378734a", null);
                SetWindowPos(handle, (IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            }
            catch (Exception e)
            {
                // Do nothing
            }

            handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            SetWindowPos(handle, (IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        private void UpdateImage()
        {
            try
            {
                string fileName = " C:\\HTech\\KinectTmpFile\\UserColorMap.png";
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, System.IO.FileShare.ReadWrite);
                pictureBox2.Image = System.Drawing.Image.FromStream(fs);
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                fs.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
