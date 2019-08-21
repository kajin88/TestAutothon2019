using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutothon.Library.Models;
using TestAutothon.Library.Models.Enums;

namespace TestAutothon.Library
{
    public class AutomationHelper
    {
        public void Run(string url, AutomationBrowserType browserType, string outputDirectory)
        {
            string title = AutomationUtility.GetFromAPI(url + "/video");

            var youtubeDriver = new AutomationDriver();
            youtubeDriver.StartBrowser(browserType, 3);

            AutomationFacade facade = new AutomationFacade(youtubeDriver.Browser, title);
            var data = facade.GoToChannel(outputDirectory);

            string jsonPath = $"{outputDirectory}\\{AutomationUtility.ExcludeSymbols(title)}.json";
            AutomationUtility.Serialize<VideoData>(data, jsonPath);
            var result = AutomationUtility.UploadFile(url + "/upload", jsonPath);

            var validatedResult = AutomationUtility.GetFromAPI(url + "/result/" + result);

            youtubeDriver.StopBrowser();
        }
    }
}
