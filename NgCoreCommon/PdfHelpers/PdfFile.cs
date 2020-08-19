using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using Models;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Configuration;
using System.Web;
using iTextSharp.text.html.simpleparser;
using System.Xml.Serialization;
using System.Linq;

namespace NgCore.Common.PdfHelpers
{
   public class PdfFile: PdfPageEventHelper
    {
      public  PdfFile() { }
        public PdfFile(string path) {
            _path = path;
        }
        string _path;
       // public IConfiguration iconfig { get; }
       //private PdfFile(IConfiguration config) {
       //     iconfig = config;
       // }

        public void CreatePdf(string path,string fileName)
        {
            string filePath = path + fileName;
            Document doc = new Document(PageSize.LETTER,10,10,42,35);
           Font mainFont = FontFactory.GetFont("Segoe UI", 22, new BaseColor( Color.Blue));

            List<User> users = new List<User>() { new User {id=1,name="srikanth",mailId="sri.p.com",role="Admin" },new User { id = 2, name = "Anvesh", mailId = "Anvesh.g.com", role = "User" } };

            PdfPTable tbl = new PdfPTable(4);
            PdfPCell tcell =null;

            tcell = new PdfPCell(new Phrase(new Chunk("Id",FontFactory.GetFont("Arial-bold",21))));
            tbl.AddCell(tcell);
            tcell = new PdfPCell(new Phrase("name"));
            tbl.AddCell(tcell);
            tcell = new PdfPCell(new Phrase("Email Id"));
            tbl.AddCell(tcell);
            tcell = new PdfPCell(new Phrase("Role"));
            tbl.AddCell(tcell);

            foreach(var user in users)
            {
                tcell = new PdfPCell(new Phrase(new Chunk(user.id.ToString(), FontFactory.GetFont("Arial-bold", 21))));
                tbl.AddCell(tcell);
                tcell = new PdfPCell(new Phrase(user.name));
                tbl.AddCell(tcell);
                tcell = new PdfPCell(new Phrase(user.mailId));
                tbl.AddCell(tcell);
                tcell = new PdfPCell(new Phrase(user.role));
                tbl.AddCell(tcell);
            }


           PdfWriter writer= PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();
            doc.Add(tbl as IElement);
            doc.Add(new Paragraph("This Is my First Pdf.",mainFont));
            doc.Close();
        }

        public string ReadPdfFile(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);
                var datar = pdfReader.AcroFields.Fields.Select(x => x.Key + ": " + pdfReader.AcroFields.GetField(x.Key)).ToList();
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    List<string> dataarr=new List<string>();
                   

                    dataarr = datar;


                    XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                    using (TextWriter writer = new StreamWriter(_path+"test.xml"))
                    {
                        serializer.Serialize(writer, dataarr);
                    }

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
                pdfReader.Close();
            }
            return text.ToString();
        }
    }
}
