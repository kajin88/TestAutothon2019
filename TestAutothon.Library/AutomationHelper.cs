using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutothon.Library.Models.Enums;

namespace TestAutothon.Library
{
    public class AutomationHelper
    {
        public void Run(string url, AutomationBrowserType browserType)
        {
            string title = AutomationUtility.GetVideoTitleFromAPI(url);

            var youtubeDriver = new AutomationDriver();
            youtubeDriver.StartBrowser(browserType, 3);

            AutomationFacade facade = new AutomationFacade(youtubeDriver.Browser);
            facade.GoToChannel();
        }
    }
}
