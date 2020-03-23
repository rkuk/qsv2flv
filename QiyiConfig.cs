using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;

namespace qsv2flv
{
    /// <summary>
    /// Description of IqiyiFolderConverter.
    /// </summary>
    public class QiyiConfig
    {
        private string configFilePath;
        private Dictionary<string,string> configs;

        public QiyiConfig(string configFilePath)
        {
            this.configFilePath = configFilePath;
            this.configs = null;
        }

        public string Series
        {
            get { return this["clm"]; }
        }

        public string Title
        {
            get { return this["episode"] + " " + this["subTitle"]; }
        }

        private string this[string configName]
        {
            get
            {
                if(this.configs == null)
                    this.parseConfig();

                return this.configs[configName];
            }
        }

        private void parseConfig()
        {
            this.configs = new Dictionary<string,string>();
            foreach(string line in File.ReadAllLines(this.configFilePath))
            {
                int sep = line.IndexOf("=");
                if(sep>0)
                    this.configs[line.Substring(0,sep)] = this.unicodeDecode(line.Substring(sep+1).Trim());
            }
        }

        private string unicodeDecode(string str)
        {
            string result = "";

            if(str.Length<6)
                result = str;
            else if(str.StartsWith(@"\u",StringComparison.CurrentCultureIgnoreCase))
            {
                string code1 = str.Substring(2,2),
                       code2 = str.Substring(4,2);

                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(code1, NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(code2, NumberStyles.HexNumber).ToString());
                result = Encoding.Unicode.GetString(bytes) + this.unicodeDecode(str.Substring(6));
            }
            else
                result = str.Substring(0,1) + this.unicodeDecode(str.Substring(1));

            return result;
        }
    }
}
