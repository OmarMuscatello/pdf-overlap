using iTextSharp.text.pdf;
using System;
using System.IO;

namespace pdf_overlap
{
    class Program
    {
        private static void Main(string[] args)
        {
            const string baseFilePath = @"<File to be overlapped>";
            const string overFilePath = @"<File to overlap>";
            const string outputFilePath = @"<Output file>";

            var result = Overlap(baseFilePath, overFilePath, outputFilePath);

            if (result.HasError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("DONE!");
            }

            Console.ReadKey();
        }

        private class Result
        {
            public string Error { get; set; }
            public bool HasError { get => !string.IsNullOrEmpty(Error); }
        }

        /// <summary>
        /// Overlaps each page of <paramref name="overFilePath"/> on the respective page of <paramref name="baseFilePath"/>. 
        /// </summary>
        /// <param name="baseFilePath">File to be overlapped</param>
        /// <param name="overFilePath">File to overlap</param>
        /// <param name="outputFilePath">Output file</param>
        /// <returns></returns>
        private static Result Overlap(string baseFilePath, string overFilePath, string outputFilePath)
        {
            var result = new Result();

            PdfReader overReader = null;
            PdfReader baseReader = null;
            PdfStamper stamper = null;

            try
            {
                overReader = new PdfReader(overFilePath);
                baseReader = new PdfReader(baseFilePath);

                stamper = new PdfStamper(overReader, new FileStream(outputFilePath, FileMode.Create));

                var numberOfPages = Math.Min(baseReader.NumberOfPages, overReader.NumberOfPages);

                PdfContentByte background;

                for (var i = 1; i <= numberOfPages; i++)
                {
                    var page = stamper.GetImportedPage(baseReader, i);

                    var pageWidth = overReader.GetPageSizeWithRotation(i).Width;
                    var pageHeight = overReader.GetPageSizeWithRotation(i).Height;

                    background = stamper.GetUnderContent(i);

                    switch (page.Rotation)
                    {
                        case 0:
                            background.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            break;
                        case 90:
                            background.AddTemplate(page, 0, -1f, 1f, 0, 0, pageHeight);
                            break;
                        case 180:
                            background.AddTemplate(page, -1f, 0, 0, -1f, pageWidth, pageHeight);
                            break;
                        case 270:
                            background.AddTemplate(page, 0, 1f, -1f, 0, pageWidth, 0);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = "Error: " + ex.Message;
            }
            finally
            {
                if (stamper != null) stamper.Close();
                if (overReader != null) overReader.Dispose();
                if (baseReader != null) baseReader.Dispose();
            }

            if (!File.Exists(outputFilePath))
            {
                result.Error = "Output file not found.";
            }

            return result;
        }

    }
}
