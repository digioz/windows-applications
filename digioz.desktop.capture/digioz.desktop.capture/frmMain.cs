using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace digioz.desktop.capture
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// Capture a Screenshot of desktop
        /// </summary>
        /// <returns></returns>
        private Image CaptureScreen(int displayNumber = 1)
        {
            // To Do - Check which display needs capturing
            // http://stackoverflow.com/questions/6175968/screenshot-from-second-screen
            // http://stackoverflow.com/questions/1121600/how-do-i-determine-which-monitor-my-net-windows-forms-program-is-running-on


            //Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            //Bitmap target = new Bitmap(screenSize.Width, screenSize.Height);
            //using (Graphics g = Graphics.FromImage(target))
            //{
            //    g.CopyFromScreen(0, 0, 0, 0, new Size(screenSize.Width, screenSize.Height));
            //}
            //return target;

            Bitmap screenshot;

            //foreach (Screen screen in Screen.AllScreens)
            //{
                Screen screen = Screen.FromControl(this);
                screenshot = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                // Create a graphics object from the bitmap
                Graphics g = Graphics.FromImage(screenshot);
                // Take the screenshot from the upper left corner to the right bottom corner
                g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
            //    // Save the screenshot
            //}

            return screenshot;

        }

        /// <summary>
        /// Save Image to file system
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool SaveImage(Image image, string path)
        {
            bool result = false;

            try
            {
                image.Save(path, ImageFormat.Png);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Method to View a file by running it
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool ViewFile(string fileName)
        {
            bool result = false;

            try
            {
                Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = fileName;
                proc.Start();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        private List<string> GetListOfDisplays()
        {
            List<string> displays = new List<string>();

            try
            {
                int i = 1;

                foreach (var screen in Screen.AllScreens)
                {
                    string displayType = "Secondary Display";

                    if (screen.Primary)
                    {
                        displayType = "Primary Display";
                    }

                    displays.Add(i.ToString() + " - " + displayType);

                    //displays.Add(screen.Primary.ToString() + " - " + screen.DeviceName + " - " + screen.GetType().ToString());
                }
            }
            catch (Exception)
            {
                // Ignore
            }

            return displays;
        } 

        /// <summary>
        /// Form Constructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + ".png";

            // minimize current form
            this.WindowState = FormWindowState.Minimized;

            SaveImage(CaptureScreen(), fileName);

            // Show the current form again
            this.WindowState = FormWindowState.Normal;

            ViewFile(fileName);

        }
    }
}
