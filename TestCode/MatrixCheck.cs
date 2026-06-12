using OpenCvSharp;
class MatrixCheck{
    public static void Run(string filepath)
    {
        Mat frame = Cv2.ImRead(filepath);
        byte? blue;
        byte? green;
        byte? red;
        int col=0;
        int row=0;
        int frameRows = frame.Rows;
        int frameCols = frame.Cols;

        Console.WriteLine(frame);
        for (row = 0; row < frameRows; row++)
        {
            if(row % 100 == 0)
            {
            //print every 100th row
                Console.Write("[");
                for (col = 0; col < frameCols; col++)
                {
                    if(col % 100 == 0)
                    {
                        //for every tenth pixel, print the bgr values
                        Vec3b pixels = frame.At<Vec3b>(row, col);
                        blue = pixels.Item0;
                        green = pixels.Item1;
                        red = pixels.Item2;

                        Console.Write("[" + blue + " " + green + " " + red + "] num col: " + col);
                        Console.WriteLine("...");
                    }
                    // Use At<byte> for 8-bit unsigned matrices
                }
            }       
        }
        Console.WriteLine("Total num cols: " + col);
        Console.WriteLine("Total num rows: " + row); //verify num rows
        Console.WriteLine(frame.Size());
        Console.WriteLine(frame);
        Console.WriteLine("Total channels: " + frame.Channels());
        Console.WriteLine("Total num rows: " + frame.Size(0));
        Console.WriteLine("Total num cols: " + frame.Size(1)); //verify num rows
        Console.WriteLine("Total dims: " + frame.Dims);
        Console.WriteLine("Mat Type: " + frame.Type());


        
    }
}