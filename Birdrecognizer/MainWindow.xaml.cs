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

                var result = new Birdrecognizer.ModelOutput();
                string path = openFile.getFilePath();

                statusText.Text = "Henter bilde av en fugl";
                statusProgress.Value = 20;
                await Task.Delay(delay);

                if (path == string.Empty)
                {
                    return;
                }
                else
                {

                    imagePath.Text = path;
                    birdImage.Source = new BitmapImage(new Uri(path));
                    var inputImage = new Birdrecognizer.ModelInput()
                    {
                        ImageSource = System.IO.File.ReadAllBytes(path)
                    };

                    statusText.Text = "Analyserer bilde";
                    statusProgress.Value = 40;
                    await Task.Delay(delay);

                    result = Birdrecognizer.PredictEngine.Value.Predict(inputImage);

                    statusText.Text = "Henter Wikipedia artikler";
                    statusProgress.Value = 80;
                    await Task.Delay(delay);

                    Uri engUri = new Uri(Wikipedia.getEngWiki(result.PredictedLabel));
                    Uri noUri = new Uri(Wikipedia.getNoWiki(result.PredictedLabel));
                    EngWikiBrowser.Source = engUri;
                    NorWikiBrowser.Source = noUri;

                    recognizedBird.Text = $"Denne modellen gjenkjenner følgende fugl: {result.PredictedLabel}. \nMed {float.Round(100 * result.Score[3], 2)}% sannsynlighet \nPå de neste fanene kan du lese hva som står på wikipedia om denne fuglen.";

                    statusText.Text = "Ferdig!";
                    statusProgress.Value = 0;

                }
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

            GlobalFontSize = BirdStyle.IncreaseFontSize(GlobalFontSize);
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

            GlobalFontSize = BirdStyle.DecreaseFontSize(GlobalFontSize);
            recognizedBird.FontSize = GlobalFontSize;
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            /*Ikke ferdig med denne. Trenger at funksjonen skiller mellom eng og nor side.*/
            NorWikiBrowser.CoreWebView2.ExecuteScriptAsync("document.execCommand('copy');");
            EngWikiBrowser.CoreWebView2.ExecuteScriptAsync("document.execCommand('copy');");
        }
    }
}