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
        private readonly string videoTitle;

        private IWebElement _videoElement;
        private IWebElement _videosTabElement;
        private bool _videoExists = true;

        public YoutubePage(IWebDriver _driver, string _videoTitle)
        {
            this.driver = _driver;
            this.videoTitle = _videoTitle;
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
                return this.driver.FindElement(By.XPath("//SPAN[@class='style-scope ytd-channel-renderer'][text()='STeP-IN Forum']"), 180);
            }
        }

        public IWebElement VideoElement
        {
            get
            {
                if (_videoElement == null && _videoExists)
                {
                    try
                    {
                        _videoElement = FindVideo(videoTitle);
                    }
                    catch(Exception)
                    {
                        _videoExists = false;
                        _videoElement = null;                        
                    }
                }

                return _videoElement;
            }
        }


        public IWebElement VideosTabElement
        {
            get
            {
                if(_videosTabElement == null )
                {
                    _videosTabElement = GetVideosTab();
                }

                return _videosTabElement;
            }
        }

        public YoutubePage Navigate()
        {
            this.driver.Navigate().GoToUrl(this.url);
            this.driver.WaitForPageLoad();
            return this;
        }

        public YoutubePage NavigateToChannel()
        {
            this.ChannelElement.Click();
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

        private IWebElement FindVideo(string videoTitle)
        {
            IWebElement foundElement = null;

            try
            {
                while (true)
                {
                    try
                    {
                        foundElement = driver.FindElement(By.XPath($"//a[contains(@id,'video-title') and contains(@title,'{videoTitle}')]"));
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,500)");
                    }
                }

                if (foundElement != null)
                {
                    String scrollElementIntoMiddle = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                                                + "var elementTop = arguments[0].getBoundingClientRect().top;"
                                                + "window.scrollBy(0, elementTop-(viewPortHeight/2));";
                    ((IJavaScriptExecutor)driver).ExecuteScript(scrollElementIntoMiddle, foundElement);
                }

                return foundElement;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public YoutubePage GoToVideo()
        {
            if (VideoElement != null)
                VideoElement.Click();
            return this;
        }

        public YoutubePage GoToVideosTab()
        {
            if(VideosTabElement != null)
                VideosTabElement.Click();
            return this;
        }

        private IWebElement GetVideosTab()
        {

            var elements = this.driver.FindElements(By.XPath("//div[contains(@class, 'tab-content style-scope paper-tab')]"));
            if(elements != null && elements.Any())
            {
                foreach(var ele in elements)
                {
                    if(!string.IsNullOrEmpty(ele.Text) && ele.Text.Equals("Videos", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ele;
                    }
                }
            }

            return null;
        }
    }
}
