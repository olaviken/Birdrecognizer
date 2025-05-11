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

        public static async Task<BirdResult> AnalyzeImage(string path)
        {
            var inputImage = new BirdrecognizerAI.ModelInput()
            {
                ImageSource = System.IO.File.ReadAllBytes(path)
            };

            var result = BirdrecognizerAI.PredictEngine.Value.Predict(inputImage);

            return new BirdResult
            {
                BirdName = result.PredictedLabel,
                Score = result.Score.Max(),
            };
        }
    }
}
