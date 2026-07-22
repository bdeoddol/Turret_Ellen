using System.Collections.Immutable;
using ByteTrackCSharp;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using OpenCvSharp.Flann;

public class Postprocessing
{
    public static IDisposableReadOnlyCollection<OrtValue> infer(float[] src, long[] shape, ref InferenceSession model)
    {
        using var inputOrtValue = OrtValue.CreateTensorValueFromMemory(src, shape); //OrtValue
        using var runOptions = new RunOptions(); //RunOptions
            
        var inputs = new Dictionary<string, OrtValue> {
            { "images",  inputOrtValue }
        };

        IDisposableReadOnlyCollection<OrtValue>  output = model.Run(runOptions, inputs, model.OutputNames);
        return output;
    }
     public static List<Detection> parseOutputData(ref IDisposableReadOnlyCollection<OrtValue> data)
    {
        using var data_0 = data[0]; //OrtValue
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

        return List;
    }

    private static List<Detection> filterByConfidence(List<Detection> outputData, double cfdThreshold)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].conf >= cfdThreshold){List.Add(outputData[i]);}
        }
        
        return List;
    }
    private static List<Detection> filterByClass(List<Detection> outputData, int classID)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].classID == classID){List.Add(outputData[i]);}
        }
        return List;
    }

    private static void filterByClassOPT(ref List<Detection> outputData, int classID)
    {
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].classID != classID){outputData.RemoveAt(i);}
        }
    }
    private static void filterByConfidenceOPT(ref List<Detection> outputData, double cfdThreshold)
    {
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].conf < cfdThreshold){outputData.RemoveAt(i);}
        }
    }

    public static void plotDetections(List<Detection> output,  Mat frame)
    {

        // List<Detection> outputData = filterByConfidence(filterByClass(output, 0),0.5);
        filterByClassOPT(ref output, 0);
        filterByConfidenceOPT(ref output, 0.25);

        // List<Detection> outputData = filterByClass(output, 0);
        float x1, y1, x2, y2, cfd, cls;
        int width, height;
        int ConfAsPercent, detID;
        OpenCvSharp.Point center;
        for(int det = 0; det < output.Count; det++)
        {
            if(det >= output.Count){break;}
            //prepare data for each detection 
            x1 = output[det].x1;
            y1 = output[det].y1;
            x2 = output[det].x2;
            y2 = output[det].y2;
            cfd = output[det].conf;
            cls = output[det].classID;
            ConfAsPercent = (int)(cfd*100);
            detID = output[det].detID;
            
            width = output[det].width;
            height = output[det].height;
            center = output[det].boxCenter;
            

            //plot our detection
            plotSingularHelper((int)x1,(int)y1,width, height, center, ConfAsPercent, detID, frame);
        }
        
        
        return;
    }

    private static void plotSingularHelper(int x1, int y1, int width, int height, OpenCvSharp.Point center, int ConfPercent, int detID, Mat frame)
    {
        OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect(x1, y1, width, height); //construct our bounding box
        Scalar color = new Scalar(4, 28, 255); //construct w/ bgr values for bright red
        OpenCvSharp.Point upLeft = new OpenCvSharp.Point(x1, y1-5); //our text will be built starting from bottom left, attach bottom left to the top left of our bounding box

        //draw our boxes
        Cv2.Rectangle(frame, boundingBox, color, 2, LineTypes.Link8, 0);
        Cv2.Circle(frame, center, 2, color, 2);
        Cv2.PutText(frame, "Person " + detID +  ": "  + ConfPercent + "%", upLeft, HersheyFonts.HersheyDuplex, 0.5, color, 2); //TODO implement to change fonts

        return;
    }

    public static ByteTrackCSharp.Object DetToByteTrackObject(Detection detectContainer)
    {
        float width = detectContainer.x2-detectContainer.x1;
        float height = detectContainer.y2-detectContainer.y1;

        ByteTrackCSharp.Rect box = new ByteTrackCSharp.Rect(detectContainer.x1, detectContainer.y1, width, height);
        ByteTrackCSharp.Object retObj = new ByteTrackCSharp.Object(box , (int)detectContainer.classID, detectContainer.conf);

        return retObj; 
    }

    public static STrack ObjToSTrack(ByteTrackCSharp.Object obj)
    {
        STrack retTrack = new STrack(obj.rect, obj.prob, obj.label);
        return retTrack;
    }

    public static List<Detection> ListSTrackToDet(List<STrack> track)
    {

        List<Detection> mutDet = new List<Detection>();
        foreach(STrack val in track)
        {
            ByteTrackCSharp.Rect BTRect = val.getRect();
            float score = val.getScore();
            int trackID = val.TrackId;
            int classID = val.classID; //new datamember in STrack class

            Detection det = new Detection(BTRect.x(), BTRect.y(), BTRect.x() + BTRect.width(), BTRect.y() + BTRect.height(), score, (float)classID, trackID);
            mutDet.Add(det);
        }    
        
                        

        return mutDet;
    }
}