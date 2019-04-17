using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CateringDB
{
    public sealed class FileStorage
    {
        private static readonly object singleLock = new object();
        private static FileStorage instance = null;

        public static FileStorage Instance
        {
            get
            {
                lock (singleLock)
                {
                    if (instance == null)
                    {
                        instance = new FileStorage();
                    }
                    return instance;
                }
            }
        }

        public void Set(string input, string path, bool mode)
        {
            StreamWriter sw = new StreamWriter(path, mode, Encoding.Default);
            sw.WriteLine(input);
            sw.Close();
        }

        public string Get(int line, string path, bool mode)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var textReader = new StreamReader(fileStream))
                {
                    string str;
                    List<string> lineList = new List<string>();
                    while (!string.IsNullOrEmpty((str = textReader.ReadLine())))
                    {

                        lineList.Add(str);
                    }
                    if (lineList.Count >= line)
                    {
                        return lineList[line - 1];
                    }
                    else
                    {
                        return "error";
                    }
                }                
            }
            catch (FileNotFoundException e)
            {
                FileStream fs = File.Create(path);
                byte[] buf = new byte[] { 0 };
                fs.Write(buf, 0, 1);
                return "0";
            }            
        }
    }
}
