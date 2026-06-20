
class Program
{
    static int Main(string[] args)
    {
        //main Benchmark entrypoint
        //change program name/ filepath accordingly
        // PreprocessingTest.Run("../testImgs/imrs-sampleWebcam.jpg");
        // PerformInferencing.Run("../TestAssets/yolo26m.onnx", "../testImgs/imrs-magazine.jpg");
        ModelInfo.Run("../TestAssets/yolo26m.onnx");

        return 0;
    }
}
