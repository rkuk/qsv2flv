using System;
using System.IO;

namespace qsv2flv
{
	/// <summary>
	/// Description of QsvFileConverter.
	/// </summary>
	public class QsvFileConverter : QsvConverter
	{
		public QsvFileConverter(string filePath)
			:base(filePath)
		{
		}
		
		protected override string getQsvPath(string filePath)
		{
			return filePath;
		}
		
		protected override string getFlvPath(string filePath)
		{
			return Path.ChangeExtension(filePath,"flv");
		}
	}
}
