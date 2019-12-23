using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastWorkLight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetHtmlAsync(comboBox1);

        }
        private static async void GetHtmlAsync(ComboBox comboBox)
        {
            var urlParse = comboBox.Text;
            string url = $"{urlParse}";
            HttpClient httpClient = new HttpClient();
            var htmlWrite = await httpClient.GetStringAsync(url);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlWrite);

            var ItemList = doc.DocumentNode.Descendants("div")
                .Where(x => x.GetAttributeValue("class", "").Equals("js-catalog_serp")).ToList();

            var ProductList = ItemList[0].Descendants("div")
                .Where(x => x.GetAttributeValue("class", "").Equals("description item_table-description")).ToList();
            foreach (var prod in ProductList)
            {
                var name = prod.Descendants("h3")
                    .Where(x => x.GetAttributeValue("class", "").Equals("snippet-title")).FirstOrDefault().InnerText.Trim();
                var price = prod.Descendants("div")
                    .Where(x => x.GetAttributeValue("class", "").Equals("about")).FirstOrDefault().InnerText.Trim();
                //var link = prod.Descendants("button").FirstOrDefault().GetAttributeValue("data-item-url", "");


                Console.WriteLine(name + ": " + price + "\n");
            }
        }

    }
}
