using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Utilities;

namespace DataExtractor
{
    class Writer
    {
        private string _currentDirectory;

        /// <summary>
        /// Constructor
        /// Creates a directory for the given file to storage the text and images
        /// </summary>
        /// <param name="file"></param>
        public Writer(string file)
        {
            this._currentDirectory = Util.GetOutputFolderPath(file);

            Directory.CreateDirectory(this._currentDirectory);
            Directory.CreateDirectory(this._currentDirectory + "\\Imagenes");
            Directory.CreateDirectory(this._currentDirectory + "\\Texto");
        }

        /// <summary>
        /// Writes the file
        /// </summary>
        /// <param name="text"></param>
        public void WriteToFile(StringBuilder text)
        {
            using (var file = new StreamWriter(this._currentDirectory + "\\Texto\\text.txt"))
            {
                file.WriteLine(text.ToString());
                file.Close();
            }
        }

        /// <summary>
        /// Writes the file
        /// </summary>
        /// <param name="text"></param>
        public void WriteImages(List<Image> imagesList)
        {
            int counter = 1;

            foreach (var image in imagesList)
            {
                image.Save(_currentDirectory + "\\Imagenes\\" + counter + ".jpg");

                counter++;
            }
        }
    }
}
