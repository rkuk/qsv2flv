using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace qsv2flv
{
    public class Transcoder
    {
        // private FileStream temp, qsv;
        // private FlvWriter flv;
        private string qsvPath, flvPath;

        /// <summary>
        /// Transcode
        /// </summary>
        //Constructed Code
        public Transcoder(string qsvPath, string flvPath)
        {
            this.qsvPath = qsvPath;
            this.flvPath = flvPath;
        }

        public void Transcode()
        {
            string tempPath = this.flvPath + ".temp";
            FileInfo tempfile = new FileInfo(tempPath);
            if (!Directory.Exists(tempfile.DirectoryName))
                Directory.CreateDirectory(tempfile.DirectoryName);
            if (tempfile.Exists)
                tempfile.Delete();

            bool error = false;
            FileStream qsv = null, temp = null;
            try
            {
                qsv = new FileStream(this.qsvPath, FileMode.Open);
                temp = new FileStream(tempfile.FullName,FileMode.CreateNew);

                SeekBegin(qsv);
                SkipMeta(qsv);
                while (true)
                {
                    // Console.WriteLine("loop");
                    try
                    {
                        if (ReadNext(qsv))
                        {
                            byte[] buffer = GetTag(qsv);
                            temp.Write(buffer, 0, buffer.GetLength(0));
                        }
                        else
                        {
                            SkipMeta(qsv);
                            SeekNextTag(qsv);
                            SeekNextTag(qsv);
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }
            }
            catch
            {
                error = true;
                throw;
            }
            finally
            {
                if(qsv!=null)
                {
                    qsv.Close();
                    qsv.Dispose();
                }
                if(temp!=null)
                {
                    temp.Close();
                    temp.Dispose();
                    if(error && File.Exists(tempPath))
                        File.Delete(tempPath);
                }
            }

            FlvWriter flv = null;
            try
            {
                flv = new FlvWriter(this.flvPath, tempPath);
                flv.Parse();
                flv.Output();
            }
            catch
            {
                error = true;
                throw;
            }
            finally
            {
                if(flv!=null)
                {
                    flv.Dispose();
                    if(error && File.Exists(this.flvPath))
                        File.Delete(this.flvPath);
                }

                if(File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }

        /// <summary>
        /// Transcode Tools
        /// </summary>
        private bool CheckTAG(FileStream qsv)
        {
            byte[] buffer = new byte[10];
            byte[] tag = Encoding.ASCII.GetBytes("QIYI VIDEO");
            qsv.Seek(0L, SeekOrigin.Begin);
            qsv.Read(buffer, 0, 10);
            return buffer == tag ? true : false;
        }

        private bool CheckVersion(FileStream qsv)
        {
            byte[] buffer = new byte[4];
            qsv.Seek(10L, SeekOrigin.Begin);
            qsv.Read(buffer, 0, 4);
            return BytesToInt(buffer, 0) == 2 ? true : false;
        }

        private void SeekBegin(FileStream qsv)
        {
            long offset = 0L;
            int size = 0;
            byte[] buffer = new byte[12];
            qsv.Seek(74L, SeekOrigin.Begin);
            qsv.Read(buffer, 0, 12);
            offset = BytesToLong(buffer, 0);
            size = BytesToInt(buffer, 8);
            qsv.Seek(offset + size, SeekOrigin.Begin);
        }

        private void SkipMeta(FileStream qsv)
        {
            long maxLen = qsv.Length-4;
            qsv.Position += 0xD;
            int len = 0;
            while (true)
            {
                int bit = qsv.ReadByte();
                ++len;
                if(bit == -1 || qsv.Position > maxLen)
                    throw new Exception("No meta skipped");
                else if (bit == 0x9)
                {
                    byte[] buffer = new byte[4];
                    qsv.Read(buffer, 0, 4);
                    int size =
                        (0xFF & buffer[0]) << 24 | (0xFF & buffer[1]) << 16 | (0xFF & buffer[2]) << 8 | (0xFF & buffer[3]);
                    if (len == size) break;
                    else qsv.Position -= 4;
                }
            }
        }

        private void SeekNextTag(FileStream qsv)
        {
            int dataSize = 0;
            byte[] buffer = new byte[4];
            qsv.Read(buffer, 0, 4);
            dataSize = (0xFF & buffer[1]) << 16 | (0xFF & buffer[2]) << 8 | (0xFF & buffer[3]);
            qsv.Seek(dataSize + 11, SeekOrigin.Current);
        }

        private byte[] GetTag(FileStream qsv)
        {
            int dataSize = 0;
            byte[] buffer = new byte[4];
            qsv.Read(buffer, 0, 4);
            qsv.Position -= 4;
            dataSize = (0xFF & buffer[1]) << 16 | (0xFF & buffer[2]) << 8 | (0xFF & buffer[3]);
            byte[] tag = new byte[dataSize + 15];
            qsv.Read(tag, 0, dataSize + 15);
            return tag;
        }

        private bool ReadNext(FileStream qsv)
        {
            if (qsv.Position >= qsv.Length - 11)
            {
                throw new EndOfStreamException();
            }
            byte[] buffer = new byte[11];
            qsv.Read(buffer, 0, 11);
            qsv.Position -= 11;
            if ((buffer[8] | buffer[9] | buffer[10]) == 0 && (buffer[0] == 0x8 || buffer[0] == 0x9))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int BytesToInt(byte[] paramArrayOfByte, int paramInt)
        {
            return 0xFF & paramArrayOfByte[paramInt] | (0xFF & paramArrayOfByte[(paramInt + 1)]) << 8 | (0xFF & paramArrayOfByte[(paramInt + 2)]) << 16 | (0xFF & paramArrayOfByte[(paramInt + 3)]) << 24;
        }

        private long BytesToLong(byte[] paramArrayOfByte, int paramInt)
        {
            int i = 0xFF & paramArrayOfByte[paramInt] | (0xFF & paramArrayOfByte[(paramInt + 1)]) << 8 | (0xFF & paramArrayOfByte[(paramInt + 2)]) << 16 | (0xFF & paramArrayOfByte[(paramInt + 3)]) << 24;
            int j = 0xFF & paramArrayOfByte[(paramInt + 4)] | (0xFF & paramArrayOfByte[(paramInt + 5)]) << 8 | (0xFF & paramArrayOfByte[(paramInt + 6)]) << 16 | (0xFF & paramArrayOfByte[(paramInt + 7)]) << 24;
            return i + j;
        }
    }
}
