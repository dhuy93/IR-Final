using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR3
{
    class MainProcess
    {
        // Add MathNet.Numerics.LinearAlgebra library to project
        // To choose between 2 way to build corpus
        
        // Remove stop word when create new corpus
        //Choose 2000 word as vector space when create new corpus

        //Computing Tf-idf after choose vector space
        //

        public static string stoppath;
        public static string baseDirectoryName = "IR-Final";
        

        public static void Main()
        {
            //Set directory for source file
            string exepath = AppDomain.CurrentDomain.BaseDirectory;
            stoppath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\stopwords_vn.txt";
            string dirpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\news_dataset";
            string vectorspaceresultpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\vpresult.txt";  // significant words
            string idfresultpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\idfresult.bin";
            string vectorresultpath = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\vectordata";
            //string popularwordfile = exepath.Substring(0, exepath.LastIndexOf(baseDirectoryName) + baseDirectoryName.Length) + "\\SignificantWordAsVectorSpace.txt";

            //string[] significantwordfile = new string[1] { popularwordfile };
            string[] savefilepath = new string[3] { vectorresultpath, idfresultpath, vectorspaceresultpath };
            //Choose method to create corpus

            // Mode 1: Create database to search faster, should only run once
            //Corpus sample = new Corpus(dirpath, stoppath, savefilepath);//, significantwordfile);

            // Mode 2: Search
            Corpus sample = new Corpus(savefilepath);
            //OutputDictionary(sample.MasterDictionary, sample, "wordfrequency.txt");
            //OutputDictionary(sample.vectorspace, "SignificantWordAsVectorSpace.txt");

        }

        private static SortedDictionary<string, int> QuerySignificantWord(List<Document> queries)
        {
            SortedDictionary<string, int> res = new SortedDictionary<string, int>();
            foreach (Document query in queries)
            {
                foreach (KeyValuePair<string, int> pair in query.wordlist)
                {
                    if (res.ContainsKey(pair.Key))
                        res[pair.Key] += pair.Value;
                    else res.Add(pair.Key, pair.Value);
                }
            }
            return res;
        }

        private static string GetQueryCategory(string queryfilename)
        {
            return queryfilename.Remove(queryfilename.LastIndexOf('.'));
        }

        private static void OutputDictionary(Dictionary<string, int> Dictionary, Corpus sample, string filename)
        {
            List<KeyValuePair<string,int>> l = Dictionary.OrderBy(p => p.Value).ToList();
            long total = l.Sum(p => p.Value);
            int highlimit = (int)total;
            int lowlimit = (int)Math.Log(total);
            l = l.Where(p => p.Value > lowlimit && p.Value < highlimit).ToList();
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(total.ToString());
            sw.WriteLine(lowlimit.ToString());
            foreach (KeyValuePair<string, int> p in l)
            {
                if (p.Value > lowlimit && p.Value < highlimit)
                {
                    sw.WriteLine(p.Key);
                    sw.WriteLine(p.Value.ToString());
                    sw.WriteLine();
                }
            }
            sw.Close();

            //SortedDictionary<string, int> SortedDictionary = new SortedDictionary<string, int>(Dictionary);
            //StreamWriter sw = new StreamWriter(filename);
            //foreach (string word in SortedDictionary.Keys.ToArray())
            //{
            //    sw.WriteLine(word);
            //    sw.WriteLine(SortedDictionary[word].ToString());
            //    sw.WriteLine();
            //}
            //sw.Close();
        }
        private static void OutputDictionary(SortedDictionary<string, int> SortedDictionary, string filename)
        {
            List<KeyValuePair<string, int>> l = SortedDictionary.OrderByDescending(p => p.Value).ToList();
            long total = l.Sum(p => p.Value);
            int highlimit = (int)(total/Math.Log(total));
            int lowlimit = (int)Math.Log(total);
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(total.ToString());
            sw.WriteLine(lowlimit.ToString());
            foreach (KeyValuePair<string, int> p in l)
            {
                if (p.Value > lowlimit && p.Value<highlimit)
                {
                    sw.WriteLine(p.Key);
                    sw.WriteLine(p.Value.ToString());
                    sw.WriteLine();
                }
            }
            sw.Close();
            //StreamWriter sw = new StreamWriter(filename);
            //foreach (string word in SortedDictionary.Keys.ToArray())
            //{
            //    sw.WriteLine(word);
            //    sw.WriteLine(SortedDictionary[word].ToString());
            //    sw.WriteLine();
            //}
            //sw.Close();
        }
    }
}
