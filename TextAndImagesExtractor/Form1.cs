using DataExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Utilities;

namespace TextAndImagesExtractor
{
    public partial class Form1 : Form
    {
        private Extractor _extractor;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string[] filesArray = openFileDialog1.FileNames;
                List<string> filesList = new List<string>(filesArray);
                this.label1.Text = String.Format("Seleccionados: {0}", filesList.Count);
                this._extractor = new Extractor(filesList);

                this.ShowFilesInList(filesList);
            }
            else
            {
                MessageBox.Show("Debe seleccionar algun archivo PDF.");
            }
        }

        private void ShowFilesInList(List<string> list)
        {
            this.listBox1.Items.Clear();

            foreach (var document in list)
            {
                this.listBox1.Items.Add(Util.GetFileName(document));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this._extractor != null)
            {
                try
                {
                    this.button1.Enabled = false;
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                    this.label1.Text = String.Empty;
                    this.progressBar1.Value = 0;
                    this.progressBar1.Value = 3;
                    Thread backgroundThread =
                        new Thread(
                   new ThreadStart(() =>
                   {
                   Extractor.progressBar = this.progressBar1;
                   Extractor.displayLabel = this.label1;
                       this._extractor.Extract();

                       this.progressBar1.BeginInvoke(
                           new Action(() =>
                           {
                               this.progressBar1.Value += 100 - this.progressBar1.Value;
                               this.label1.Text = "Listo";

                           }));

                       MessageBox.Show("Proceso finalizado!");

                       this.button1.BeginInvoke(
                           new Action(() =>
                           {
                               this.button1.Enabled = true;
                               this.button2.Enabled = true;
                               this.button3.Enabled = true;
                           }));
                   }));

                    backgroundThread.Start();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Util.SavePath = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
