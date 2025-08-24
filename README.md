# Birdrecognizer
This project is to test ML.Net and the image classifier in this framework. It is trained on 14 different races of birds living in Norway. For each race of bird it is used 10 images of that bird. The files are organized into a folder where each bird has its own folder containing the images for respective birds.

## Missing file due to size restriction
The file for BirdrecognizerAI.mlnet is 94 MB large avoid uploading it for now. However, it should be possible to recreate in Visual Studio adding a ML.Net class. Then using Image classification. 

<img width="1431" height="714" alt="image" src="https://github.com/user-attachments/assets/148861ec-a1b5-427f-8fb5-00819dbfe5d0" />

The folder which contains all the birds images is used as training data. 

## Graphic user interface (GUI)
The graphic user interface is divided into menu and main user interface. Menu has different controls and where the user chooses which photo to check. The main window is divided between a large image area and a information area. Information area is again divided up using tabs. 
It is divided into three different tabs, one for informing the user which bird the system assume is on the image the user has chosen, and two tabs linked to wikiepedia.org showing pages for the birds. For norwegian and english page for the assumed bird.

## How it works
The user opens a chosen image file with a bird. The system will then analyze this image to see if it is similar to the birds it has beeen trained on. As an output it will give the label of the most likely bird and probability of it being this bird. 
Since this project has used the scientific name when naming the birds it is an easy task sending a search request to wikipedia.org. We are using the following adresses for wikipedia. eng: https://en.wikipedia.org/wiki/ and nor: https://no.wikipedia.org/wiki/. These search result is then showed in the respective tabs for wikipedia.

## Technical aspect

### Dependencies
This project use following framework and external libraries:
1. Microsoft.ML ([https://learn.microsoft.com/nb-no/dotnet/machine-learning/](https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet))
2. Microsoft.ML.Vision (https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet)
3. Microsoft.Web.WebView2 (https://learn.microsoft.com/nb-no/microsoft-edge/webview2/)
4. Ookii.Dialogs.Wpf (https://github.com/ookii-dialogs/ookii-dialogs-wpf)
5. Schisharp.Tensorflow.Redist (https://www.tensorflow.org/)

### Project classes
Beside the main class this project has severeal classes. We can divide these classes into classes directly connected to the ML modell and support classes. 

The classes for the machine learning model is following:
1. BirdrecognizerAI.consumption.cs: This is the class that contains methods for using the finished machine learning model. It is part of the ML.Net framework.
2. BirdrecognizerAI.training,cs: THis is the class that contains methods for retrain the model. It is part of the ML.Net framework.

The next classes can be defined as support classes:
1. BirdResult.cs: This class sends the chosen image to the proper model and returns the result from the AI model.
2. BirdSupport.cs: This class is for performing any changes to the text showing in the GUI or copying any text that is showed.
3. FileHandling.cs: THis class is linked to retraining the model with new data. This class contain methods for creating folder for new birds or add new image to existing folders. This method is not being used since the project do not allow user for retraining the model. Also this class could be merged with the class below OpenFile.cs.
4. OpenFile.cs: This class finds the path for the file the user has chosen for analysis.
5. Wikipedia.cs: This class creates the respective wikipedia URLs for the result. Also it shows the summary of the result the model returns

### Other uses for this project or parts of the project
This project can be adapted to be used in other project such as:
1. Industrial quality control
2. Product categorization
3. Security and surveillance

