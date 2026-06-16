using System.Drawing.Imaging.Effects;
using System.Runtime.CompilerServices;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;


class PerformInferencing
{
    public static void Run(string modelpath)
    {
        
        InferenceSession currmodel = new InferenceSession(modelpath);

        Mat frame = Cv2.ImRead("../testImgs/imrs-sampleWebcam.jpg"); //image resized to 640x640 (images are required to fit a dimension divisible by 32)
        float[] src = PreprocessingTest.prepareSource(frame);
        long[] shape = PreprocessingTest.prepareShape(frame);


        // infer(src, shape, currmodel);
        getModelInfo(currmodel);
        
    }

    private static void infer(float[] src, long[] shape, InferenceSession model)
    {
        OrtValue inputOrtValue = OrtValue.CreateTensorValueFromMemory(src, shape);
        RunOptions runOptions = new RunOptions();
            
        var inputs = new Dictionary<string, OrtValue> {
            { "images",  inputOrtValue }
        };

        using var output = model.Run(runOptions, inputs, model.OutputNames);

        OrtValue output_0 = output[0]; //access singular batch at index 0
        ReadOnlySpan<float> outputData = output_0.GetTensorDataAsSpan<float>();
        

        OrtTensorTypeAndShapeInfo tensorTypeAndShape = output_0.GetTensorTypeAndShape();
        Console.WriteLine("Output tensor dim count: " + tensorTypeAndShape.DimensionsCount);
        Console.WriteLine("Output element count: " + tensorTypeAndShape.ElementCount);

        long[] outputShape = tensorTypeAndShape.Shape;
        Console.Write("Output tensor shape: ( ");
        foreach (var value in outputShape)
        {
            Console.Write(value + " ");
        }
        Console.Write(")\n");

        // int cnt = 0;
        // foreach(var value in outputData)
        // {
        //     if( value % 6 == 0)
        //     {
        //         cnt++;
        //     }
        // }
        // Console.WriteLine(cnt);
    }

    private static void getModelInfo(InferenceSession model)
    {
        // check input data info
        IReadOnlyDictionary<string, NodeMetadata> inputMdat = model.InputMetadata;
        NodeMetadata valuePair;
        string[] symbolicDims;
        Console.WriteLine("Model Input Metadata: ");
        foreach(var value in inputMdat)
        {
            Console.WriteLine(value.Key + "-> ");
                valuePair = value.Value;
                symbolicDims = valuePair.SymbolicDimensions;
                Console.WriteLine("\t Value Pair Symbolic Dims represented as: ");
                Console.Write("\t( ");
                foreach(string variable in symbolicDims)
                {
                    Console.Write(variable + " ");
                }
                Console.Write(" )\n\n");

                
        
        }        

        // check output data info
        IReadOnlyDictionary<string, NodeMetadata> outputMdat =  model.OutputMetadata;
        Console.WriteLine("Model Output Metadata: ");
        foreach(var value in outputMdat)
        {Console.WriteLine(value.Key + ", " + value.Value);}
    }
}

