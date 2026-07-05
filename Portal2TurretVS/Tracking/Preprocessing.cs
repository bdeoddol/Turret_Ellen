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
}