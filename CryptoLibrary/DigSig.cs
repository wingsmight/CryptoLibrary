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
    /*Цифровая подпись*/
    public partial class DigSig : Form
    {

        byte[] inputBuffer;
        byte[] outputBuffer;
        byte[] signMsg;

        string rsaparam;
        string dsaparam;



        public enum AlgName { RSA, DSA };
        public void GenerateKeys(AlgName algorithmName, string filename)
        {

            switch (algorithmName)
            {
                case AlgName.RSA:
                    RSACng rsa = new RSACng();
                    rsaparam = rsa.ToXmlString(true);
                    using (StreamWriter writer = new StreamWriter(File.Open(filename, FileMode.Create)))
                    {
                        writer.Write(rsaparam);
                    }

                    break;
                case AlgName.DSA:
                    DSACng dsa = new DSACng();
                    dsaparam = dsa.ToXmlString(true);
                    using (StreamWriter writer = new StreamWriter(File.Open(filename, FileMode.Create)))
                    {
                        writer.Write(dsaparam);
                    }
                    break;
                    ;
            }
        }

        public byte[] CreateSignature(byte[] msg, AlgName algorithmName)
        {


            switch (algorithmName)
            {
                case AlgName.RSA:
                    RSACng rsa = new RSACng();
                    rsa.FromXmlString(rsaparam);
                    return rsa.SignData(msg, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                case AlgName.DSA:
                    DSACng dsa = new DSACng();
                    dsa.FromXmlString(dsaparam);
                    try { outputBuffer = dsa.CreateSignature(msg); }
                    catch
                    {
                        MessageBoxButtons butt = MessageBoxButtons.OK;
                        DialogResult result;
                        result = MessageBox.Show("Cлишком короткое сообщение для подписи!");
                        break;
                    }
                    return outputBuffer;
            }
            return null;
        }

        public bool CheckSignature(byte[] msg, byte[] signature, AlgName algorithmName)
        {
            using (StreamReader reader = new StreamReader(File.Open("RSAkeys.txt", FileMode.Open)))
            {
                rsaparam = reader.ReadToEnd();
            }


            using (StreamReader reader = new StreamReader(File.Open("DSAkeys.txt", FileMode.Open)))
            {
                dsaparam = reader.ReadToEnd();
            }

            switch (algorithmName)
            {
                case AlgName.RSA:
                    RSACng rsa = new RSACng();
                    rsa.FromXmlString(rsaparam);
                    return rsa.VerifyData(msg, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                case AlgName.DSA:
                    DSACng dsa = new DSACng();
                    dsa.FromXmlString(dsaparam);
                    return dsa.VerifySignature(msg, signature);
            }
            return false;
        }


        public DigSig()
        {
            InitializeComponent();

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;

        }

        private void вернутьсяНаОкноВыбораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            GenerateKeys(AlgName.RSA, "RSAkeys.txt");
            GenerateKeys(AlgName.DSA, "DSAkeys.txt");

            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            label3.Text = "Выбранный файл - " + openFileDialog1.FileName;
            inputBuffer = File.ReadAllBytes(openFileDialog1.FileName);

            textBox1.Text = "Подписать сообщение: " + Encoding.UTF8.GetString(inputBuffer) + Environment.NewLine;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button6.Enabled = false;

            signMsg = CreateSignature(inputBuffer, AlgName.RSA);

            using (BinaryWriter writer = new BinaryWriter(File.Open("signatureRSA.txt", FileMode.Create)))
            {
                writer.Write(signMsg);
            }

            if (checkBox1.Checked)
                textBox1.Text += "C ключом " + rsaparam + Environment.NewLine;

            textBox1.Text += Environment.NewLine + "Подпись: " + Encoding.UTF8.GetString(signMsg) + Environment.NewLine;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            button6.Enabled = true;

            bool check = CheckSignature(inputBuffer, signMsg, AlgName.RSA);

            textBox1.Text += Environment.NewLine;
            textBox1.Text += check ? "Подпись прошла проверку" : "Подпись не прошла проверку";
            textBox1.Text += Environment.NewLine;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;

            signMsg = CreateSignature(inputBuffer, AlgName.DSA);
            if (signMsg == null)
                textBox1.Text += "Ошибка!!!" + Environment.NewLine;
            else
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("signatureDSA.txt", FileMode.Create)))
                {
                    writer.Write(signMsg);
                }

                if (checkBox1.Checked)
                    textBox1.Text += "C ключом " + dsaparam + Environment.NewLine;

                textBox1.Text += Environment.NewLine + "Подпись: " + Encoding.UTF8.GetString(signMsg) + Environment.NewLine;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;

            bool check = CheckSignature(inputBuffer, signMsg, AlgName.DSA);

            textBox1.Text += Environment.NewLine;
            textBox1.Text += check ? "Подпись прошла проверку" : "Подпись не прошла проверку";
            textBox1.Text += Environment.NewLine;
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            label4.Text = "Выбранный файл - " + openFileDialog2.FileName;
            signMsg = File.ReadAllBytes(openFileDialog2.FileName);

            textBox1.Text = "Проверить сообщение: " + Encoding.UTF8.GetString(signMsg) + Environment.NewLine;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }
    }
}
