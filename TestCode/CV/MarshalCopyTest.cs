using System.Reflection.Metadata;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Numerics.Tensors;

class MarshalCopyTest
{
    public static void Run(string filepath)
    {
        
        Mat frame = Cv2.ImRead(filepath);
        Mat blobbed_frame = CvDnn.BlobFromImage(frame, 1.0/255.0, default, default, true, false);
        Mat flat = blobbed_frame.Reshape(1,1);  //note that Reshape() returns the same reshaped matrix, it does not modify the obj itself
        int len = flat.Cols;

        ulong[] shape = new ulong[4];
        IntPtr flatPtr = flat.Data;
        float[] hi = new float[len];
        Marshal.Copy(flatPtr, hi, 0, len);

        shape[0] = 1;
        shape[1] = 3;
        shape[2] = (ulong)blobbed_frame.Size(2);
        shape[3] = (ulong)blobbed_frame.Size(3);


        Console.WriteLine("Float array prepared, printing every 10000th element...");
        for(int i = 0; i < len; i++)
        {if(i % 10000 == 0){Console.Write(hi[i] + " ");}}
        Console.Write("\nTensor Shape in BCHW: ");
        for(int i = 0; i < 4; i++){
            Console.Write(shape[i] + " ");
        }

        
    }
}