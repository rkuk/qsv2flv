using System;
using System.IO;
using System.Collections.Generic;

namespace qsv2flv
{
    class Program
    {
        public static void Main(string[] args)
        {
            ConvertMission mission = new ConvertMission(args);

            if(mission.ConverterList.Count == 0)
            {
                print("No qsv file found");
                return;
            }

            print(ConsoleColor.Yellow, "{0} qsv files to convert",mission.ConverterList.Count);

            List<string> flvList = runMission(mission);

            print(ConsoleColor.Yellow, "Converting complete.(Success:{0} Fail:{1})", flvList.Count, mission.ConverterList.Count-flvList.Count);

            if(mission.SavePath!=null && flvList.Count>0)
            {
                print("Moving flv files");
                Categorizer mover = new Categorizer(mission.SavePath);
                mover.Categorize(flvList);
                print("Moving complete");
            }
        }

        private static List<string> runMission(ConvertMission mission)
        {
            List<string> flvList = new List<string>();
            int index = 1;
            foreach(QsvConverter converter in mission.ConverterList)
            {
                try
                {
                    print("info: convert {0} {1}", index++, converter.InputPath);
                    string flvPath = converter.Convert();
                    flvList.Add(flvPath);
                    print(ConsoleColor.Green, "      save to {0}", flvPath);
                }
                catch (Exception e)
                {
                    print(ConsoleColor.Red, "error: fail to convert {0}", e.Message);
                }
            }

            return flvList;
        }

        private static void print(ConsoleColor color,string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void print(string format, params object[] args)
        {
            print(ConsoleColor.White, format, args);
        }
    }
}
