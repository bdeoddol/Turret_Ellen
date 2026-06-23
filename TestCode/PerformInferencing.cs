using System.Buffers;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing.Imaging.Effects;
using System.Net.Security;
using System.Numerics;
using System.Numerics.Tensors;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using OpenCvSharp.ML;


class PerformInferencing
{
    public static void Run(string modelpath, string imgPath)
    {
        InferenceSession currmodel;
        try
        {
            using var options = SessionOptions.MakeSessionOptionWithCudaProvider(0);
            currmodel = new InferenceSession(modelpath, options);
            // use CUDA
        }
        catch
        {
            currmodel = new InferenceSession(modelpath);
            // use CPU
        }              

    //preprocessing
        Mat frame = Cv2.ImRead(imgPath); //image to be processed
        int frameOrigWidth = frame.Width;
        int frameOrigHeight = frame.Height;
        int aUWidth = ImgResizev1.AlignUp(frame.Width)*32;
        int aUHeight = ImgResizev1.AlignUp(frame.Height)*32;
        int aDWidth = ImgResizev1.AlignDown(frame.Width)*32;
        int aDHeight = ImgResizev1.AlignDown(frame.Height)*32;
        if(ImgResizev1.ValidateImgDim(frame) == false)
        {
            ImgResizev1.performResize(frame, aUWidth, frame.Height*(aUWidth/frame.Width));
            ImgResizev1.performPaddingVert(frame,aUHeight);
        }

        float[] src = PreprocessingTest.prepareSource(frame);
        long[] shape = PreprocessingTest.prepareShape(frame);
    //

        IDisposableReadOnlyCollection<OrtValue> sampleOutput;
        sampleOutput = infer(src, shape, currmodel);
        ImmutableList<Detection> detections = parseOutputData(sampleOutput);
        // getOutputInfo(sampleOutput);
        plotDetections(detections, frame, imgPath);
        
        sampleOutput.Dispose();
        
        return;
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
    
    private static ImmutableList<Detection> parseOutputData(IDisposableReadOnlyCollection<OrtValue> data)
    {
        OrtValue data_0 = data[0];
        ReadOnlySpan<float> dataSpan = data_0.GetTensorDataAsSpan<float>();
        
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < dataSpan.Length; i+=6)
        {
            Detection newDet = new Detection(
                    dataSpan[i+0],
                    dataSpan[i+1],
                    dataSpan[i+2],
                    dataSpan[i+3],
                    dataSpan[i+4],
                    dataSpan[i+5]);
            List.Add(newDet);
        }
        ImmutableList<Detection> retList = List.ToImmutableList<Detection>();

        return retList;
    }

    private static ImmutableList<Detection> filterByConfidence(ImmutableList<Detection> outputData, double cfdThreshold)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].conf >= cfdThreshold){List.Add(outputData[i]);}
        }
        ImmutableList<Detection> retList = List.ToImmutableList();
        return retList;
    }

    private static ImmutableList<Detection> filterByClass(ImmutableList<Detection> outputData, int classID)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].classID == classID){List.Add(outputData[i]);}
        }
        ImmutableList<Detection> retList = List.ToImmutableList();
        return retList;
    }

    private static void plotDetections(ImmutableList<Detection> output, Mat frame, string imgPath)
    {

        // ImmutableList<Detection> outputData = filterByConfidence(filterByClass(output, 0), 0.50);
        ImmutableList<Detection> outputData = filterByClass(output, 0);
        float x1, y1, x2, y2, cfd, cls, width, height;
        int ConfAsPercent;
        for(int det = 0; det < outputData.Count; det++)
        {
            if(det >= outputData.Count){break;}
            //prepare data for each detection 
            x1 = outputData[det].x1;
            y1 = outputData[det].y1;
            x2 = outputData[det].x2;
            y2 = outputData[det].y2;
            cfd = outputData[det].conf;
            cls = outputData[det].classID;
            ConfAsPercent = (int)(cfd*100);
            
            width = x2-x1;
            height = y2-y1;

            Console.WriteLine("Detection no." + det + ": " + x1 + " " + y1 +  " " + x2 + " " + y2 + " " + width + " " + height + " " + cfd + " " + cls);   

            //plot our detection
            plotSingularHelper((int)x1,(int)y1,(int)width,(int)height,ConfAsPercent, frame);
        }
        
        Mat origImg = Cv2.ImRead(imgPath);
        Rect ROI = ImgResizev1.GetRectOfOriginalFrame(origImg);
        frame = new Mat(frame, ROI);    
        ImgResizev1.performResize(frame, origImg.Width, origImg.Height);
        Cv2.ImWrite("bbFrame.jpg", frame);        
        return;
    }

    private static void plotSingularHelper(int x1, int y1, int width, int height, int ConfPercent, Mat frame)
    {
        Rect boundingBox = new Rect(x1, y1, width, height); //construct our bounding box
        Scalar color = new Scalar(4, 28, 255); //construct w/ bgr values for bright red
        Point upLeft = new Point(x1, y1-5); //our text will be built starting from bottom left, attach bottom left to the top left of our bounding box

        //draw our boxes
        Cv2.Rectangle(frame, boundingBox, color, 2, LineTypes.Link8, 0);
        Cv2.PutText(frame, "Person: "  + ConfPercent + "%", upLeft, HersheyFonts.HersheyDuplex, 0.5, color, 2); //TODO implement to change fonts


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