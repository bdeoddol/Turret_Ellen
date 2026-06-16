
class Program
{
    static int Main(string[] args)
    {
        //main Benchmark entrypoint
        //change program name/ filepath accordingly
        // PreprocessingTest.Run("../testImgs/magazine.jpg");do
        ModelInfo.Run("../TestAssets/yolo26m.onnx");

        return 0;
    }
}
