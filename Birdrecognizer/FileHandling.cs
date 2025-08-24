using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ookii.Dialogs.Wpf; 


namespace Birdrecognizer
{
    static internal class FileHandling
    {

        public static void createNewBird(string imagepath, string birdname)
        {
            /* This method creates a new folder for the bird and copies the image to that folder.
             * The image is then copied to the new folder with the name of the bird.
             * The folder is created in the current directory for birds.
             * The birdfolder is then used to retrain the model with the new bird.
             */
            try
            {
                birdname = birdname.Trim().ToLower();
                string birdFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "Birds", birdname);
                if (!System.IO.Directory.Exists(birdFolder))
                {
                    System.IO.Directory.CreateDirectory(birdFolder);

                    string extension = System.IO.Path.GetExtension(imagepath);
                    string newImageName = $"{birdname} 01{extension}";
                    string newImagePath = System.IO.Path.Combine(birdFolder, newImageName);

                    System.IO.File.Copy(imagepath, newImagePath, true);
                }
                else
                {
                    var files = System.IO.Directory.GetFiles(birdFolder);
                    int maxImageNumber = 0;
                    foreach (var file in files)
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file).ToLower();
                        if (fileName.StartsWith(birdname))
                        {
                            string[] parts = fileName.Split(' ');
                            if (parts.Length > 1 && int.TryParse(parts[2], out int imageNumber))
                            {
                                if (imageNumber > maxImageNumber)
                                {
                                    maxImageNumber = imageNumber;
                                }
                            }
                        }
                    }

                    maxImageNumber++;
                    string extension = System.IO.Path.GetExtension(imagepath);
                    string newImageName = $"{birdname} {maxImageNumber:D2}{extension}";
                    string newImagePath = System.IO.Path.Combine(birdFolder, newImageName);
                    
                    System.IO.File.Copy(imagepath, newImagePath, true);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating bird folder: {ex.Message}","Filfeil", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public static void addImageToBird(string imagepath) //Denne må gjøres om fikk ikke kopiert bildet til mappen.
        {
            /* This method adds an image to an existing bird folder.
             * User chooses the bird folder from the birds folder.
             * The image is then copied to the correct bird folder with the name of the bird and next sequence number.
             */
            try
            {
                string systemfolder = System.IO.Path.Combine(Environment.CurrentDirectory, "Birds");
                string birdFolder = string.Empty;

                var dialog = new VistaFolderBrowserDialog
                {
                    Description = "Select the bird folder to add the image to",
                    UseDescriptionForTitle = true,
                    SelectedPath = systemfolder
                };
                if (dialog.ShowDialog() == true)
                {
                    birdFolder = dialog.SelectedPath;
                }

                var files = System.IO.Directory.GetFiles(birdFolder);
                string birdName = System.IO.Path.GetFileName(birdFolder.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar)).ToLower();
                int maxImageNumber = 0;

                foreach (var file in files)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension(file).ToLower();
                    if (filename.StartsWith(birdName))
                    {
                        string[] parts = filename.Split(' ');
                        if (parts.Length > 1 && int.TryParse(parts[2], out int imageNumber))
                        {
                            if (imageNumber > maxImageNumber)
                            {
                                maxImageNumber = imageNumber;
                            }
                        }
                    }
                }

                maxImageNumber++;
                string extension = System.IO.Path.GetExtension(imagepath);
                string newImageName = $"{birdName} {maxImageNumber:D2}{extension}";
                string newImagePath = System.IO.Path.Combine(birdFolder, newImageName);

                System.IO.File.Copy(imagepath, newImagePath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding image to bird folder: {ex.Message}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
