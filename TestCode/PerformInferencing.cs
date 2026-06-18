using System.Collections.Immutable;
using System.Data;
using System.Drawing.Imaging.Effects;
using System.Numerics;
using System.Numerics.Tensors;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;


class PerformInferencing
{
    public static void Run(string modelpath, string imgPath)
    {
        IDisposableReadOnlyCollection<OrtValue> sampleOutput;
        InferenceSession currmodel = new InferenceSession(modelpath);

        Mat frame = Cv2.ImRead(imgPath); //image resized(images are required to fit a dimension divisible by 32)
        float[] src = PreprocessingTest.prepareSource(frame);
        long[] shape = PreprocessingTest.prepareShape(frame);


        // infer(src, shape, currmodel);
        sampleOutput = infer(src, shape, currmodel);
        // getOutputInfo(sampleOutput);
        plotDetections(sampleOutput, imgPath);
        sampleOutput.Dispose();
        
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
        Console.WriteLine("Num Elements, 300 detections of 6 detection elements: " + outputData.Length);

        int detCnt = 0;
        for(int i = 0; i < 30; i+=6)
        {
            // if(outputData[i+5] == 0 ){
                Console.WriteLine("Detection no. " + detCnt);
                Console.WriteLine("x1: " + outputData[i+0]);
                Console.WriteLine("y1: " + outputData[i+1]);
                Console.WriteLine("x2: " + outputData[i+2]);
                Console.WriteLine("y2: " + outputData[i+3]);
                Console.WriteLine("Conf: " + outputData[i+4]);
                Console.WriteLine("Detected Class ID: " + outputData[i+5] + "\n");
            // }
            detCnt++;
        }
        return;
    }
    
    private static ImmutableList<Detection> filterByConfidence(ReadOnlySpan<float> outputData, double cfdThreshold)
    {
        List<Detection> List = new List<Detection>();

        for(int i = 0; i < outputData.Length; i+=6)
        {
            if(outputData[i+4] >= cfdThreshold)
            {
                Detection newDet = new Detection(
                    outputData[i+0],
                    outputData[i+1],
                    outputData[i+2],
                    outputData[i+3],
                    outputData[i+4],
                    outputData[i+5]);
                    List.Add(newDet);
            }
        }
        ImmutableList<Detection> retList = List.ToImmutableList();
        
        return retList;
    }

    private static ImmutableList<Detection> filterByClass(ReadOnlySpan<float> outputData, int classID)
    {
        List<Detection> List = new List<Detection>();

        for(int i = 0; i < outputData.Length; i+=6)
        {
            if(outputData[i+5] == classID)
            {
                Detection newDet = new Detection(
                    outputData[i+0],
                    outputData[i+1],
                    outputData[i+2],
                    outputData[i+3],
                    outputData[i+4],
                    outputData[i+5]
                    );
                    List.Add(newDet);
            }
        }
        ImmutableList<Detection> retList = List.ToImmutableList();
        
        return retList;
    }

    private static void plotDetections(IDisposableReadOnlyCollection<OrtValue> output, string imgPath)
    {
        OrtValue output_0 = output[0];
        ReadOnlySpan<float> outputSpan = output_0.GetTensorDataAsSpan<float>();
        // ImmutableList<Detection> outputData = filterByClass(outputSpan, 0); 
        ImmutableList<Detection> outputData = filterByConfidence(outputSpan, 0.75);
        Mat frame = Cv2.ImRead(imgPath);

        
        float x1, y1, x2, y2, cfd, cls, width, height;
        int ConfPercent;
        for(int det = 0; det < outputData.Count; det++)
        {
            //prepare data for each detection 
            x1 = outputData[det].x1;
            y1 = outputData[det].y1;
            x2 = outputData[det].x2;
            y2 = outputData[det].y2;
            cfd = outputData[det].conf;
            cls = outputData[det].classID;

            width = x2 = x1;
            height = y2-y1;
            Console.WriteLine(x1 + " " + y1 +  " " + x2 + " " + y2 + " " + width + " " + height + " " + cfd + " " + cls);   
            ConfPercent = (int)(cfd*100);

            //plot our detection
            plotSingularDetection((int)x1,(int)y1,(int)width,(int)height,ConfPercent, frame);
        }
        
        Cv2.ImWrite("bbFrame.jpg", frame);

        return;
    }

    private static void plotSingularDetection(int x1, int y1, int width, int height, int ConfPercent, Mat frame)
    {
        Rect boundingBox = new Rect(x1, y1, width, height); //construct our bounding box
        Scalar color = new Scalar(57, 255, 20); //construct w/ rgb values for neon-green
        Point upLeft = new Point(x1, y1-5); //our text will be built starting from bottom left, attach bottom left to the top left of our bounding box


        //draw our boxes
        Cv2.Rectangle(frame, boundingBox, color, 2, LineTypes.Link8, 0);
        Cv2.PutText(frame, "Person: "  + ConfPercent + "%", upLeft, HersheyFonts.HersheySimplex, 0.75, color, 2);

    }
}


        // int row = 0;
        // int offset = 6 * row;
        // float x1 = outputData[offset + 0];
        // float y1 = outputData[offset + 1];
        // float x2 = outputData[offset + 2];
        // float y2 = outputData[offset + 3];
        // float cfd = outputData[offset + 4];
        // int cls = (int)outputData[offset + 5];

        // float width = x2 - x1;
        // float height = y2 - y1;
        // Console.WriteLine(x1 + " " + x2 + " " + width + " " + height + " " + cfd + " " + cls);

        // Rect boundingBox = new Rect((int)x1, (int)y1, (int)width, (int)height); 

        // Scalar color = new Scalar(57, 255, 20); //construct w/ rgb values for neon-green

        // Cv2.Rectangle(frame, boundingBox, color, 2, LineTypes.Link8, 0);
        

        // Cv2.ImWrite("bbFrame.jpg", frame);