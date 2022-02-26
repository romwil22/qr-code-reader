using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;

namespace cs_qrcode_reader
{
    public partial class Form1 : Form
    {
        public FilterInfoCollection captureDevice;
        public VideoCaptureDevice finalFrame;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            captureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in captureDevice)
            {
                camera_device_combox.Items.Add(device.Name);
            }

            camera_device_combox.SelectedIndex = 0;
            finalFrame = new VideoCaptureDevice();
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            finalFrame = new VideoCaptureDevice(captureDevice[camera_device_combox.SelectedIndex].MonikerString);
            finalFrame.NewFrame += new NewFrameEventHandler(finalframe_newframe);
            finalFrame.Start();
        }

        private void finalframe_newframe(object sender, NewFrameEventArgs eventArgs)
        {
            qrcode_picturebox.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (finalFrame.IsRunning == true)
            {
                finalFrame.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader qrCodeReader = new BarcodeReader();
            Result qrResult = qrCodeReader.Decode((Bitmap)qrcode_picturebox.Image);

            try
            {
                string qrDecoded = qrResult.ToString().Trim();
                if (qrDecoded != "")
                {
                    qrcode_textbox.Text = qrDecoded;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
