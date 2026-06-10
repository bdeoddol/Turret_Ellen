using OpenCvSharp;
class TestOCV{
    public static void Run()
    {
        Mat? frame = Cv2.ImRead("../testImgs/magazine.jpg");
        byte? blue;
        byte? green;
        byte? red;
        int col;
        int row;

        for (row = 0; row < frame.Rows; row++)
        {
            Console.Write("[");
            for (col = 0; col < frame.Cols; col++)
            {
                if(col % 100 == 0)
                {
                    //for every tenth pixel, print the bgr values
                    Vec3b pixels = frame.At<Vec3b>(row, col);
                    blue = pixels.Item0;
                    green = pixels.Item1;
                    red = pixels.Item2;

                    Console.Write("[" + blue + " " + green + " " + red + "]");
                    Console.Write("...");
                }
                // Use At<byte> for 8-bit unsigned matrices
            }
            Console.WriteLine("num col: " + col); //verify num col
            Console.WriteLine("]");
        }

        Console.WriteLine("num rows: " + row); //verify num rows
        Console.WriteLine(frame.Size());
    }
}