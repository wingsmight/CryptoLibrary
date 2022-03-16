using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoLibrary
{
    /*Хэш*/
    public partial class Hash : Form
    {
        byte[] inputBuffer;
        byte[] outputBuffer;


        public Hash()
        {
            InitializeComponent();
        }

        public byte[] HashMsg(byte[] msg, HashAlgorithm alg)
        {
            return alg.ComputeHash(msg);
        }

        private void вернутьсяНаОкноВыбораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            label2.Text = "Выбранный файл - " + openFileDialog1.FileName;
            inputBuffer = File.ReadAllBytes(openFileDialog1.FileName);

            textBox1.Text = "Взять хэш от сообщения: " + Encoding.Default.GetString(inputBuffer) + Environment.NewLine;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            outputBuffer = HashMsg(inputBuffer, MD5.Create());

            using (BinaryWriter writer = new BinaryWriter(File.Open("MD5Hash.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }

            string str = "";
            foreach (var obj in outputBuffer)
            {
                str += obj.ToString();
            }

            textBox1.Text += "MD5 хэш данного сообщения: " + str + Environment.NewLine;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outputBuffer = HashMsg(inputBuffer, RIPEMD160.Create());

            using (BinaryWriter writer = new BinaryWriter(File.Open("RIPEMD160Hash.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }

            string str = "";
            foreach (var obj in outputBuffer)
            {
                str += obj.ToString();
            }

            textBox1.Text += "RIPEMD-160 хэш данного сообщения: " + str + Environment.NewLine;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            outputBuffer = HashMsg(inputBuffer, SHA1.Create());

            using (BinaryWriter writer = new BinaryWriter(File.Open("SHA1Hash.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }

            string str = "";
            foreach (var obj in outputBuffer)
            {
                str += obj.ToString();
            }

            textBox1.Text += "SHA-1 хэш данного сообщения: " + str + Environment.NewLine;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            outputBuffer = HashMsg(inputBuffer, SHA256.Create());

            using (BinaryWriter writer = new BinaryWriter(File.Open("SHA256Hash.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }

            string str = "";
            foreach (var obj in outputBuffer)
            {
                str += obj.ToString();
            }

            textBox1.Text += "SHA-256 хэш данного сообщения: " + str + Environment.NewLine;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            outputBuffer = HashMsg(inputBuffer, SHA384.Create());

            using (BinaryWriter writer = new BinaryWriter(File.Open("SHA384Hash.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }

            string str = "";
            foreach (var obj in outputBuffer)
            {
                str += obj.ToString();
            }

            textBox1.Text += "SHA-384 хэш данного сообщения: " + str + Environment.NewLine;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outputBuffer = HashMsg(inputBuffer, SHA512.Create());

            using (BinaryWriter writer = new BinaryWriter(File.Open("SHA512Hash.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }

            string str = "";
            foreach (var obj in outputBuffer)
            {
                str += obj.ToString();
            }

            textBox1.Text += "SHA-512 хэш данного сообщения: " + str + Environment.NewLine;
        }
    }
}
