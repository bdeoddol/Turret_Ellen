using System.Drawing.Printing;
using System.Runtime.Versioning;
using OpenCvSharp;
using OpenCvSharp.Extensions;

class MatrixFlatten
{
    public static void Run(string filepath)
    {
        Mat frame = Cv2.ImRead(filepath);
        frame = frame.Reshape(1,1);
        Vec3b pixel;
        byte blue;
        byte green;
        byte red;
        int col;
        int row;
        int frameRows = frame.Rows;
        int frameCols = frame.Cols;

        Console.WriteLine(frame);
        for(row = 0; row < frameRows; row++)
        {
         for(col = 0; col < frameCols; col++)
            {
              if(col % 10000 == 0)
                {
                    pixel = frame.At<Vec3b>(row,col);
                    blue = pixel.Item0;
                    green = pixel.Item1;
                    red = pixel.Item2;

                    Console.WriteLine("[" + blue + " " + green + " " + red + "]");      
                }
              
            }  
        Console.WriteLine("Total Num Cols: " + col);             
        }
        Console.WriteLine("Total Num Rows: " + row);
        Console.WriteLine(frame);
        Console.WriteLine(frame.Size());
        
    }


}