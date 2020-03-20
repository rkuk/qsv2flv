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
            Transcoder trans = null;
            try {
                string qsvPath = this.getQsvPath(this.InputPath);
                string flvPath = this.getFlvPath(this.InputPath);
                trans = new Transcoder(qsvPath,flvPath);
                trans.Transcode();

                return flvPath;
            }
            catch
            {
                throw new Exception(this.InputPath);
            }
        }

        protected abstract string getQsvPath(string path);
        protected abstract string getFlvPath(string path);
    }
}
