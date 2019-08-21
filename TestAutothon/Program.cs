using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutothon.Library;
using TestAutothon.Library.Models.Enums;

namespace TestAutothon
{
    class Program
    {
        static void Main(string[] args)
        {
            //if(args != null && args.Length > 0)
            //{
            //    foreach(var arg in args)
            //    {

            //    }
            //}

            string api = "http://54.169.34.162:5252/video";
            string resultFolder = AutomationUtility.GetOutputDirectory();

            AutomationHelper helper = new AutomationHelper();
            helper.Run(api, AutomationBrowserType.PCChromeBrowser, resultFolder);
        }

        private static void ParseArgument()
        {

        }
    }
}
