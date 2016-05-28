using System;
using System.IO;

namespace Utilities
{
    public static class Util
    {
        public static string SavePath;
        /// <summary>
        /// Gets the output folder path for the given document
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetOutputFolderPath(string file)
        {
            var fileName = Path.GetFileName(file).Replace(" ", string.Empty).Replace(".pdf", String.Empty);

            if (SavePath == String.Empty)
            {
                return AppDomain.CurrentDomain.BaseDirectory + "documents\\" + fileName;
            }
            else
            {
                return SavePath + "\\" + fileName;
            }
        }

        /// <summary>
        /// Gets the output folder path for the given document
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
