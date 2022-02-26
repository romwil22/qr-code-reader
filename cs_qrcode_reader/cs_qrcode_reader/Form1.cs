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
        FilterInfoCollection filterIntoCollection;
        VideoCaptureDevice captureDevice;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filterIntoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo filterInfo in filterIntoCollection)
            {
                camera_device_combox.Items.Add(filterInfo.Name);
            }

            camera_device_combox.SelectedIndex = 0;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            captureDevice = new VideoCaptureDevice(filterIntoCollection[camera_device_combox.SelectedIndex].MonikerString);
            captureDevice.NewFrame += finalframe_newframe;
            captureDevice.Start();
            timer1.Start();
        }

        private void finalframe_newframe(object sender, NewFrameEventArgs eventArgs)
        {
            qrcode_picturebox.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (captureDevice.IsRunning)
            {
                captureDevice.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (qrcode_picturebox.Image != null)
            {
                BarcodeReader qrCodeReader = new BarcodeReader();
                Result result = qrCodeReader.Decode((Bitmap)qrcode_picturebox.Image);

                if (result != null)
                {
                    qrcode_textbox.Text = result.ToString();
                    timer1.Stop();

                    if (captureDevice.IsRunning)
                    {
                        captureDevice.Stop();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            qrcode_textbox.Clear();
        }
    }
}
