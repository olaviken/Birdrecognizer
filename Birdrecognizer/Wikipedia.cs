using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birdrecognizer
{
    static internal class Wikipedia
    {
        public static string getEngWiki(string wikipediaId)
        {
            wikipediaId = wikipediaId.ToLower();
            string temp = wikipediaId.Replace(" ", "_");
            string url = $"https://en.wikipedia.org/wiki/{temp}";

            return url;
        }

        public static string getNoWiki(string wikipediaId)
        {
            wikipediaId = wikipediaId.ToLower();
            string temp = wikipediaId.Replace(" ", "_");
            string url = $"https://no.wikipedia.org/wiki/{temp}";

            return url;
        }

    }
}
