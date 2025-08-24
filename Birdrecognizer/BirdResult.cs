using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birdrecognizer
{
    public class BirdResult
    {
        public string BirdName { get; set; } = string.Empty;
        public float Score { get; set; }

        public static async Task<BirdResult> AnalyzeImage(string path, ITransformer model, MLContext mLContext)
        {
            var inputImage = new BirdrecognizerAI.ModelInput()
            {
                ImageSource = await System.IO.File.ReadAllBytesAsync(path)
            };

            var predictionEngine = mLContext.Model.CreatePredictionEngine<BirdrecognizerAI.ModelInput, BirdrecognizerAI.ModelOutput>(model);
            var result = predictionEngine.Predict(inputImage);

            return new BirdResult
            {
                BirdName = result.PredictedLabel,
                Score = result.Score.Max(),
            };
        }
    }
}
