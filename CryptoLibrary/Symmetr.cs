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
    /*Симметричные алгоритмы*/
    public partial class Symmetr : Form
    {
        byte[] inputBuffer;
        byte[] outputBuffer;
        byte[] IV;
        byte[] key;
        byte[] encodeMsg;
        AesCng aes = new AesCng();
        TripleDESCng tripdes = new TripleDESCng();

        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public byte[] Encode(byte[] msg, SymmetricAlgorithm algorithm, byte[] IV, byte[] key)
        {
            ICryptoTransform encoder = algorithm.CreateEncryptor(key, IV);
            return encoder.TransformFinalBlock(msg, 0, msg.Length);
        }
        public byte[] Decode(byte[] msg, SymmetricAlgorithm algorithm, byte[] IV, byte[] key)
        {
            ICryptoTransform decoder = algorithm.CreateDecryptor(key, IV);
            return decoder.TransformFinalBlock(msg, 0, msg.Length);
        }

        public Symmetr()
        {
            InitializeComponent();
        }

        private void вернутьсяНаОкноВыбораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            label3.Text = "Выбранный файл - " + openFileDialog1.FileName;
            
            inputBuffer = File.ReadAllBytes(openFileDialog1.FileName);

            textBox1.Text = "Зашифровать сообщение: " + Encoding.Default.GetString(inputBuffer) + Environment.NewLine;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            label4.Text = "Выбранный файл - " + openFileDialog2.FileName;
            using (StreamReader reader = File.OpenText(openFileDialog2.FileName))
            {
                key = StringToByteArray(reader.ReadLine());
                IV = StringToByteArray(reader.ReadLine());
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button6.Enabled = false;

            encodeMsg = Encode(inputBuffer, aes, IV, key);

            using (BinaryWriter writer = new BinaryWriter(File.Open("EncodeAES.txt", FileMode.Create)))
            {
                writer.Write(encodeMsg);
            }

            textBox1.Text += "Шифротекст (AES): " + Encoding.Default.GetString(encodeMsg) + Environment.NewLine;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            outputBuffer = Decode(encodeMsg, aes, IV, key);

            using (BinaryWriter writer = new BinaryWriter(File.Open("DecodeAES.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }


            textBox1.Text += "Дешифровано (AES): " + Encoding.Default.GetString(outputBuffer) + Environment.NewLine;

            button5.Enabled = true;
            button6.Enabled = true;
        }



        private void button7_Click(object sender, EventArgs e)
        {
            aes.GenerateKey();
            key = aes.Key;

            aes.GenerateIV();
            IV = aes.IV;

            textBox1.Text += "Key: " + Encoding.Default.GetString(key) + Environment.NewLine;
            textBox1.Text += "IV: " + Encoding.Default.GetString(IV) + Environment.NewLine;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            tripdes.GenerateKey();
            key = tripdes.Key;

            tripdes.GenerateIV();
            IV = tripdes.IV;

            textBox1.Text += "Key: " + Encoding.Default.GetString(key) + Environment.NewLine;
            textBox1.Text += "IV: " + Encoding.Default.GetString(IV) + Environment.NewLine;

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;

            encodeMsg = Encode(inputBuffer, tripdes, IV, key);

            using (BinaryWriter writer = new BinaryWriter(File.Open("Encode3DES.txt", FileMode.Create)))
            {
                writer.Write(encodeMsg);
            }

            textBox1.Text += "Шифротекст (3DES): " + Encoding.Default.GetString(encodeMsg) + Environment.NewLine;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            outputBuffer = Decode(encodeMsg, tripdes, IV, key);

            using (BinaryWriter writer = new BinaryWriter(File.Open("Decode3DES.txt", FileMode.Create)))
            {
                writer.Write(outputBuffer);
            }


            textBox1.Text += "Дешифровано (3DES): " + Encoding.Default.GetString(outputBuffer) + Environment.NewLine;

            button3.Enabled = true;
            button4.Enabled = true;
        }
    }
}
