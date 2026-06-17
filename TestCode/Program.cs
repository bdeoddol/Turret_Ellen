
class Program
{
    static int Main(string[] args)
    {
        //main Benchmark entrypoint
        //change program name/ filepath accordingly
        // PreprocessingTest.Run("../testImgs/magazine.jpg");do
        PerformInferencing.Run("../TestAssets/yolo26m.onnx", "../testImgs/imrs-sampleWebcam.jpg");

        return 0;
    }
}
