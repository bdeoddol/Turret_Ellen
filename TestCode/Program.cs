
class Program
{
    static int Main(string[] args)
    {
        //main Benchmark entrypoint
        //change program name/ filepath accordingly
        // PreprocessingTest.Run("../testImgs/imrs-sampleWebcam.jpg");
        // ImgResizev1.Run("../testImgs/magazine.jpg");
        // PerformInferencing.Run("../TestAssets/yolo26n.onnx", "../testImgs/imrs-magazine.jpg");
        // ModelInfo.Run("../TestAssets/yolo26m.onnx");
        OpenPort.Run();
        // Capture.Run();
        

        return 0;
    }
}
