using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using OpenCvSharp;
using OpenCvSharp.Dnn;

class PreprocessingTest
{
    public static void Run(string filepath)
    {
        Mat frame = Cv2.ImRead(filepath);
        float[] src = prepareSource(frame);
        long[] dimensions = prepareShape(frame);

        for(int i = 0; i < (frame.Size(0) * frame.Size(1) * frame.Channels()); i++)
        {
            if(i % 10000 == 0)
            {
                Console.Write(src[i] + " ");
            }
        }
        Console.WriteLine("\nBCHW Shape: ");
        for(int i = 0; i < dimensions.Length; i++)
        {
            Console.Write(dimensions[i] + " ");
        }
    }
    private static float[] prepareSource(Mat frame)
    {
        Mat blobbed_frame = CvDnn.BlobFromImage(frame, 1, default, default, true, false);
        Mat flattened = blobbed_frame.Reshape(1,1);
        uint len = (uint)flattened.Size(1);
        float[] retArray = new float[len];
        IntPtr matPtr = flattened.Data;

        Marshal.Copy(matPtr, retArray, 0, (int)len);

        return retArray;
    }
    private static long[] prepareShape(Mat frame)
    {
        Mat blobbed_frame = CvDnn.BlobFromImage(frame, 1, default, default, true, false);
        int dims = blobbed_frame.Dims;
        long[] retArray = new long[dims];
        for(int i = 0; i < dims; i++)
        {
            retArray[i] = blobbed_frame.Size(i);
        }
        
        return retArray;
    }
}