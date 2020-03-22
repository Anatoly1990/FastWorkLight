using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastWorkLight.Models
{
    public class PDF
    {
        FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        DialogResult result;
        public string path;

        public void Create(List<Job> date, TextBox t1, RichTextBox r1)
        {           
            result = folderBrowser.ShowDialog();           

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                path = folderBrowser.SelectedPath;

                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                PdfWriter.GetInstance(doc, new FileStream($"{path}\\WorkList_{t1.Text}.pdf", FileMode.Create));
                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                //Создаем объект таблицы и передаем в нее число столбцов таблицы из нашего датасета
                PdfPTable table = new PdfPTable(3);

                //Добавим в таблицу общий заголовок
                PdfPCell cell = new PdfPCell(new Phrase($"WorkList_from_{t1.Text}.pdf", font));

                cell.Colspan = 3;
                cell.HorizontalAlignment = 1;
                //Убираем границу первой ячейки, чтобы балы как заголовок
                cell.Border = 0;
                table.AddCell(cell);

                //Сначала добавляем заголовки таблицы
                table.AddCell(new Phrase(@"Entity", font));
                table.AddCell(new Phrase(@"Manage", font));
                table.AddCell(new Phrase(@"Pay", font));

                //cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;

                foreach (var item in date)
                {
                    table.AddCell(new Phrase(item.Entity.ToString(), font));
                    table.AddCell(new Phrase(item.Manage.ToString(), font));
                    table.AddCell(new Phrase(item.Pay.ToString(), font));
                }
                //Добавляем таблицу в документ
                doc.Add(table);
                //Закрываем документ
                doc.Close();

                r1.Text = "";
                MessageBox.Show($"Файл: {path}\\WorkList_{ t1.Text}.pdf успешно записан","Сообщение",MessageBoxButtons.OK);

                Process.Start($"{path}\\WorkList_{ t1.Text}.pdf");
            }
            

        }
    }
}
