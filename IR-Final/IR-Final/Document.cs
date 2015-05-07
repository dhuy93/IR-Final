using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR3
{
    class Document
    {
        public string docname;
        public int Wordnumber
        {
            get
            {
                return wordlist.Sum(p=>p.Value);
            }
        }
        public Dictionary<string, int> wordlist = new Dictionary<string,int>();
        public Vector<double> vector = null;
        public string category;

        public Document(string docname, string content, Document stoplist)
        {
            this.docname = docname;
            string[] list = SplitWord(content);
            foreach (string w in list)
            {
                if (w.Length > 0)
                {
                    if (wordlist.ContainsKey(w))
                        wordlist[w] += 1;
                    else wordlist.Add(w, 1);
                }
            }
            RemoveStopWord(stoplist);
        }

        public Document(string docname, string content, Document stoplist, SortedDictionary<string, int> vectorspace = null)
        {
            this.docname = docname;
            string[] list = SplitWord(content);
            foreach (string w in list)
                if (w.Length > 0 && !(vectorspace!=null && !vectorspace.ContainsKey(w)))
                    if (wordlist.ContainsKey(w))
                        wordlist[w] += 1;
                    else wordlist.Add(w,1);
            RemoveStopWord(stoplist);
        }

        public Document(FileInfo fileinfo, string category, Document stoplist)
        {
            docname = fileinfo.Name;
            this.category = category;
            StreamReader sr = new StreamReader(fileinfo.FullName);
            if (!sr.EndOfStream)
            {
                string[] list = SplitWord(sr);
                //wordnumber = list.Length;
                for (int i = 0; i < list.Length; ++i)
                    if (list[i].Length > 0)
                    {
                        if (wordlist.ContainsKey(list[i]))
                            wordlist[list[i]] += 1;
                        else wordlist.Add(list[i], 1);
                    }
            }
            sr.Close();
            RemoveStopWord(stoplist);
        }

        public Document(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            docname = fi.Name;
            StreamReader sr = new StreamReader(filepath);
            if (!sr.EndOfStream)
            {
                string[] list = SplitWord(sr);
                for (int i = 0; i < list.Length; ++i)
                    if (list[i].Length > 0)
                    {
                        if (wordlist.ContainsKey(list[i]))
                            wordlist[list[i]] += 1;
                        else wordlist.Add(list[i], 1);
                    }
            }
            sr.Close();
        }

        public Document(FileInfo fi)
        {
            docname = fi.Name;
            vector = Corpus.ReadVectorInBin(fi.FullName);
            wordlist = null;
        }

        public Document(string queryname, string querydescription, string category)
        {
            this.docname = queryname;
            string[] list = SplitWord(querydescription);
            //wordnumber = list.Length;
            for (int i = 0; i < list.Length; ++i)
                if (list[i].Length > 0)
                {
                    if (wordlist.ContainsKey(list[i]))
                        wordlist[list[i]] += 1;
                    else wordlist.Add(list[i], 1);
                }
            this.category = category;
            
        }

        private string[] SplitWord(string content)
        {
            char[] splitchar = new char[33];
            splitchar[0] = ' ';
            splitchar[1] = '\n';
            splitchar[2] = '\t';
            splitchar[3] = ',';
            splitchar[4] = '-';
            splitchar[5] = '/';
            splitchar[6] = '_';
            splitchar[7] = '(';
            splitchar[8] = ')';
            splitchar[9] = '[';
            splitchar[10] = ']';
            splitchar[11] = '{';
            splitchar[12] = '}';
            splitchar[13] = '<';
            splitchar[14] = '>';
            splitchar[15] = '?';
            splitchar[16] = ';';
            splitchar[17] = ':';
            splitchar[18] = '\'';
            splitchar[19] = '"';
            splitchar[20] = '|';
            splitchar[21] = '\\';
            splitchar[22] = '!';
            splitchar[23] = '@';
            splitchar[24] = '#';
            splitchar[25] = '$';
            splitchar[26] = '^';
            splitchar[27] = '&';
            splitchar[28] = '*';
            splitchar[29] = '`';
            splitchar[30] = '~';
            splitchar[31] = '=';
            splitchar[32] = '+';
            //splitchar[33] = '.';
            string[] rawlist = content.Split(splitchar,StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rawlist.Length; ++i)
                rawlist[i] = PreprocessWord(rawlist[i]);
            return rawlist;
        }
        private string[] SplitWord(StreamReader sr)
        {
            return SplitWord(sr.ReadToEnd());
        }

        private string PreprocessWord(string word)
        {
            char[] trimchar = new char[16];
            trimchar[0] = ' ';
            trimchar[1] = '"';
            trimchar[2] = '\'';
            trimchar[3] = '.';
            trimchar[4] = ',';
            trimchar[5] = '\n';
            trimchar[6] = '\r';
            trimchar[7] = '(';
            trimchar[8] = ')';
            trimchar[9] = '\t';
            trimchar[10] = '?';
            trimchar[11] = ';';
            trimchar[12] = ':';
            trimchar[13] = (char)8220;
            trimchar[14] = (char)8221;
            trimchar[15] = (char)8230;
            return word.ToLower().TrimStart(trimchar).TrimEnd(trimchar);            
        }

        public void RemoveStopWord(Document stoplist)
        {
            foreach (string stopword in stoplist.wordlist.Keys.ToArray())
                if (wordlist.ContainsKey(stopword))
                {
                    //wordnumber -= wordlist[stopword];
                    wordlist.Remove(stopword);
                }
        }

        public double TermFrequency(string term)
        {
            if (wordlist.ContainsKey(term))
                return wordlist[term];
            return 0;
        }

        public Vector<double> TermFrequencyVector(SortedDictionary<string, int> vectorspace)
        {
            List<double> termfrequencylist = new List<double>();
            foreach (KeyValuePair<string, int> pair in vectorspace)
            {
                if (wordlist.ContainsKey(pair.Key))
                    termfrequencylist.Add(wordlist[pair.Key]);
                else termfrequencylist.Add(0);
            }
            Vector<double> termfrequencyvector = Vector<double>.Build.DenseOfArray(termfrequencylist.ToArray());
            termfrequencyvector = termfrequencyvector.Normalize(1);
            return termfrequencyvector;
        }

        public double DocumentNearess(Document target)
        {
            double distance = vector.DotProduct(target.vector);
            return distance;
        }
    }
}

