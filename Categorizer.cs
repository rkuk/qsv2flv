using System;
using System.Collections.Generic;
using System.IO;

namespace qsv2flv
{
    /// <summary>
    /// Description of QsvConverter.
    /// </summary>
    public class Categorizer
    {
        private string rootPath;
        public Categorizer(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public string Categorize(string filePath)
        {
            //without specifying rootPath, it mean keep the files in its own place
            if(this.rootPath==null || filePath==null)
                return filePath;

            //specified the target name of the file
            if(this.rootPath.EndsWith(".flv",StringComparison.CurrentCultureIgnoreCase))
                return this.moveFile(filePath, this.rootPath);

            string folder = Path.GetDirectoryName(filePath);
            string name = Path.GetFileName(filePath);
            if(QiyiFolder.IsQiyiFolder(folder))
            {
                QiyiFolder qyFolder = new QiyiFolder(folder);
                QiyiConfig config = qyFolder.Config;
                name = Path.Combine(config.Series, name);
            }

            return this.moveFile(filePath, Path.Combine(this.rootPath, name));
        }

        private string moveFile(string src, string dest)
        {
            string dir = Path.GetDirectoryName(dest);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            dest = this.getUniquePath(dest);
            File.Move(src, dest);
            return dest;
        }

        private string getUniquePath(string filePath)
        {
            if(File.Exists(filePath))
            {
                string dir = Path.GetDirectoryName(filePath),
                       ext = Path.GetExtension(filePath),
                       name = Path.GetFileNameWithoutExtension(filePath);
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");

                return Path.Combine(dir, name + "_" + time + ext);
            }

            return filePath;
        }
    }
}
