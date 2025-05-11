using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;

namespace Birdrecognizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int GlobalFontSize = 14;
        private int delay = 1000;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void openFileWBird_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusText.Text = "Oppgi bildefil";
                statusProgress.Value = 10;

                var result = new BirdrecognizerAI.ModelOutput();
                string path = OpenFile.getFilePath();

                statusText.Text = "Henter bilde av en fugl";
                statusProgress.Value = 20;
                await Task.Delay(delay);

                if (string.IsNullOrEmpty(path)) return;
                
                imagePath.Text = path;
                birdImage.Source = new BitmapImage(new Uri(path));

                statusText.Text = "Analyserer bilde";
                statusProgress.Value = 40;
                await Task.Delay(delay);

                BirdResult birdResult = await BirdResult.AnalyzeImage(path);
                
                statusText.Text = "Henter Wikipedia artikler";
                statusProgress.Value = 80;
                await Task.Delay(delay);

                EngWikiBrowser.Source = Wikipedia.getEngWiki(birdResult.BirdName);
                NorWikiBrowser.Source = Wikipedia.getNoWiki(birdResult.BirdName);
                
                recognizedBird.Text = Wikipedia.GetSummary(birdResult.BirdName, birdResult.Score);
                
                statusText.Text = "Ferdig!";
                statusProgress.Value = 0;

                
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Feil: Filen ble ikke funnet. Sjekk filbanen.", "FilFeil", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"En feil oppstod: {ex.Message}", "Ukjent feil", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void IncreaseFontsize_Click(object sender, RoutedEventArgs e)
        {

            GlobalFontSize = BirdSupport.IncreaseFontSize(GlobalFontSize);
            recognizedBird.FontSize = GlobalFontSize;

            if (EngWikiBrowser.CoreWebView2 != null)
            {
                EngWikiBrowser.CoreWebView2.ExecuteScriptAsync("document.body.style.zoom = parseFloat(document.body.style.zoom || 1) + 0.1;");
            }

            if (NorWikiBrowser.CoreWebView2 != null)
            {
                NorWikiBrowser.CoreWebView2.ExecuteScriptAsync("document.body.style.zoom = parseFloat(document.body.style.zoom || 1) + 0.1;");
            }
        }

        private void DencreaseFontsize_Click(object sender, RoutedEventArgs e)
        {

            GlobalFontSize = BirdSupport.DecreaseFontSize(GlobalFontSize);
            recognizedBird.FontSize = GlobalFontSize;

            if (EngWikiBrowser.CoreWebView2 != null)
            {
                EngWikiBrowser.CoreWebView2.ExecuteScriptAsync("document.body.style.zoom = parseFloat(document.body.style.zoom || 1) - 0.1;");
            }

            if (NorWikiBrowser.CoreWebView2 != null)
            {
                NorWikiBrowser.CoreWebView2.ExecuteScriptAsync("document.body.style.zoom = parseFloat(document.body.style.zoom || 1) - 0.1;");
            }
        }

        private async void CopyText_Click(object sender, RoutedEventArgs e)
        {
            string norText = await BirdSupport.CopySelectedText(NorWikiBrowser.CoreWebView2);
            string engText = await BirdSupport.CopySelectedText(EngWikiBrowser.CoreWebView2);

            string copiedText = string.Empty;
            if (!string.IsNullOrWhiteSpace(norText))
            {
                copiedText = norText;
            }
            else if (!string.IsNullOrWhiteSpace(engText))
            {
                copiedText = engText;
            }
            if (!string.IsNullOrWhiteSpace(copiedText))
            {
                Clipboard.SetText(copiedText);
                MessageBox.Show("Teksten er kopiert til utklippstavlen.", "Kopier tekst", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ingen tekst ble valgt.", "Ingen tekst", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}