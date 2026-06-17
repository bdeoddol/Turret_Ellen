using System.Drawing.Imaging.Effects;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;


class PerformInferencing
{
    public static void Run(string modelpath, string imgPath)
    {
        IDisposableReadOnlyCollection<OrtValue> sampleOutput;
        InferenceSession currmodel = new InferenceSession(modelpath);

        Mat frame = Cv2.ImRead(imgPath); //image resized to 640x640 (images are required to fit a dimension divisible by 32)
        float[] src = PreprocessingTest.prepareSource(frame);
        long[] shape = PreprocessingTest.prepareShape(frame);


        // infer(src, shape, currmodel);
        sampleOutput = infer(src, shape, currmodel);
        getOutputInfo(sampleOutput);
        
        
    }

    private static IDisposableReadOnlyCollection<OrtValue> infer(float[] src, long[] shape, InferenceSession model)
    {
        OrtValue inputOrtValue = OrtValue.CreateTensorValueFromMemory(src, shape);
        RunOptions runOptions = new RunOptions();
            
        var inputs = new Dictionary<string, OrtValue> {
            { "images",  inputOrtValue }
        };

        IDisposableReadOnlyCollection<OrtValue> output = model.Run(runOptions, inputs, model.OutputNames);

        return output;
    }

    private static void getOutputInfo(IDisposableReadOnlyCollection<OrtValue> output)
    {
        OrtValue output_0 = output[0]; //access singular batch at index 0
       
        
        OrtTensorTypeAndShapeInfo tensorTypeAndShape = output_0.GetTensorTypeAndShape();
        Console.WriteLine("Output tensor dim count: " + tensorTypeAndShape.DimensionsCount);
        Console.WriteLine("Output element count: " + tensorTypeAndShape.ElementCount);
        Console.WriteLine("Output tensor element data type: tensorTypeAndShape.ElementDataType");
        long[] outputShape = tensorTypeAndShape.Shape;
        Console.Write("Output tensor shape: ( ");
        foreach (var value in outputShape)
        {
            Console.Write(value + " ");
        }
        Console.Write(")\n\n");

        ReadOnlySpan<float> outputData = output_0.GetTensorDataAsSpan<float>(); //grab the internal data as a span
        Console.WriteLine("Internal Data: ");
        Console.WriteLine("Num Elements, 300 detections for 6 detection elements: " + outputData.Length);

        int detCnt = 0;
        for(int i = 0; i < 300; i+=6)
        {
            if(outputData[i+5] == 0){
                Console.WriteLine("Detection no. " + detCnt);
                Console.WriteLine("x1: " + outputData[i+0]);
                Console.WriteLine("y1: " + outputData[i+1]);
                Console.WriteLine("x2: " + outputData[i+2]);
                Console.WriteLine("y2: " + outputData[i+3]);
                Console.WriteLine("Conf: " + outputData[i+4]);
                Console.WriteLine("Detected Class ID: " + outputData[i+5] + "\n");
                detCnt++;
            }
    
        }



        return;
    }


}

