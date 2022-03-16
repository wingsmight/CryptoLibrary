using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace CryptoLibrary
{
    /*Асимметричные алгоритмы*/
    public partial class Asymmetr : Form
    {

        byte[] inputBuffer;
        byte[] outputBuffer;
        byte[] encodeMsg;

        byte[] param;

        RSACng rsa = new RSACng();



        public Asymmetr()
        {
            InitializeComponent();
            button3.Enabled = false;
            button4.Enabled = false;

        }

        public byte[] GenerateKeyRSA(RSACryptoServiceProvider algorithm, string filename)
        {
            var param = algorithm.ExportCspBlob(true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                writer.Write(param);
            }
            return param;
        }

        public byte[] EncodeRSA(byte[] msg, RSACryptoServiceProvider algorithm, RSAEncryptionPadding paddingMode)
        {
            algorithm.ImportCspBlob(param);
            return algorithm.Encrypt(msg, paddingMode);
        }

        public byte[] DecodeRSA(byte[] msg, RSACryptoServiceProvider algorithm, RSAEncryptionPadding paddingMode)
        {
            algorithm.ImportCspBlob(param);
            return algorithm.Decrypt(msg, paddingMode);
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

            textBox1.Text = "Зашифровать сообщение: " + Encoding.Default.GetString(inputBuffer) + Environment.NewLine;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            encodeMsg = EncodeRSA(inputBuffer, new RSACryptoServiceProvider(), RSAEncryptionPadding.Pkcs1);

            using (BinaryWriter writer = new BinaryWriter(File.Open("EncodeRSA.txt", FileMode.Create)))
            {
                writer.Write(encodeMsg);
            }

            textBox1.Text += "Шифротекст: " + Encoding.Default.GetString(encodeMsg) + Environment.NewLine;

            button4.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            param = GenerateKeyRSA(new RSACryptoServiceProvider(), "RSAParams.txt");

            textBox1.Text += "Ключ сгенерирован! " + Environment.NewLine;

            button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            outputBuffer = DecodeRSA(encodeMsg, new RSACryptoServiceProvider(), RSAEncryptionPadding.Pkcs1);

            using (BinaryWriter writer = new BinaryWriter(File.Open("DecodeRSA.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }


            textBox1.Text += "Дешифровано: " + Encoding.Default.GetString(outputBuffer) + Environment.NewLine;

        }
    }
}
