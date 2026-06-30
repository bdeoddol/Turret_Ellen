using OpenCvSharp;
using OpenCvSharp.Dnn;
using System.Runtime.InteropServices;

public class Preprocessing
{
    public static float[] prepareSrc(Mat frame)
    {
        frame = CvDnn.BlobFromImage(frame, 1.0 / 255.0, default, default, true, false);
        Mat flattened = frame.Reshape(1, 1);
        uint len = (uint)flattened.Size(1);

        IntPtr matData = flattened.Data;
        float[] retArray = new float[len];
        Marshal.Copy(matData, retArray, 0, (int)len);

        return retArray;
    }
    public static long[] prepareShape(Mat frame)
    {
        Mat blob = CvDnn.BlobFromImage(frame, 1.0 / 255.0, default, default, true, false);
        int dims = blob.Dims;
        long[] retArray = new long[dims];

        for (int i = 0; i < dims; i++)
        { retArray[i] = blob.Size(i); }

        return retArray;
    }
}