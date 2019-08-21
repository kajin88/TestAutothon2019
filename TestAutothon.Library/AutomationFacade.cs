using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutothon.Library.Pages;

namespace TestAutothon.Library
{
    public class AutomationFacade
    {
        private readonly IWebDriver driver;
        private readonly int timeOutInSeconds;
        private YoutubePage yPage;
        private string videoTitle;

        public AutomationFacade(IWebDriver _driver, string _videoTitle, int timeOutInSeconds = 30)
        {
            this.driver = _driver;
            this.timeOutInSeconds = timeOutInSeconds;
            this.videoTitle = _videoTitle;
        }

        public YoutubePage YPage
        {
            get
            {
                if (yPage == null)
                {
                    yPage = new YoutubePage(driver, videoTitle);
                }

                return yPage;
            }
        }


        public void GoToChannel()
        {
            this.YPage
                .Navigate()
                .Search()
                .GoToVideo();

        }

    }
}
