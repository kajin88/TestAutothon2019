using OpenQA.Selenium;
using TestAutothon.Library.Models;
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


        public VideoData GoToChannel(string outputDirectory)
        {
            VideoData data = new VideoData()
            {
                Team = "Siemens Team 2",
                Video = videoTitle
            };

            var element = this.YPage
                .Navigate()
                .Search()
                .NavigateToChannel()
                .GoToVideosTab()
                .GoToVideo();

            data.ScreenshotPath = $"{outputDirectory}\\{AutomationUtility.ExcludeSymbols(videoTitle)}.jpg";
            this.YPage.GetScreenshot(data.ScreenshotPath);

            this.yPage.ClickOnVideo(element);

            data.UpcomingVideos = this.yPage
                                    .SetVideoQuality()
                                    .GetUpcomingVideoTitles();

            return data;
        }

    }
}
