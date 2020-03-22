using System;
using System.Collections.Generic;
using System.IO;

namespace qsv2flv
{
    /// <summary>
    /// Description of IqiyiFolderConverter.
    /// </summary>
    public class QiyiFolder
    {
        private static Dictionary<string,QiyiConfig> configMap = new Dictionary<string,QiyiConfig>();
        private string folderPath;

        public QiyiFolder(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public string QsvFilePath
        {
            get{ return this.getInnerFilePath(".qsv"); }
        }

        public string ConfigFilePath
        {
            get{ return this.getInnerFilePath(".qiyicfg"); }
        }

        public QiyiConfig Config
        {
            get
            {
                QiyiConfig config = null;
                if(! QiyiFolder.configMap.TryGetValue(this.folderPath, out config))
                {
                    config = new QiyiConfig(this.ConfigFilePath);
                    QiyiFolder.configMap[this.folderPath] = config;
                }

                return config;
            }
        }

        private string getInnerFilePath(string extension)
        {
            string folderName = Path.GetFileName(this.folderPath);
            return Path.Combine(this.folderPath, folderName + extension);
        }

        public static bool IsQiyiFolder(string folderPath)
        {
            if(!Directory.Exists(folderPath))
                return false;

            QiyiFolder folder = new QiyiFolder(folderPath);
            return File.Exists(folder.QsvFilePath) && File.Exists(folder.ConfigFilePath);
        }
    }
}
