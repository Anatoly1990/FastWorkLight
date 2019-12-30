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
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace FastWorkLight
{
    public partial class Form1 : Form
    {
        string urlAddress;
        int valueList;
        public Form1()
        {
            InitializeComponent();          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "hh.ru":
                    StartDriverHH(comboBox1, textBox1, textBox2);
                    GetHtmlAsync(urlAddress, richTextBox1, progressBar1);
                    break;
                case "gorodrabot.ru":
                    StartDriverGR(comboBox1, textBox1, textBox2);
                    break;
                
                case "superjob.ru":
                    StartDriveSJ(comboBox1, textBox1, textBox2);
                    break;

                default:
                    break;
            }     
            //GetHtmlAsync(comboBox1);

        }
        private static void StartDriveSJ(ComboBox url, TextBox work, TextBox city)
        {
            IWebDriver webDriver = new ChromeDriver();           
            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl($"https://{url.Text}");
            IWebElement elementInput = webDriver.FindElement(By.CssSelector("input[name='keywords']"));
            elementInput.SendKeys($"{work.Text}");

            IWebElement elementButton = webDriver.FindElement(By.CssSelector("span[class='_3mfro _3JLhD _1hP6a _2JVkc _2VHxz']"));
            elementButton.Click();

            IWebElement elementInputCity = webDriver.FindElement(By.CssSelector("input[name='geo']"));
            elementInputCity.SendKeys($"{city.Text}");

            for (int i = 0; i < 2; i++)
            {
                IWebElement elementBox = webDriver.FindElement(By.CssSelector("span[class='_3mfro _1hP6a _2JVkc _2VHxz']"));
                elementBox.Click();
            }
            //SendKeys(OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(20000);

            IWebElement elementBCity = webDriver.FindElement(By.CssSelector("span[class='qTHqo _2h9me _2f9Qn WUxjs _3DcL4']"));
            elementBCity.Click();           

            Thread.Sleep(5000);
            //webDriver.Quit();

        }

        private string StartDriverHH(ComboBox url, TextBox work, TextBox city)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl($"https://{url.Text}");            

            IWebElement elementCity = driver.FindElement(By.CssSelector("button[data-qa='mainmenu_areaSwitcher']"));
            elementCity.Click();
            Thread.Sleep(2000);
            IWebElement elementInputCity = driver.FindElement(By.CssSelector("input[type='text']"));
            elementInputCity.SendKeys($"{city.Text}" + OpenQA.Selenium.Keys.Enter);

            Thread.Sleep(1500);
            IWebElement buttonCity = driver.FindElement(By.CssSelector("li[class='suggest__item suggest__item_delimiter_line Bloko-Suggest-Item']"));
            buttonCity.Click();

            IWebElement elementInput = driver
                .FindElement(By.CssSelector("input[data-qa='search-input']"));
            elementInput.SendKeys($"{work.Text}"+OpenQA.Selenium.Keys.Enter);

            //find last list
            IWebElement link = driver
                .FindElements(By.CssSelector("a[class='bloko-button HH-Pager-Control']")).Last();
            valueList = Convert.ToInt32(link.GetAttribute("data-page"));

            urlAddress = driver.Url;
            driver.Quit();
            return urlAddress;        

        }

        private static void StartDriverGR(ComboBox url, TextBox work, TextBox city)
        {
            
        
            using (var driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl($"https://{url.Text}");                             
                driver.FindElementById("query").SendKeys($"{work.Text}");
                driver.FindElementById("location").Click();
                driver.FindElementById("location").Clear();               
                Thread.Sleep(5000);
                driver.FindElementById("location").SendKeys($"{city.Text}");
                driver.FindElementByClassName("btn-default").Click();

                driver.FindElementByName("q").SendKeys($"{work.Text}");
                driver.FindElementById("expanded_location_search").SendKeys($"{city.Text}");
                
                //driver.FindElementsByXPath($"//button[.='{city.Text}']");


                //button[.='Log In']

                //lnk pull-right-xs search-section-locale__city

                Thread.Sleep(1200000);

                //return url; 

            }
            
           
        }

        private static async void GetHtmlAsync(string url, RichTextBox item, ProgressBar bar)
        {
            int index = 1;
            string end = "&page=0";
            HttpClient httpClient = new HttpClient();
            var htmlWrite = await httpClient.GetStringAsync(url + end);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlWrite);

            var ItemList = doc.DocumentNode.Descendants("div")
                .Where(x => x.GetAttributeValue("data-qa", "").Equals("vacancy-serp__results")).ToList();

            var WorkList = ItemList[0].Descendants("div")
                .Where(x => x.GetAttributeValue("data-qa", "").Equals("vacancy-serp__vacancy")).ToList();
             bar.Maximum= WorkList.Count();
            new Thread(async () =>
            {
                foreach (var prod in WorkList)
                {
                    var entity = prod.Descendants("a")
                        .Where(x => x.GetAttributeValue("class", "").Equals("bloko-link HH-LinkModifier")).
                        FirstOrDefault().InnerText.Trim();

                    string pay = "з/п не указана";
                    try
                    {
                        if (prod.Descendants("div")
                            .Where(x => x.GetAttributeValue("class", "").Equals("vacancy-serp-item__compensation"))
                            .FirstOrDefault().InnerText.Trim() != null)
                        {
                            pay = prod.Descendants("div")
                                .Where(x => x.GetAttributeValue("class", "").Equals("vacancy-serp-item__compensation")).
                                FirstOrDefault().InnerText.Trim();
                        }
                    }
                    catch (NullReferenceException e) { }
                    var manage = prod.Descendants("a")
                        .Where(x => x.GetAttributeValue("data-qa", "").Equals("vacancy-serp__vacancy-employer"))
                        .FirstOrDefault().InnerText.Trim();
                    item.Text += $"{index}.  {entity} :    {manage}    -    {pay}\n";
                    bar.Value = index;
                    index++;
                }
            }).Start();
        }
            

    }
}
