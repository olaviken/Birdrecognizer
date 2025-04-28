using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birdrecognizer
{
     static internal class BirdStyle
    {
        private const int MinFontSize = 8;
        private const int MaxFontSize = 30;
        private const int FontSizeChange = 2;

        public static int IncreaseFontSize(int GlobalFontSize)
        {
            if (GlobalFontSize < MaxFontSize)
            {
                GlobalFontSize += FontSizeChange;
                return GlobalFontSize;
            }
            return GlobalFontSize;
        }

        public static int DecreaseFontSize(int GlobalFontSize)
        {
            if (GlobalFontSize > MinFontSize)
            {
                GlobalFontSize -= FontSizeChange;
                return GlobalFontSize;
            }
            return GlobalFontSize;
        }



        public static int ResetFontSize()
        {
            return 14;
        }

        /* public async Task SetWebViewFont(WebView2 webView)
         {
             string script = $"document.body.style.fontSize = '{GlobalFontSize}px';";
             await webView.ExecuteScriptAsync(script);
         }*/
    }
}
