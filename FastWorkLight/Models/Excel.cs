using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace FastWorkLight.Models
{
    public class Excel
    {   
        FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        
        DialogResult result;

        public string path;

        public void CreateNewFile(List<Job> date, RichTextBox r1)
        {
            result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                path = folderBrowser.SelectedPath;
                XLWorkbook workbook = new XLWorkbook();
                IXLWorksheet worksheet = workbook.Worksheets.Add("R1");

                int iter = 1;
                foreach (var st in date)
                {
                    worksheet.Cell(iter, 1).SetValue(st.Entity);
                    worksheet.Cell(iter, 2).SetValue(st.Manage);
                    worksheet.Cell(iter, 3).SetValue(st.Pay);
                    iter++;
                }

                MemoryStream MS = new MemoryStream();
                workbook.SaveAs(path+@"\SaveFile.xlsx");
                MS.Position = 0;
                r1.Text = "";
                MessageBox.Show($"Файл: SaveFile.xlsx был сохранен в {path}","Сообщение" , MessageBoxButtons.OK);

            }
            else { return; }
        }

    }
}
