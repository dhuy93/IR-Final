using IR3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IR_Final
{
    static class Program
    {
        public static string stoppath;
        public static string baseDirectoryName = "IR-Final";
        public static string dirpath = null;
        public static string[] savefilepath = null;
        public static string[] modes = {"Build mode", "Search mode"};

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Set directory for source file
            string exepath = AppDomain.CurrentDomain.BaseDirectory;
            stoppath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\stopwords_vn.txt";
            dirpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\news_dataset";
            string vectorspaceresultpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\vpresult.txt";  // significant words
            string idfresultpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\idfresult.bin";
            string vectorresultpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\vectordata";
            //string popularwordfile = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\SignificantWordAsVectorSpace.txt";

            //string[] significantwordfile = new string[1] { popularwordfile };
            savefilepath = new string[3] { vectorresultpath, idfresultpath, vectorspaceresultpath };
            //Choose method to create corpus



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            return;
            //// Mode 1: Create database to search faster, should only run once
            ////Corpus sample = new Corpus(dirpath, stoppath, savefilepath);//, significantwordfile);

            //// Mode 2: Search
            //Corpus sample = new Corpus(stoppath, savefilepath);
            ////OutputDictionary(sample.MasterDictionary, sample, "wordfrequency.txt");
            ////OutputDictionary(sample.vectorspace, "SignificantWordAsVectorSpace.txt");
            //string queryStr = Console.ReadLine();
            //Document queryDoc = sample.ProcessQuery("", queryStr);

            //// Search for relevant documents
            //List<Document> results = sample.SearchRelevantDocuments(queryDoc);
            //foreach (Document doc in results)
            //{
            //    Console.WriteLine(doc.docname);
            //}


        }
    }
}
