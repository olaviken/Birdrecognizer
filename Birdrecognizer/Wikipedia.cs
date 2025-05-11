using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birdrecognizer
{
    static internal class Wikipedia
    {
        public static Uri getEngWiki(string wikipediaId)
        {
            wikipediaId = wikipediaId.ToLower();
            string temp = wikipediaId.Replace(" ", "_");
            string url = $"https://en.wikipedia.org/wiki/{temp}";

            Uri engUri = new Uri(url);
            return engUri;
        }

        public static Uri getNoWiki(string wikipediaId)
        {
            wikipediaId = wikipediaId.ToLower();
            string temp = wikipediaId.Replace(" ", "_");
            string url = $"https://no.wikipedia.org/wiki/{temp}";
            Uri noUri = new Uri(url);

            return noUri;
        }

       
        public static string GetSummary(string predictedLabel, float score)
        {
            return $"Denne modellen gjenkjenner følgende fugl: {predictedLabel}. \nMed sannsynlighet på {Math.Round(score * 100, 2)}%. \nPå de neste fanene kan du lese hva som står på wikipedia om denne fuglen.";
        }

    }
}
