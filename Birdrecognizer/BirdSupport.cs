using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birdrecognizer
{
     static internal class BirdSupport
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

        
        public async static Task<string> CopySelectedText(CoreWebView2 webView)
        {
            if (webView != null)
            {
                return string.Empty;
            }
            else { 
                string javascript = "window.getSelection().toString();";
                string result = await webView.ExecuteScriptAsync(javascript);

                if(string.IsNullOrWhiteSpace(result))
                {
                    return string.Empty;
                }               
                    return result.Trim('"');
            }
        }
    }
}
