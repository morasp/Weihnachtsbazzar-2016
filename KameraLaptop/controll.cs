using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video;
using AForge.Video.DirectShow;

namespace KameraLaptop
{
    public delegate void pressedEventHandler(object sender, EventArgs e);
    public delegate void blackEventHandler(object sender, EventArgs e);
    public delegate void videoPressedEventHandler(object sender, EventArgs e);
    public partial class controll : Form
    {
        Form1 senderForm;
        
        public event pressedEventHandler pressed;
        public event blackEventHandler black;
        public event videoPressedEventHandler videoPressed;
        public int selectedDevice
        {
            get
            {
                return comboBox1.SelectedIndex;
            }
        }
        public string file
        {
            get
            {
                return tb_file.Text;
            }
            set
            {
                tb_file.Text = value;
            }
        }
        public controll(Form1 sender)
        {
            InitializeComponent();
            this.senderForm = sender;
        }

        private void controll_Load(object sender, EventArgs e)
        {
            getCameraList();
        }
        private void getCameraList()
        {
            try
            {
                senderForm.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                comboBox1.Items.Clear();
                if (senderForm.videoDevices.Count == 0)
                    throw new ApplicationException();

                senderForm.DeviceExist = true;
                foreach (FilterInfo device in senderForm.videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0; //make dafault to first cam
            }
            catch (ApplicationException)
            {
                senderForm.DeviceExist = false;
                comboBox1.Items.Add("No capture device on your system");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Camtrigger1");
            pressed(sender, e);
        }
        protected virtual void OnPress(object sender,EventArgs e)
        {
            if (pressed!= null)
                pressed(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            black(sender, e);
        }

        private void controll_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video Files (*.mp4; *.avi; *.wmv; *.webm)|*.mp4; *.avi; *.wmv; *.webm| All files (*.*) | *.*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
                file = ofd.FileName;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("FileTrigger1");
            videoPressed(sender, e);
        }
    }
}
