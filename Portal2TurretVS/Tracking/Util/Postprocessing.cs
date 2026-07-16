using System.Collections.Immutable;
using ByteTrackCSharp;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;

public class Postprocessing
{
    public static IDisposableReadOnlyCollection<OrtValue> infer(float[] src, long[] shape, InferenceSession model)
    {
        OrtValue inputOrtValue = OrtValue.CreateTensorValueFromMemory(src, shape);
        RunOptions runOptions = new RunOptions();
            
        var inputs = new Dictionary<string, OrtValue> {
            { "images",  inputOrtValue }
        };

        IDisposableReadOnlyCollection<OrtValue> output = model.Run(runOptions, inputs, model.OutputNames);
        return output;
    }
     public static List<Detection> parseOutputData(IDisposableReadOnlyCollection<OrtValue> data)
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

        return List;
    }

    public static List<Detection> filterByConfidence(List<Detection> outputData, double cfdThreshold)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].conf >= cfdThreshold){List.Add(outputData[i]);}
        }
        
        return List;
    }

    public static List<Detection> filterByClass(List<Detection> outputData, int classID)
    {
        List<Detection> List = new List<Detection>();
        for(int i = 0; i < outputData.Count; i++)
        {
            if(outputData[i].classID == classID){List.Add(outputData[i]);}
        }
        return List;
    }

    public static void plotDetections(List<Detection> output,  Mat frame)
    {

        List<Detection> outputData = filterByConfidence(filterByClass(output, 0),0.5);
        // List<Detection> outputData = filterByClass(output, 0);
        float x1, y1, x2, y2, cfd, cls, width, height;
        int ConfAsPercent, detID;
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
            detID = outputData[det].detID;
            
            width = x2-x1;
            height = y2-y1;

            //plot our detection
            plotSingularHelper((int)x1,(int)y1,(int)width,(int)height,ConfAsPercent, detID, frame);
        }
        
        
        return;
    }

    private static void plotSingularHelper(int x1, int y1, int width, int height, int ConfPercent, int detID, Mat frame)
    {
        OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect(x1, y1, width, height); //construct our bounding box
        Scalar color = new Scalar(4, 28, 255); //construct w/ bgr values for bright red
        OpenCvSharp.Point upLeft = new OpenCvSharp.Point(x1, y1-5); //our text will be built starting from bottom left, attach bottom left to the top left of our bounding box

        //draw our boxes
        Cv2.Rectangle(frame, boundingBox, color, 2, LineTypes.Link8, 0);
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