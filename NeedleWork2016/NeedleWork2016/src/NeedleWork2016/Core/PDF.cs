using Aspose.Pdf.Generator;
using Aspose.Pdf;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services;
using iTextSharp.text;

namespace NeedleWork2016.Core
{
    public class PDF 
    {

        private  PdfPTable table;
        private  iTextSharp.text.Image image;
        private PdfPCell cell;
    
        /// <summary>
        /// This metod create table in the pdf file
        /// </summary>
        public  void CreateTable()
        {
            table = new PdfPTable(1);

            table.WidthPercentage = 100;

            table.SetWidths(new float[] { 812 });

        }
        /// <summary>
        /// This method return table for the pdf report
        /// </summary>
        /// <returns></returns>
        public  PdfPTable GetTable()
        {
            return table;

        }
        /// <summary>
        /// This method create the image on the base of the byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public iTextSharp.text.Image GetImage(byte[] data)
        {

            image = iTextSharp.text.Image.GetInstance(byteArrayToImage1(data), System.Drawing.Imaging.ImageFormat.Jpeg);

            image.ScalePercent(200);

            return image;

        }
        /// <summary>
        /// This method create  a cell in the pdf file
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public  PdfPCell CreateCell(iTextSharp.text.Image image)
        {

            cell = new PdfPCell(image);

            cell.PaddingTop = 10;

            cell.PaddingRight = 0;

            cell.PaddingBottom = 10;

            cell.PaddingLeft = 15;

            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            cell.Border = 0;

            return cell;

        }
        /// <summary>
        /// This method consist settings of pdf file
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="doc"></param>
        public  void Settings(MemoryStream ms, iTextSharp.text.Document doc)
        {
            PdfWriter.GetInstance(doc, ms);
            iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4);

            doc.SetPageSize(rec);

            doc.SetMargins(0, 0, 0, 0);

            doc.Open();

            doc.NewPage();

        }
        /// <summary>
        /// This method converts image on the base of the byte array
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public  System.Drawing.Image byteArrayToImage1(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);

            return System.Drawing.Image.FromStream(ms);
        }
        /// <summary>
        /// This method converts base64 string to the byte array
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public byte[] StringToCode(string imageData)
        {
            byte[] data = Convert.FromBase64String(imageData);

            return data;
        }
    }
}
