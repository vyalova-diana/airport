using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace CateringDB
{
    class Status
    {
        private readonly string file = "Status.txt";

        public void SaveStatus(int input)
        {
            StreamWriter sw = new StreamWriter(file, false, Encoding.Default);
            sw.WriteLine(input.ToString());
        }

        public int GetStatus()
        {

            try
            {
                StreamReader sr = new StreamReader(file);
                var temp = sr.ReadToEnd();

                return Convert.ToInt32(temp);
            }
            catch (FileNotFoundException e)
            {
                FileStream fs = File.Create("Status.txt");
                byte[] buf = new byte[] { 0 };
                fs.Write(buf, 0, 1);

                return 0;
            }
        }
    }
}

