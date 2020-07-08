using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using WebEye.Controls.WinForms.WebCameraControl;


namespace WebCamera
{
    public partial class WebCamVideoCapture : Form
    {
        //IEnumerable<WebCameraId> webCameraIds;
        public WebCamVideoCapture()
        {
            InitializeComponent();
            try
            {
                foreach(WebCameraId webCameraId in webCameraControl.GetVideoCaptureDevices())
                {
                    selectedCamComboBox.Items.Add(new CamComboBoxItem(webCameraId));
                }

                if (selectedCamComboBox.Items.Count != 0)
                {
                    selectedCamComboBox.SelectedIndex = 0;
                }
            }
            catch(Exception ex)
            {
                
            }
            
            try
            {
                sizeToolStripComboBox.Items.Add(new CamSizeComboBoxItem(1, 640, 360));
                sizeToolStripComboBox.Items.Add(new CamSizeComboBoxItem(2, 640, 480));
                sizeToolStripComboBox.SelectedIndexChanged += SizeToolStripComboBox_SelectedIndexChanged;
            }
            catch (Exception ex)
            {

            }

        }

        private void SizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CamSizeComboBoxItem item = (CamSizeComboBoxItem)sizeToolStripComboBox.SelectedItem;
            webCameraControl.Width = item.Width;
            webCameraControl.Height = item.Height;
        }

        private void startToolStripButton_Click(object sender, EventArgs e)
        {
            if(selectedCamComboBox.SelectedIndex != -1)
            {
                CamComboBoxItem item = (CamComboBoxItem)selectedCamComboBox.SelectedItem;
                webCameraControl.StartCapture(item.Id); 
            }
        }

        private void stopToolStripButton_Click(object sender, EventArgs e)
        {
            if (webCameraControl.IsCapturing)
            {
                webCameraControl.StopCapture();
            } 
        }

        private class CamComboBoxItem
        {
            public CamComboBoxItem(WebCameraId id)
            {
                _id = id;
            }

            private readonly WebCameraId _id;
            public WebCameraId Id
            {
                get { return _id; }
            }

            public override string ToString()
            {
                // Generates the text shown in the combo box.
                return _id.Name;
            }
        }

        private class CamSizeComboBoxItem
        {
            public CamSizeComboBoxItem(Int32 id, Int32 width, Int32 height)
            {
                Id = id;
                Width = width;
                Height = height;
            }
            public Int32 Id { get; set; }
            public Int32 Width { get; set; }
            public Int32 Height { get; set; }

            public override string ToString()
            {
                return $"{Id}: {Width}x{Height}";
            }
        }

        public static void SaveImageCapture(Image image)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image";// Default file name
            s.DefaultExt = ".Jpg";// Default file extension
            s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension

            // Show save file dialog box
            // Process save file dialog box results
            if (s.ShowDialog() == DialogResult.OK)
            {
                // Save Image
                string filename = s.FileName;
                FileStream fstream = new FileStream(filename, FileMode.Create);
                image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                fstream.Close();
            }
        }

        private void screenToolStripButton_Click(object sender, EventArgs e)
        {
            SaveImageCapture(webCameraControl.GetCurrentImage());
        }
    }
}
