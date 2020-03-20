using System;
using System.IO;
using System.Collections.Generic;

namespace qsv2flv
{
    /// <summary>
    /// Description of QsvConverterFactory.
    /// </summary>
    public class QsvConverterFactory
    {
        public List<QsvConverter> CreateConverter(string path)
        {
            List<QsvConverter> converters = new List<QsvConverter>();

            if(Directory.Exists(path))
            {
                if(QiyiFolder.IsQiyiFolder(path))
                    converters.Add(new QiyiFolderConverter(path));
                else
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                    foreach(FileInfo qsv in dirInfo.GetFiles("*.qsv"))
                        converters.AddRange(this.CreateConverter(qsv.FullName));

                    foreach(DirectoryInfo folder in dirInfo.GetDirectories())
                        converters.AddRange(this.CreateConverter(folder.FullName));
                }
            }
            else if(File.Exists(path) && Path.GetExtension(path).ToUpper()==".QSV")
            {
                converters.Add(new QsvFileConverter(path));
            }
            // else;

            return converters;
        }
    }
}
