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

namespace SendImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MemoryStream ms;
        TcpClient client;
        NetworkStream ns;
        BinaryWriter bw;

        string GetIpAddress()
        {
            IPHostEntry host;
            string localhost = "?";
            host = Dns.GetHostEntry(Dns.GetHostName()); // return hostname

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localhost = ip.ToString();
                }
            }
            return localhost;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtServer.Text = GetIpAddress();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            pictureBox1.Image = Image.FromFile(path);
            txtPath.Text = path;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] buffer = ms.GetBuffer(); // buffer for receiving data
                ms.Close();
                client = new TcpClient(txtServer.Text, 19999);
                ns = client.GetStream(); // receive stream for reading and writing 
                bw = new BinaryWriter(ns); // write to buffer as binary
                bw.Write(buffer); // send information to server
                bw.Close();
                ns.Close();
                client.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
