using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace DataExtractor
{
    public class Extractor
    {
        private List<string> _files;
        public static ProgressBar progressBar;
        public static Label displayLabel;

        /// <summary>
        /// Class constructor. Takes a file name as parameter and performs
        /// operations over this file
        /// </summary>
        /// <param name="files">the file name</param>
        public Extractor(List<string> files)
        {

            this._files = files;
        }

        /// <summary>
        /// Extract the text and images
        /// Store each document in a separated folder, with the text in a txt file
        /// and the images
        /// </summary>
        public void Extract()
        {
            int counter = 1;
            foreach (var file in this._files)
            {
                displayLabel.BeginInvoke(
                    new Action(() => {

                        displayLabel.Text = String.Format("Procesando: {0}/{1}", counter, this._files.Count + 1);
                    }));

                this.Extract(file);

                counter++;
            }
        }

        /// <summary>
        /// Perform the actions to extract the text and images
        /// </summary>
        /// <param name=""></param>
        private void Extract(string file)
        {
            try
            {
                ///reads the text from the pdf
                var text = this.ReadText(file);

                ///Extract the images in the document
                var images = this.ExtractImages(file);

                var writer = new Writer(file);
                ///Writes the text to the output file
                writer.WriteToFile(text);
                writer.WriteImages(images);

            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Documento: {0}. \nDetalle: {1}",
                    Util.GetFileName(file), e.Message), "Error al procesar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                progressBar.BeginInvoke(
                new Action(() =>
                {
                    
                    var newProgress = 100 / this._files.Count;
                    if (progressBar.Value + newProgress > 100)
                    {
                        progressBar.Value =100;
                    }
                    else
                    {
                        progressBar.Value += newProgress;
                    }
                    
                }));
            }
        }

        /// <summary>
        /// Reads the text of the given page
        /// put the text in the given StringBuilder
        /// </summary>
        /// <param name="file"></param>
        /// <returns>String builder with the Data</returns>
        private StringBuilder ReadText(string file)
        {
            StringBuilder textBuilder = new StringBuilder();

            if (File.Exists(file))
            {
                PdfReader pdfReader = new PdfReader(file);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(
                        ASCIIEncoding.Convert(
                            Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)
                            ));

                    if (!String.IsNullOrEmpty(currentText))
                    {
                        textBuilder.Append(currentText);
                    }
                }
                pdfReader.Close();
            }
            return textBuilder;
        }

        /// <summary>
        /// Gets all the images in the given document
        /// </summary>
        /// <param name="file"></param>
        private List<Image> ExtractImages(string file)
        {
            var randomAccess = new RandomAccessFileOrArray(file);
            var reader = new PdfReader(randomAccess, null);
            List<Image> imgList = new List<Image>();

            for (int i = 0; i <= reader.XrefSize - 1; i++)
            {
                var pdfObject = reader.GetPdfObject(i);

                if ((pdfObject != null) && pdfObject.IsStream())
                {
                    var PDFStremObj = (PdfStream)pdfObject;
                    PdfObject subtype = PDFStremObj.Get(PdfName.SUBTYPE);

                    if ((subtype != null) && subtype.ToString() == PdfName.IMAGE.ToString())
                    {
                        PdfImageObject PdfImageObj = new PdfImageObject((PRStream)PDFStremObj);

                        Image ImgPDF = PdfImageObj.GetDrawingImage();


                        imgList.Add(ImgPDF);

                    }
                }    
            }

            reader.Close();
            return imgList;
        }

    }
}
