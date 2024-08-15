using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spycraft
{
    internal class FileHander
    {
    }

    public class FIleOperations
    {
        public const int MADE_FILE_READ = 1;
        public const int MADE_FILE_WRITE = 2;
        public const int MADE_FILE_APPEND = 3;

        public static FileStream sfxOpenFile(string filename, int mode)
        {
            FileStream fileStream = null;
            FileMode fileMode = FileMode.Open;
            FileAccess fileAccess = FileAccess.Read;
            FileShare fileShare = FileShare.Read;

            switch (mode)
            {
                case MADE_FILE_READ:
                    fileMode = FileMode.Open;
                    fileAccess = FileAccess.Read;
                    fileShare = FileShare.Read;
                    break;

                case MADE_FILE_WRITE:
                    fileMode = FileMode.Create;
                    fileAccess = FileAccess.Write;
                    fileShare = FileShare.None;
                    break;

                case MADE_FILE_APPEND:
                    fileMode = FileMode.Append;
                    fileAccess = FileAccess.Write;
                    fileShare = FileShare.None;
                    break;

                default:
                    // handle invalid mode 
                    throw new ArgumentException("Invalid mode, buddy!");
            }

            try
            {
                fileStream = new FileStream(filename, fileMode, fileAccess, fileShare);
            }
            catch (IOException)
            {
                return null;
            }

            return fileStream;
        }

        public static int sfxWriteFile(FileStream fileStream, byte[] buffer, int size)
        {
            try
            {
                fileStream.Write(buffer, 0, size);
                return size;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file: " + ex.Message);
                return -1;
            }
        }

        public static int sfxReadFile(FileStream fileStream, byte[] buffer, int size)
        {
            try
            {
                fileStream.Read(buffer, 0, size);
                return size;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file: " + ex.Message);
                return -1;
            }
        }
    }
}
