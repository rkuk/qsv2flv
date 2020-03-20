using System;
using System.IO;

namespace qsv2flv
{
    /// <summary>
    /// Description of IqiyiFolderConverter.
    /// </summary>
    public class QiyiFolderConverter : QsvConverter
    {
        private QiyiFolder folder;
        public QiyiFolderConverter(string folderPath)
            :base(folderPath)
        {
            this.folder = new QiyiFolder(folderPath);
        }

        protected override string getQsvPath(string folderPath)
        {
            return this.folder.QsvFilePath;
        }

        protected override string getFlvPath(string folderPath)
        {
            QiyiConfig config = this.folder.Config;
            return Path.Combine(folderPath, config["episode"] + config["subTitle"] + ".flv");
        }
    }
}
