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

            int convertCount = mission.ConverterList.Count;
            if(convertCount == 0)
            {
                print("No qsv file found");
                return;
            }
            else if(convertCount>1 && mission.SavePath.EndsWith(".flv",StringComparison.CurrentCultureIgnoreCase))
            {
            	print("Can't convert multiple qsf files to a single flv");
            	return;
            }

            print(ConsoleColor.Yellow, "{0} qsv files to convert",mission.ConverterList.Count);

            int successCount = runMission(mission);

            print(ConsoleColor.Yellow, "Complete.(Success:{0} Fail:{1})",successCount, mission.ConverterList.Count-successCount);
        }

        private static int runMission(ConvertMission mission)
        {
            Categorizer mover = new Categorizer(mission.SavePath);
            int index = 1, successCount = 0;
            foreach(QsvConverter converter in mission.ConverterList)
            {
                try
                {
                    print("info: convert({0}) {1}", index++, converter.QsvPath);
                    string flvPath = converter.Convert();
                    successCount++;
                    flvPath = mover.Categorize(flvPath);
                    print(ConsoleColor.Green, "      save to {0}", flvPath);
                }
                catch (Exception e)
                {
                    mover.Categorize(converter.QsvPath);
                    print(ConsoleColor.Red, "error: ({0}) {1}", e.Message, converter.QsvPath);
                }
            }

            return successCount;
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
