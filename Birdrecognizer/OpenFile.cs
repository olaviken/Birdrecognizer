using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birdrecognizer
{
     static internal class OpenFile
    {
        /*
         * This class is meant for containing methods for opening and saving files.         
         * This can be extended for future use with different file types.
         */
        public static string getFilePathImage()
        {
            string path = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            openFile.Multiselect = false;
            openFile.Title = "Select an image of a bird";
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.InitialDirectory = Environment.CurrentDirectory;

            if (openFile.ShowDialog() == true)
            {
                path = openFile.FileName;
            }

            if (path == string.Empty)
            {
                return string.Empty;
            }
            else
            {
                return path;
            }
        }
    }
}
