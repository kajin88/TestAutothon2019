using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAutothon.Library.Pages
{
    public class YoutubePage
    {
        private readonly IWebDriver driver;
        private readonly string url = @"https://www.youtube.com";
        private readonly string searchKeyword = "step-inforum";


        public YoutubePage(IWebDriver _driver)
        {
            this.driver = _driver;
        }


        public IWebElement SearchBox
        {
            get
            {
                return this.driver.FindElement(By.XPath("//INPUT[@id='search']"));
            }
        }

        public IWebElement ChannelElement
        {
            get
            {
                return this.driver.FindElement(By.XPath("//SPAN[@class='style-scope ytd-channel-renderer'][text()='STeP-IN Forum']"));
            }
        }


        public YoutubePage Navigate()
        {
            this.driver.Navigate().GoToUrl(this.url);
            this.driver.WaitForPageLoad();
            return this;
        }

        public YoutubePage Search()
        {
            this.SearchBox.Clear();
            this.SearchBox.SendKeys(this.searchKeyword);
            this.SearchBox.Submit();
            return this;
        }

        public void GetScreenshot(string path)
        {
            ITakesScreenshot screenshot = (ITakesScreenshot)this.driver;
            screenshot.GetScreenshot().SaveAsFile(path);
        }
    }
}
