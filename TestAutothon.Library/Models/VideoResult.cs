using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAutothon.Library.Models
{
    public class VideoResult
    {
        public string Team { get; set; }

        public string Video { get; set; }

        public List<string> UpcomingVideos { get; set; }
    }
}
