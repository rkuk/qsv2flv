using System;

namespace qsv2flv
{
    /// <summary>
    /// Description of QsvConverter.
    /// </summary>
    public class ConvertException : Exception
    {
        public string QsvPath;
        public string FlvPath;

        public ConvertException(string qsvPath, string flvPath, string message)
            :base(message)
        {
            this.QsvPath = qsvPath;
            this.FlvPath = flvPath;
        }
    }
}
