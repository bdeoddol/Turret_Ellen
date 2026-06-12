using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;


class MatrixBlob
{
    public static void Run(string filepath)
    {
        Mat frame = Cv2.ImRead(filepath);
        Console.WriteLine("Initial Frame Mat Type: " + frame.Type());
        Mat blobbed_frame = CvDnn.BlobFromImage(frame, 1, default, default, true, false);
        //after blobbing from image, you cannot refer to mat.rows/mat.cols at the 4th dimension, this only works from 1st to 3rd dimensions
        //you must reference sizes of the matrix at the n-th dimensions

        Console.WriteLine(blobbed_frame);
        Console.WriteLine("Blobbed frame dims: " + blobbed_frame.Dims);
        Console.WriteLine(blobbed_frame.Size());
        Console.WriteLine("B: " + blobbed_frame.Size(0)); //matrix size at the 1st dim
        Console.WriteLine("C: " + blobbed_frame.Size(1));
        Console.WriteLine("H: " + blobbed_frame.Size(2));
        Console.WriteLine("W: " + blobbed_frame.Size(3)); //matrix size at the 4th dim


        Console.WriteLine("\n\n Flattening....");
        Mat flattened = blobbed_frame.Reshape(1,1);
        Size size = flattened.Size();
        Console.WriteLine(flattened);
        Console.WriteLine(flattened.Size());
        
        // Console.WriteLine("Batch: " + blobbed_frame.Size(0));
        Console.WriteLine("Flattened Total channels: " + flattened.Channels());
        Console.WriteLine("Flattened Total num rows: " + flattened.Size(0));
        Console.WriteLine("Flattened Total num cols: " + flattened.Size(1)); //verify num rows
        Console.WriteLine("Flattened Total dims: " + flattened.Dims);
        Console.WriteLine("Flattened Blobbed Frame Mat Type: " + flattened.Type());


        int col;
         for(col = 0; col < flattened.Cols; col++)
            {
              if(col % 10000 == 0)
                {
                    Console.Write(flattened.At<float>(0, col) + " ");
                }
              
            }  
        Console.WriteLine("");
        Console.WriteLine("Total Num Cols: " + col);             
        Console.WriteLine("");
        Console.WriteLine("printing first 3 pixels...");
        {
            for(int i = 0; i < 3; i++)
            {
                Console.Write(flattened.At<float>(0,i)+ " ");
            }
        }
        
    }
}