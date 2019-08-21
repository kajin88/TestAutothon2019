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
                        System.Threading.Thread.Sleep(5000);
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

        public IWebElement GoToVideo()
        {
            return VideoElement;
        }

        public YoutubePage ClickOnVideo(IWebElement video)
        {
            if (video != null)
                video.Click();

            System.Threading.Thread.Sleep(5000);

            return this;
        }

        public YoutubePage GoToVideosTab()
        {
            System.Threading.Thread.Sleep(5000);
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

        public YoutubePage SetVideoQuality(string quality = "medium")
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"document.getElementById('movie_player').setPlaybackQualityRange('{quality}');");
            return this;
        }

        public List<string> GetUpcomingVideoTitles()
        {
            List<string> upcomingVideoTitles = new List<string>();
            ICollection<IWebElement> upcomingVideoElements = null;
            int oldCount = 0;
            while (true)
            {
                upcomingVideoElements = driver.FindElements(By.XPath("//div[@id='items']//span[@id='video-title']"));
                if (upcomingVideoElements != null && oldCount != upcomingVideoElements.Count)
                {
                    oldCount = upcomingVideoElements.Count;
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,document.scrollingElement.scrollHeight)");
                    System.Threading.Thread.Sleep(3000);
                }
                else
                    break;

            }

            foreach (var element in upcomingVideoElements)
            {
                upcomingVideoTitles.Add(element.GetAttribute("title"));
            }

            return upcomingVideoTitles;
        }
    }
}
