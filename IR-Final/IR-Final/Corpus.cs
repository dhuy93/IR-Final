using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR3
{
    class Corpus
    {
        public SortedDictionary<string, Document> documents = new SortedDictionary<string, Document>();
        public SortedDictionary<string, int> vectorspace = new SortedDictionary<string, int>();

        Vector<double> idfvector;
        int spacedegree = 500;
        Document stoplist;

        string[] savefilepath;

        Dictionary<string, int> masterdictionary = new Dictionary<string, int>();

        public Dictionary<string, int> MasterDictionary
        {
            get { return masterdictionary; }
            //set { masterdictionary = value; }
        }

        //public Corpus(string dirpath, string stopfilepath, string[] significantwordpaths)
        //{
        //    stoplist = new Document(stopfilepath);
        //    vectorspace = ReadSignificantWord(significantwordpaths);
        //    DirectoryInfo dirinfo = new DirectoryInfo(dirpath);
        //    int count = 0;
        //    foreach (FileInfo fileinfo in dirinfo.GetFiles())
        //    {
        //        StreamReader sr = new StreamReader(fileinfo.FullName);
        //        while (!sr.EndOfStream)
        //        {
        //            string content = sr.ReadToEnd();
        //            Document doc = new Document(fileinfo.Name, content, stoplist);
        //            if (doc!=null && !documents.ContainsKey(doc.docname))
        //            {
        //                count++;
        //                documents.Add(doc.docname, doc);
        //                if (count % 100 == 0)
        //                    Console.WriteLine(count + " dictionary created.");
        //            }
        //        }
        //    }
        //    CreateMasterDictionarySinglePass();
        //    CalculateDocumentsVector();
        //    //GenerateRandomVectorSpace(5000);
        //}

        public Corpus(string dirpath, string stopfilepath, string[] savefilepath)
        {
            this.savefilepath = savefilepath;
            stoplist = new Document(stopfilepath);
            DirectoryInfo dirinfo = new DirectoryInfo(dirpath);
            int count = 0;
            foreach (FileInfo fileinfo in dirinfo.GetFiles())
            {
                StreamReader sr = new StreamReader(fileinfo.FullName);
                string content = sr.ReadToEnd();
                Document doc = new Document(fileinfo.Name, content,stoplist);
                if (doc != null && !documents.ContainsKey(doc.docname))
                {
                    count++;
                    documents.Add(doc.docname, doc);
                    if (count % 100 == 0)
                        Console.WriteLine(count + " dictionary created.");
                }
                sr.Close();
            }
            CreateMasterDictionarySinglePass();
            CalculateDocumentsVector2();
        }

        public Corpus(string stopfilepath,string[] savefilepath)
        {
            this.savefilepath = savefilepath;
            stoplist = new Document(stopfilepath);
            Document vectorspacedoc = new Document(savefilepath[2]);
            vectorspace = new SortedDictionary<string, int>(vectorspacedoc.wordlist);
            idfvector = ReadVectorInBin(savefilepath[1]);
            DirectoryInfo vectordirinfo = new DirectoryInfo(savefilepath[0]);
            int count = 0;
            foreach (FileInfo fi in vectordirinfo.GetFiles())
            {
                count++;
                documents.Add(fi.Name, new Document(fi));
                if (count % 100 == 0)
                    Console.WriteLine(count.ToString() +" documents processed.");
                //if (count > 10000)
                //    break;
            }
        }

        private void CalculateDocumentsVector2()
        {
            long totalword = MasterDictionary.Values.Sum();
            int highlimit = (int)(0.5 * documents.Count);
            int lowlimit = (int)(4 * Math.Log(totalword));
            IEnumerable<KeyValuePair<string, int>> temporaryDictionary = MasterDictionary.Where(p => (p.Value >= lowlimit && p.Value <= highlimit));
            Dictionary<string, int> temp2 = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> p in temporaryDictionary)
            {
                if (temp2.ContainsKey(p.Key))
                    temp2[p.Key] += p.Value;
                else temp2.Add(p.Key, p.Value);
            }
            vectorspace = GetRandom(temp2, 2500);

            List<KeyValuePair<string,int>> vlist = vectorspace.ToList();
            StreamWriter sw = new StreamWriter(savefilepath[2]);
            for (int i = 0; i < vlist.Count; ++i)
            {
                sw.WriteLine(vlist[i].Key);
                //sw.WriteLine(vlist[i].Value.ToString());
                //sw.WriteLine();
            }
            sw.Close();

            idfvector = CalculateIDFVector(vectorspace);
            WriteVectorInBin(idfvector, savefilepath[1]);
            
            int count = 0;            
            foreach (Document doc in documents.Values)
            {
                CalculateVector2(doc, vectorspace, savefilepath[0]);
                doc.wordlist = null;                
                count++;
                if (count % 1000 == 0)
                    Console.WriteLine(count + " documents vector calculated.");
            }
        }

        private void CalculateVector2(Document doc, SortedDictionary<string, int> vectorspace, string dirpath)
        {
            Vector<double> termfrequencyvector = doc.TermFrequencyVector(vectorspace);
            Vector<double> res = termfrequencyvector.Map2((t, i) => t * i, idfvector);
            res = res.Normalize(1);
            WriteVectorInBin(res, dirpath + "\\" + doc.docname + ".bin");
        }

        public static void WriteVectorInBin(Vector<double> vector, string path)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create));
            foreach (double d in vector.ToArray())
                bw.Write(d);
            bw.Close();
        }
        public static Vector<double> ReadVectorInBin(string path)
        {
            BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open));
            List<double> list = new List<double>();
            while (br.BaseStream.Position!=br.BaseStream.Length)
            {
                list.Add(br.ReadDouble());
            }
            return Vector<double>.Build.DenseOfArray(list.ToArray());
        }

        //public Corpus(string filepath, string stopfilepath, string[] significantwordpaths)
        //{
        //    stoplist = new Document(stopfilepath, "stoplist");
        //    vectorspace = ReadSignificantWord(significantwordpaths);
        //    FileInfo fi = new FileInfo(filepath);
        //    StreamReader sr = new StreamReader(filepath);
        //    int count = 0;
        //    while (!sr.EndOfStream)
        //    {
        //        string content = ReadOneDocContent(sr);
        //        if (content.Length > 0)
        //        {
        //            Document document = new Document(content, vectorspace);
        //            if (document.Wordnumber > 0)
        //            {
        //                document.RemoveStopWord(stoplist);
        //                if (!documents.ContainsKey(document.docname))
        //                    documents.Add(document.docname, document);
        //                count++;
        //                if (count % 1000 == 0)
        //                    Console.WriteLine(count + " dictionary created.");
        //            }
        //        }
        //    }
        //    CreateMasterDictionarySinglePass();
        //    CalculateDocumentsVector();

        private SortedDictionary<string, int> ReadSignificantWord(string[] significantwordpaths)
        {
            SortedDictionary<string, int> res = new SortedDictionary<string, int>();
            foreach (string significantwordpath in significantwordpaths)
        //}
            {
                StreamReader sr = new StreamReader(significantwordpath);
                while (!sr.EndOfStream)
                {
                    string word = "";
                    while ((word == null || word.Length == 0) && !sr.EndOfStream)
                        word = sr.ReadLine();
                    string number = "";
                    while ((number == null || number.Length == 0) && !sr.EndOfStream)
                        number = sr.ReadLine();
                    if (word != null && word.Length > 0 && number != null && number.Length > 0)
                    {
                        if (res.ContainsKey(word))
                            res[word] += Int32.Parse(number);
                        else res.Add(word, Int32.Parse(number));
                    }
                }
                sr.Close();
            }
            return res;
        }

        //public Corpus(string filepath, string stopfilepath, int spacedegree, bool singlepass = true)
        //{
        //    stoplist = new Document(stopfilepath, "stoplist");
        //    FileInfo fi = new FileInfo(filepath);
        //    StreamReader sr = new StreamReader(filepath);
        //    int count = 0;
        //    while (!sr.EndOfStream)
        //    {
        //        string content = ReadOneDocContent(sr);
        //        if (content.Length > 0)
        //        {
        //            Document document = new Document(content);
        //            if (document.Wordnumber > 0)
        //            {
        //                document.RemoveStopWord(stoplist);
        //                if (!documents.ContainsKey(document.docname))
        //                    documents.Add(document.docname, document);
        //                count++;
        //                if (count % 1000 == 0)
        //                    Console.WriteLine(count + " dictionary created.");
        //            }
        //        }
        //    }
        //    CreateMasterDictionarySinglePass();

        //    GenerateRandomVectorSpace(spacedegree);


        //}

        private static void PrintDictionary(StreamWriter sw, SortedDictionary<string, int> dictionary)
        {
            foreach (string word in dictionary.Keys.ToArray())
            {
                sw.WriteLine(word);
                sw.WriteLine(dictionary[word].ToString());
            }
        }

        private void GenerateRandomVectorSpace(int spacedegree)
        {
            this.spacedegree = spacedegree;
            long totalword = MasterDictionary.Values.Sum();
            //Arbitrary lowlomit
            int lowlimit = (int)(5 * Math.Log10(totalword));
            //Arbitrary highlimit
            int highlimit = (int)(totalword / (8 * lowlimit));
            IEnumerable<KeyValuePair<string, int>> temporaryDictionary = MasterDictionary.Where(p => (p.Value >= lowlimit && p.Value <= highlimit));
            Dictionary<string, int> SignificantDictionary = new Dictionary<string, int>(temporaryDictionary.ToDictionary(p => p.Key, p => p.Value));

            vectorspace = GetRandom(SignificantDictionary, spacedegree);

            CalculateDocumentsVector();
        }

        private void CalculateDocumentsVector()
        {
            idfvector = CalculateIDFVector(vectorspace);
            int count = 0;
            foreach (Document doc in documents.Values)
            {
                doc.vector = CalculateVector(doc, vectorspace); 
                //doc.wordlist = null;
                count++;
                if (count % 1000 == 0)
                    Console.WriteLine(count + " documents vector calculated.");
            }
        }

        private Vector<double> CalculateIDFVector(SortedDictionary<string, int> vectorspace)
        {
            List<double> idflist = new List<double>();
            int count = 0;
            foreach (KeyValuePair<string, int> pair in vectorspace)
            {
                idflist.Add(InverseDocumentFrequency(pair.Key));
                count++;
                if (count % 1000 == 0)
                    Console.WriteLine(count + " keys processed");
            }
            VectorBuilder<double> vb = Vector<double>.Build;
            Vector<double> idfvector = vb.Dense(idflist.ToArray());
            idfvector = idfvector.Normalize(1);
            return idfvector;
        }

        private Vector<double> CalculateVector(Document doc, SortedDictionary<string, int> vectorspace)
        {
            Vector<double> termfrequencyvector = doc.TermFrequencyVector(vectorspace);
            Vector<double> res = termfrequencyvector.Map2((t, i) => t * i, idfvector);
            return res.Normalize(1);
        }

        public double InverseDocumentFrequency(string word)
        {
            if (stoplist.wordlist.ContainsKey(word))
                return 0;
            else
            {
                int containdoc = documents.Count(doc => doc.Value.wordlist.ContainsKey(word));
                double result = Math.Log10(documents.Count / (containdoc == 0 ? 1 : containdoc));
                return result;
            }
        }

        void CreateMasterDictionarySinglePass()
        {
            int count = 0;
            foreach (Document doc in documents.Values)
            {
                foreach (string word in doc.wordlist.Keys.ToArray())
                {
                    if (masterdictionary.ContainsKey(word))
                        masterdictionary[word] += doc.wordlist[word];
                    else masterdictionary.Add(word, doc.wordlist[word]);
                }
                count++;
                if (count % 100 == 0)
                    Console.WriteLine(count + " documents processed.");
            }
        }

        public static SortedDictionary<string, int> GetRandom(Dictionary<string, int> MasterDictionary, int spacedegree)
        {
            Random rand = new Random();
            SortedDictionary<string, int> result = new SortedDictionary<string, int>();
            if (MasterDictionary.Count < spacedegree)
                return new SortedDictionary<string,int>(MasterDictionary);
            while (result.Count < spacedegree)
            {
                int index = rand.Next() % MasterDictionary.Count;
                KeyValuePair<string, int> pair = MasterDictionary.ElementAt(index);
                if (pair.Value > 2 && !result.ContainsKey(pair.Key))
                {
                    result.Add(MasterDictionary.ElementAt(index).Key, MasterDictionary.ElementAt(index).Value);
                    Console.WriteLine(result.Count + " words chosen.");
                }
            }

            return result;
        }

        internal List<Document> SearchRelevantDocuments(Document query)
        {
            Vector<double> queryvector = CalculateVector(query, vectorspace);
            List<Document> result = new List<Document>();

            int count = 0;
           
                double mincosin = 0.5;
                foreach (Document doc in documents.Values)
                {
                    count++;
                    double cosin = 0;
                    if (doc.vector.L2Norm() * queryvector.L2Norm() != 0)
                        cosin = doc.vector.DotProduct(queryvector) / (doc.vector.L2Norm() * queryvector.L2Norm());
                    if (cosin > mincosin)
                    {
                        result.Add(doc);
                    }

                    if (count % 1000 == 0)
                        Console.WriteLine("No feedback: Cosine compared: " + count + " files");
                }
           
            return result.ToList();
        }

        public Document ProcessQuery(string queryname, string querycontent)
        {
            Document query = new Document(queryname, querycontent, stoplist, vectorspace);
            query.vector = CalculateVector(query, vectorspace);
            return query;
        }
    }
}
