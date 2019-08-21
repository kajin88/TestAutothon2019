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

        public AutomationFacade(IWebDriver _driver, int timeOutInSeconds = 30)
        {
            this.driver = _driver;
            this.timeOutInSeconds = timeOutInSeconds;
        }

        public YoutubePage YPage
        {
            get
            {
                if (yPage == null)
                {
                    yPage = new YoutubePage(this.driver);
                }

                return yPage;
            }
        }


        public void GoToChannel()
        {
            this.YPage
                .Navigate()
                .Search();
        }

    }
}
