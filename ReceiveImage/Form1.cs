using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ReceiveImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpListener tl;
        Socket socket;
        NetworkStream ns;
        //StreamReader sr;
        Thread th;

        void ReceiveImage()
        {
            try
            {
                tl = new TcpListener(19999);
                tl.Start(); // start server
                socket = tl.AcceptSocket();
                ns = new NetworkStream(socket);
                pictureBox1.Image = Image.FromStream(ns);
                tl.Stop();

                //if (socket.Connected)
                //{
                //    while (true)
                //    {
                //        ReceiveImage();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            th = new Thread(new ThreadStart(ReceiveImage));
            th.Start();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tl.Stop();
            th.Abort();
        }
    }
}
