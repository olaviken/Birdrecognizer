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
using Microsoft.ML;
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

        //Ai model
        private ITransformer model;
        private MLContext mlContext = new MLContext();


        public MainWindow()
        {
            InitializeComponent();

            //Load Ai model if it exists
            string modelName = "BirdrecognizerAI.zip";
            string baselineModelName = "BirdrecognizerAI.mlnet";

            if (File.Exists(modelName))
            {
                using var stream = File.OpenRead(modelName);
                model = mlContext.Model.Load(stream, out var modelInputSchema);
            }
            else if (File.Exists(baselineModelName))
            {
                using var stream = File.OpenRead(baselineModelName);
                model = mlContext.Model.Load(stream, out var modelInputSchema);
            }
            else
            {
                MessageBox.Show("Modellfilen ble ikke funnet. Vennligst last ned modellen.", "Modellfeil", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void openFileWBird_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusText.Text = "Oppgi bildefil";
                statusProgress.Value = 10;

                var result = new BirdrecognizerAI.ModelOutput();
                string path = OpenFile.getFilePathImage();


                statusText.Text = "Henter bilde av en fugl";
                statusProgress.Value = 20;


                await Task.Delay(delay);

                if (string.IsNullOrEmpty(path))
                {
                    statusProgress.Value = 0;
                    statusText.Text = "Status: Ingen bilde valgt";
                    return;
                }

                imagePath.Text = path;
                birdImage.Source = new BitmapImage(new Uri(path));

                statusText.Text = "Analyserer bilde";
                statusProgress.Value = 40;
                await Task.Delay(delay);



                BirdResult birdResult = await BirdResult.AnalyzeImage(path, model, mlContext);


                statusText.Text = "Henter Wikipedia artikler";
                statusProgress.Value = 80;
                await Task.Delay(delay);


                EngWikiBrowser.Source = Wikipedia.getEngWiki(birdResult.BirdName);
                NorWikiBrowser.Source = Wikipedia.getNoWiki(birdResult.BirdName);

                recognizedBird.Text = Wikipedia.GetSummary(birdResult.BirdName, birdResult.Score);


                statusText.Text = "Ferdig!";
                statusProgress.Value = 0;

                // Vis UI elementer for bekreftelse av fugl
                correctBird.Visibility = Visibility.Visible;
                correctBirdYes.Visibility = Visibility.Visible;
                correctBirdNo.Visibility = Visibility.Visible;

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

        /*
         * The next methods below is supposed to create a new AI model that is trained with full set of images and added new bird and photo. However this is a time consuming effort so the GUI for these method is hidden. 
         * These methods have been tested but took 2-4 hours. Therefore they are not included in the main GUI.
         * */


        private void correctBirdYes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Takk for at du bekreftet at dette er riktig fugl!", "Bekreftelse", MessageBoxButton.OK, MessageBoxImage.Information);

            correctBirdNo.Visibility = Visibility.Hidden;
        }

        private void correctBirdNo_Click(object sender, RoutedEventArgs e)
        {
            newBird.Visibility = Visibility.Visible;
            newBirdYes.Visibility = Visibility.Visible;
            newBirdNo.Visibility = Visibility.Visible;

            correctBirdYes.Visibility = Visibility.Hidden;
        }

        private void newBirdYes_Click(object sender, RoutedEventArgs e)
        {
            newBirdName.Visibility = Visibility.Visible;
            newBirdNameTextBox.Visibility = Visibility.Visible;
            newBirdNameButton.Visibility = Visibility.Visible;

            newBirdNo.Visibility = Visibility.Hidden;
        }

        private void newBirdNo_Click(object sender, RoutedEventArgs e)
        {
            addPictureToExistingBird.Visibility = Visibility.Visible;
            addPictureToExistingBirdYes.Visibility = Visibility.Visible;
            addPictureToExistingBirdNo.Visibility = Visibility.Visible;

            newBirdYes.Visibility = Visibility.Hidden;
        }

        private async void newBirdNameButton_Click(object sender, RoutedEventArgs e)
        {
            string image = imagePath.Text;
            string birdName = newBirdNameTextBox.Text.Trim();

            //Add status bar and status text
            statusText.Text = "Oppretter mappe med ny fugl";
            statusProgress.Value = 10;
            await Task.Delay(delay);

            try
            {
                if (string.IsNullOrEmpty(image) || !File.Exists(image))
                {
                    throw new FileNotFoundException("Bilde filen ble ikke funnet. Vennligst velg et gyldig bilde.");
                }

                if (string.IsNullOrEmpty(birdName))
                {
                    throw new ArgumentException("Fuglenavn kan ikke være tomt.");
                }
                FileHandling.createNewBird(image, birdName);

                //Retrain the model with the new bird

                statusText.Text = "henter mappe med fugler";
                statusProgress.Value = 20;
                await Task.Delay(delay);

                string birdFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "Birds");
                var mlContext = new Microsoft.ML.MLContext();

                statusText.Text = "Trener modellen med ny data";
                statusProgress.Value = 30;
                await Task.Delay(delay);

                var trainData = BirdrecognizerAI.LoadImageFromFolder(mlContext, birdFolder);
                var model = BirdrecognizerAI.RetrainModel(mlContext, trainData);

                statusText.Text = "Lagrer den nye modellen";
                statusProgress.Value = 70;
                await Task.Delay(delay);

                mlContext.Model.Save(model, trainData.Schema, "BirdrecognizerAI.zip");
                                
                // Reset UI elements after adding new bird
                newBirdName.Visibility = Visibility.Hidden;
                newBirdNameTextBox.Visibility = Visibility.Hidden;
                newBirdNameButton.Visibility = Visibility.Hidden;
                addPictureToExistingBird.Visibility = Visibility.Hidden;
                addPictureToExistingBirdYes.Visibility = Visibility.Hidden;
                addPictureToExistingBirdNo.Visibility = Visibility.Hidden;
                newBird.Visibility = Visibility.Hidden;
                newBirdYes.Visibility = Visibility.Hidden;
                newBirdNo.Visibility = Visibility.Hidden;
                correctBirdYes.Visibility = Visibility.Visible;
                correctBirdNo.Visibility = Visibility.Visible;

                statusText.Text = "Modellen er lagret!";
                statusProgress.Value = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"En feil oppstod: {ex.Message}", "Feil", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

       

        private void addPictureToExistingBirdYes_Click(object sender, RoutedEventArgs e)
        {
            // This method should handle adding a picture to an existing bird.
            FileHandling.addImageToBird(imagePath.Text);

            //Retraining the model with the new bird picture

            string birdFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "Birds");
            var mlContext = new Microsoft.ML.MLContext();
            var trainData = BirdrecognizerAI.LoadImageFromFolder(mlContext, birdFolder);
            var model = BirdrecognizerAI.RetrainModel(mlContext, trainData);

            mlContext.Model.Save(model, trainData.Schema, "BirdrecognizerAI.zip");

            // Reset UI elements after confirming to add picture to existing bird
            addPictureToExistingBird.Visibility = Visibility.Hidden;
            addPictureToExistingBirdYes.Visibility = Visibility.Hidden;
            addPictureToExistingBirdNo.Visibility = Visibility.Hidden;
            correctBirdYes.Visibility = Visibility.Visible;
            correctBirdNo.Visibility = Visibility.Visible;
            correctBird.Visibility = Visibility.Visible;

        }

        private void addPictureToExistingBirdNo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Du har valgt å ikke legge til bildet til en eksisterende fugl.", "Ingen bilde lagt til", MessageBoxButton.OK, MessageBoxImage.Information);
            // Reset UI elements after declining to add picture to existing bird
            addPictureToExistingBird.Visibility = Visibility.Hidden;
            addPictureToExistingBirdYes.Visibility = Visibility.Hidden;
            addPictureToExistingBirdNo.Visibility = Visibility.Hidden;
            correctBirdYes.Visibility = Visibility.Visible;
            correctBirdNo.Visibility = Visibility.Visible;
            correctBird.Visibility = Visibility.Visible;

        }

        /* End of methods related to training on new data*/
        
    }
}