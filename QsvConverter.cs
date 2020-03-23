using System;
using System.IO;

namespace qsv2flv
{
	/// <summary>
	/// Description of QsvConverter.
	/// </summary>
	public class QsvConverter
	{
		public string QsvPath;

		public QsvConverter(string qsvPath)
		{
			this.QsvPath = qsvPath;
		}

		public virtual string Convert()
		{
			string flvPath = Path.ChangeExtension(this.QsvPath, "flv");
			new Transcoder(this.QsvPath, flvPath).Transcode();

			return flvPath;
		}
	}
}
