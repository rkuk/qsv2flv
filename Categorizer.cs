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
			if(filePath==null)
				return null;
			
			bool success = filePath.EndsWith(".flv",StringComparison.CurrentCultureIgnoreCase);
			
			if(success)
			{
				//specified the target name of the file
				if(this.rootPath.EndsWith(".flv", StringComparison.CurrentCultureIgnoreCase))
					return this.moveFile(filePath, this.rootPath);
				
				return this.moveFile(filePath, this.getDestinationPath(filePath));
			}
			else if(this.rootPath!=null)
				return this.copyFile(filePath, this.getDestinationPath(filePath));
			else
				return null;
		}
		
		private string getDestinationPath(string filePath)
		{
			string folder = Path.GetDirectoryName(filePath);
			string name = Path.GetFileName(filePath);
			if(QiyiFolder.IsQiyiFolder(folder))
			{
				QiyiFolder qyFolder = new QiyiFolder(folder);
				QiyiConfig config = qyFolder.Config;
				name = config.Title + Path.GetExtension(name).ToLower();
				
				if(this.rootPath!=null)
					name = Path.Combine(config.Series,name);
			}
			
			//without specifying rootPath, it mean keep the files in its own place
			if(this.rootPath==null)
				return Path.Combine(folder,name);
			else
				return Path.Combine(this.rootPath,name);
		}

		private string moveFile(string src, string dest)
		{
			return this.sendFile(src,dest,false);
		}
		
		private string copyFile(string src, string dest)
		{
			return this.sendFile(src,dest,true);
		}
		
		private string sendFile(string src, string dest, bool isCopy)
		{
			string dir = Path.GetDirectoryName(dest);
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			dest = this.getUniquePath(dest);
			if(isCopy)
				File.Copy(src,dest);
			else
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
