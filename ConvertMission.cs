using System;
using System.IO;
using System.Collections.Generic;

namespace qsv2flv
{
    class ConvertMission
    {
        public List<QsvConverter> ConverterList;
        public string SavePath;

        public ConvertMission(string[] args)
        {
            this.parseMission(args);
        }

        private void parseMission(string[] args)
        {
            string savePath = null;
            List<string> qsvList = new List<string>();

            if(args.Length > 0)
            {
                savePath = args[args.Length-1];
                qsvList.AddRange(args);

                if(File.Exists(savePath))
                    savePath = null;
                else if(Directory.Exists(savePath))
                {
                    if(this.isQsvInFolder(new DirectoryInfo(savePath)))
                        savePath = null;
                    else
                        qsvList.RemoveAt(qsvList.Count-1);
                }
                else
                    qsvList.RemoveAt(qsvList.Count-1);
            }

            if(qsvList.Count == 0)
                qsvList.Add(Environment.CurrentDirectory);

            qsvList = qsvList.FindAll(p => Directory.Exists(p) ||
                    (File.Exists(p) && p.EndsWith(".qsv", StringComparison.OrdinalIgnoreCase)));

            QsvConverterFactory factory = new QsvConverterFactory();
            List<QsvConverter> converterList = new List<QsvConverter>();
            foreach(string qsv in qsvList)
            {
                List<QsvConverter> converters = factory.CreateConverter(qsv);
                converterList.AddRange(converters);
            }

            this.ConverterList = converterList;
            this.SavePath = savePath;
        }

        private bool isQsvInFolder(DirectoryInfo dirInfo)
        {
            if(dirInfo.GetFiles("*.qsv").Length>0)
                return true;

            foreach(DirectoryInfo subdirInfo in dirInfo.GetDirectories())
            {
                if(isQsvInFolder(subdirInfo))
                    return true;
            }

            return false;
        }
    }
}
