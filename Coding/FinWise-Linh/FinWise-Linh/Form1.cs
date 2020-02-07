using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace FinWise_Linh
{
    public partial class Form1 : Form
    {

        string[] lines = null;
        string json = null;
        string outputFileName = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(openFileDialog1.FileName);
                string inputPath = openFileDialog1.FileName;
                tbOpenFile.Text= inputPath;
                readFile(inputPath);
                coverToJson();
                //Display in GridView
                GridView.DataSource = JsonConvert.DeserializeObject<DataTable>(json);
                _ = GridView.DataBindings;
                //save file
                saveFile();
                lbStatus.Text = "Done: File Saved";
                lbStatus.ForeColor = Color.Green;
            }
        }

        private void readFile(String inputPath)
        {
            try
                { lines = System.IO.File.ReadAllLines(inputPath);}
            catch (FileNotFoundException e)
                {Console.WriteLine("File not found");}
            foreach(string line in lines)
                rtbInput.Text += line + "\n";                                   
        }

        // Convert Data to JSON Data
        private void coverToJson()
        {              
            if (lines != null)
                {
                    json = "[";
                    for (int i = 1; i < lines.Length - 2; i++)
                        json += "{" + "'Name': '" + lines[i].Substring(9, 20).Trim() + "','Account': " + lines[i].Substring(32, 11).Trim() + ",'Amount': '" + lines[i].Substring(45, 10) + "'},";
               
                    json = json.Remove(json.Length - 1);
                    json += "]";
                    json = json.Replace("'", "\"");
                }
            rtbOutput.Text = json;
        }

        private void saveFile()
        {
            //GENERATE JSON FILENAME in MMDDYYYY-HHMM format
            outputFileName = @"C:\ATF-Output\ATF" + string.Format(DateTime.Now.ToString("MMddyyyy-hhmm")) + ".etfx";
            txbSaveFileName.Text = outputFileName;
            //WRITE JSON DATA to File
            System.IO.File.WriteAllText(outputFileName, json);
        }
    }
}
