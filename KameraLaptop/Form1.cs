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
using Microsoft.DirectX.AudioVideoPlayback;
using WMPLib;

namespace KameraLaptop
{
    public partial class Form1 : Form
    {
        internal bool DeviceExist = false;
        internal FilterInfoCollection videoDevices;
        internal VideoCaptureDevice videoSource = null;
        internal Video video = null;
        public delegate void stopEventHander(object sender, EventArgs e);
        public event stopEventHander stop;
        controll controller;
        WMPLib.WindowsMediaPlayer Player;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.Visible = false;
            controller = new controll(this);
            controller.pressed += Controller_pressed;
            controller.black += Controller_black;
            controller.videoPressed += Controller_videoPressed;
            controller.Show();

            
        }

        private void Controller_videoPressed(object sender, EventArgs e)
        {
            Console.WriteLine("Video trigger");
            Button button1 = (Button)sender;
          
            if(button1.Text == "&Start")
            {
                if(controller.file != String.Empty)
                {  
                    CloseVideoSource();
                    /*video = new Video(controller.file);
                    video.Owner = pictureBox1;
                    video.Play();
                    */
                    axWindowsMediaPlayer1.Visible = true;
                    PlayFile(controller.file);
                    button1.Text = "&Stop";
                }
            }else
            {
                axWindowsMediaPlayer1.Visible = false;
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                button1.Text = "&Start";
            }
           
        }

        

        private void Controller_black(object sender, EventArgs e)
        {
            CloseVideoSource();
            pictureBox1.Image = new Bitmap(1, 1);
            controller.button1.Text = "&Start";


        }

        private void Controller_pressed(object sender, EventArgs e)
        {
            
            Button button1 = (Button)sender;
            if (button1.Text == "&Start")
            {
                if (DeviceExist)
                {
                    videoSource = new VideoCaptureDevice(videoDevices[controller.selectedDevice].MonikerString);
                    videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                    CloseVideoSource();
                    videoSource.DesiredFrameSize = new Size(1080, 720);
                    //videoSource.DesiredFrameRate = 10;
                    videoSource.Start();

                    button1.Text = "&Stop";

                }
                else
                {
                    MessageBox.Show("Error");
                    return;
                }
            }
            else
            {
                if (videoSource.IsRunning)
                {

                    CloseVideoSource();
                    //stop(sender, e);
                   
                    button1.Text = "&Start";
                }
            }
        }

       
        
        internal void CloseVideoSource()
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }
        internal void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            //do processing here
            pictureBox1.Image = img;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseVideoSource();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else
                WindowState = FormWindowState.Maximized;
        }
        private void PlayFile(string url)
        {
           
            

            axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
            axWindowsMediaPlayer1.MediaError += AxWindowsMediaPlayer1_MediaError;
            axWindowsMediaPlayer1.URL = url;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            Console.WriteLine(axWindowsMediaPlayer1.currentMedia.duration); 
        }

        private void AxWindowsMediaPlayer1_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
           Console.WriteLine("error");
        }

        private void AxWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
           Console.WriteLine("play state:"+ axWindowsMediaPlayer1.playState.ToString());
            
        }

        private void Player_MediaError(object pMediaObject)
        {
            
        }

        private void Player_PlayStateChange(int NewState)
        {
            
        }

        private void axWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            controller.BringToFront();
        }
    }
}
