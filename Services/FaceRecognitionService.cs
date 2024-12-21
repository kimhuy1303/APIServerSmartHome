using OpenCvSharp;
using OpenCvSharp.Face;
using static System.Net.Mime.MediaTypeNames;

namespace APIServerSmartHome.Services
{
    public class FaceRecognitionService
    {
        private LBPHFaceRecognizer _recognizer;
        public FaceRecognitionService()
        {
            _recognizer = LBPHFaceRecognizer.Create();
        }

        public void TrainRecognizer(List<byte[]> images, List<int> labels)
        {
            var trainingImages = images.Select(img => Mat.FromImageData(img, ImreadModes.Grayscale)).ToArray();
            _recognizer.Train(trainingImages, labels.ToArray());
        }

        public int Predict(byte[] input)
        {
            var inputMat = Mat.FromImageData(input, ImreadModes.Grayscale);
            return _recognizer.Predict(inputMat);
        }

        public double CompareFacesData(byte[] img1, byte[] img2)
        {
            using var mat1 = Mat.FromImageData(img1, ImreadModes.Grayscale);
            using var mat2 = Mat.FromImageData(img2, ImreadModes.Grayscale);

            var orb = ORB.Create();
            var keyPoints1 = orb.Detect(mat1);
            var keyPoints2 = orb.Detect(mat2);

            using var descriptor1 = new Mat();
            using var descriptor2 = new Mat();
            orb.Compute(mat1, ref keyPoints1, descriptor1);
            orb.Compute(mat2, ref keyPoints2, descriptor2);

            var bfMatcher = new BFMatcher(NormTypes.Hamming, crossCheck: true);
            var matches = bfMatcher.Match(descriptor1, descriptor2);

            return matches.Average(m => m.Distance);
        }
    }
}
