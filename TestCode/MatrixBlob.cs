using System.Reflection.Metadata;
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
        blobbed_frame = blobbed_frame.Reshape(1,1);

        
        

        Size size = blobbed_frame.Size();
        Console.WriteLine(blobbed_frame);
        Console.WriteLine(blobbed_frame.Size());
        //BCHW
        Console.WriteLine("Batch: " + blobbed_frame.Size(0));
        Console.WriteLine("Total channels: " + blobbed_frame.Size(1));
        Console.WriteLine("Total num rows: " + blobbed_frame.Size(2));
        Console.WriteLine("Total num cols: " + blobbed_frame.Size(3)); //verify num rows
        Console.WriteLine("Total dims: " + blobbed_frame.Dims);
        Console.WriteLine("Blobbed Frame Mat Type: " + blobbed_frame.Type());


    }
}