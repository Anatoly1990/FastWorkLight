using System.IO;
using System.Windows.Forms;

namespace FastWorkLight.Models
{
    public class TXT
    {
        public TXT() { }
        public void Create(RichTextBox r1)
        {
            Stream file;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = saveFileDialog1.OpenFile();
                if (file != null)
                {
                    file.Close();
                }
                using (StreamWriter streamWriter = new StreamWriter(file))
                {
                    streamWriter.WriteLine($"{r1.Text}");
                }

            }
            r1.Text = "";
        }
    }
}
