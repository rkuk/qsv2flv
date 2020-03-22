using System;

namespace qsv2flv
{
    /// <summary>
    /// Description of QsvConverter.
    /// </summary>
    public abstract class QsvConverter
    {
        public string InputPath;

        public QsvConverter(string path)
        {
            this.InputPath = path;
        }

        public virtual string Convert()
        {
            string qsvPath = this.getQsvPath(this.InputPath);
            string flvPath = this.getFlvPath(this.InputPath);
            try {
                new Transcoder(qsvPath,flvPath).Transcode();

                return flvPath;
            }
            catch(Exception e)
            {
                throw new ConvertException(qsvPath,flvPath,e.Message);
            }
        }

        protected abstract string getQsvPath(string path);
        protected abstract string getFlvPath(string path);
    }
}
