
class Program
{
    static int Main(string[] args)
    {
        //main Benchmark entrypoint
        //change program name/ filepath accordingly
        // PreprocessingTest.Run("../testImgs/imrs-sampleWebcam.jpg");
        ImgResizev1.Run("../testImgs/MessiRonaldo2.jpg");
        PerformInferencing.Run("../TestAssets/yolo26n.onnx", "rs-frame.jpg");
        // ModelInfo.Run("../TestAssets/yolo26m.onnx");
        

        return 0;
    }
}
