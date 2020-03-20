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

        public void Categorize(List<string> filePathList)
        {
            if(this.rootPath==null || filePathList.Count==0)
                return;//without specifying rootPath, it mean keep the files in its own place

            if(filePathList.Count==1 && this.rootPath.EndsWith(".flv",StringComparison.CurrentCultureIgnoreCase))
            {
                //specified the target name of the file
                this.moveFile(filePathList[0], this.rootPath);
                return;
            }

            foreach(string path in filePathList)
            {
                string folder = Path.GetDirectoryName(path);
                string name = Path.GetFileName(path);
                if(QiyiFolder.IsQiyiFolder(folder))
                {
                    QiyiFolder qyFolder = new QiyiFolder(folder);
                    QiyiConfig config = qyFolder.Config;
                    name = Path.Combine(config["clm"], name);
                }

                this.moveFile(path, Path.Combine(this.rootPath, name));
            }
        }

        private void moveFile(string src, string dest)
        {
            string dir = Path.GetDirectoryName(dest);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.Move(src,this.getUniquePath(dest));
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
