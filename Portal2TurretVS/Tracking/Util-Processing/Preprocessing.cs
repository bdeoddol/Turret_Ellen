using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using System.Runtime.InteropServices;

public class Preprocessing
{
    public static float[] prepareSrc(Mat frame)
    {
        Mat blobbed_frame = CvDnn.BlobFromImage(frame, 1.00 / 255.00, default, default, true, false); //https://github.com/orgs/ultralytics/discussions/6382?utm_source=chatgpt.com#discussioncomment-9387981
        Mat flattened = blobbed_frame.Reshape(1, 1);
        uint len = (uint)flattened.Size(1);
        float[] retArray = new float[len];
        IntPtr matPtr = flattened.Data;

        Marshal.Copy(matPtr, retArray, 0, (int)len);

        return retArray;
    }
    public static long[] prepareShape(Mat frame)
    {
        Mat blobbed_frame = CvDnn.BlobFromImage(frame, 1.00 / 255.00, default, default, true, false); //https://github.com/orgs/ultralytics/discussions/6382?utm_source=chatgpt.com#discussioncomment-9387981
        int dims = blobbed_frame.Dims;
        long[] retArray = new long[dims];
        for (int i = 0; i < dims; i++){retArray[i] = blobbed_frame.Size(i);}

        return retArray;
    }

    public static void performWarmupInferencing(ref InferenceSession inferenceModel)
    {
         Mat sampleFrame;
        float[] sampleSrc;
        long[] sampleShape;
        string[] samplePaths = {
            Path.Combine(AppContext.BaseDirectory, "assets", "imrs-magazine1024x1024.jpg"),
            Path.Combine(AppContext.BaseDirectory, "assets", "sampleimg960x540.jpg"),
            Path.Combine(AppContext.BaseDirectory, "assets", "800x608.jpg")
            };
        foreach(string val in samplePaths)
        {
            sampleFrame = Cv2.ImRead(val);
            sampleSrc = Preprocessing.prepareSrc(sampleFrame);
            sampleShape = Preprocessing.prepareShape(sampleFrame);
            using var sampleOuput = Postprocessing.infer(sampleSrc, sampleShape, ref inferenceModel);
            }
    }

    public static Rect GetRectOfOriginalFrame(Mat frame)
    {
        int aUWidth = AlignUp(frame.Width)*32;
        int aUHeight = AlignUp(frame.Height)*32;
        int difference = aUHeight-frame.Height;

        Rect retRect = new Rect(0,difference/2, aUWidth, aUHeight-difference);
        return retRect;
    }

    public static void performPaddingVert(Mat frame, int targetHeight)
    {
        int heightDiff = targetHeight - frame.Height;        
        Cv2.CopyMakeBorder(frame, frame, heightDiff/2, heightDiff-(heightDiff/2),0, 0, BorderTypes.Isolated);
        return;
    }

    public static void performPaddingHori(Mat frame, int targetWidth)
    {
        int widthDiff = targetWidth - frame.Width;
        Cv2.CopyMakeBorder(frame, frame, 0, 0, widthDiff/2, widthDiff-(widthDiff/2), BorderTypes.Isolated);
        return;
    }

    public static void performResize(Mat frame, int targetWidth, int targetHeight)
    {
        OpenCvSharp.Size newsize = new OpenCvSharp.Size(targetWidth, targetHeight);
        Cv2.Resize(frame,frame, newsize);
        return;
    }


    public static bool ValidateImgDim(Mat frame)
    {
        int height = frame.Height;
        int width = frame. Width;
        if(height%32 != 0 || width%32!= 0){return false;}
        else{return true;}
    }

    public static int AlignUp(int dim)
    {
        return (dim + 32 - 1)/32;
    }
    public static int AlignDown(int dim)
    {
        return dim/32;
    }
    
}