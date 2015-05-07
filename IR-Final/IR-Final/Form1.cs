using IR3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IR_Final
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.modeCbx.Items.AddRange(Program.modes);
            this.modeCbx.SelectedIndex = 1;
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (this.modeCbx.SelectedIndex == 0)
            {
                // Mode 1: Create database to search faster, should only run once
                try
                {
                    Corpus sample = new Corpus(Program.dirpath, Program.stoppath, Program.savefilepath);//, significantwordfile);
                }
                catch (Exception expt)
                {
                    MessageBox.Show("Error when loading database, please check if the database files are exist or the path isn't too long.");
                }
                
            }
            if (this.modeCbx.SelectedIndex == 1)
            {
                // Mode 2: Search
                Corpus sample = new Corpus(Program.stoppath, Program.savefilepath);
                String queryStr = queryTb.Text;
                Document queryDoc = sample.ProcessQuery("", queryStr);

                // Search for relevant documents
                List<Document> results = sample.SearchRelevantDocuments(queryDoc);
                String resultStr = "";
                foreach (Document doc in results)
                {
                    resultStr += doc.docname + Environment.NewLine;
                    Console.WriteLine(doc.docname);
                }
                resultsTb.Text = resultStr;
            }
        }

        private void modeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.modeCbx.SelectedIndex == 0)
            {
                this.executeBtn.Text = "Build database";
            }
            else if (this.modeCbx.SelectedIndex == 1)
            {
                this.executeBtn.Text = "Search";
            }
        }
    }
}
