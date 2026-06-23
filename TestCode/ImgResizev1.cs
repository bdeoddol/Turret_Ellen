using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using OpenCvSharp;

class ImgResizev1
{
    public static void Run(string imgPath)
    {
        Console.WriteLine("ImgResizev1");
        Mat frame = Cv2.ImRead(imgPath);
        ValidateImgDim(frame);
        int aUWidth = AlignUp(frame.Width)*32;
        int aUHeight = AlignUp(frame.Height)*32;
        int aDWidth = AlignDown(frame.Width)*32;
        int aDHeight = AlignDown(frame.Height)*32;
        Console.WriteLine("Original dims: " + frame.Width + "x" + frame.Height);
        Console.WriteLine("Aligned up dims: " + aUWidth  + "x" + aUHeight);
        Console.WriteLine("Aligned down dims: " + aDWidth + "x" + aDHeight);

        performPadding(frame, aUWidth, aUHeight);
        // performResize(frame, aUWidth, aUHeight);

        Cv2.ImWrite("imrs-frame.jpg", frame);
        return;
    }

    private static void performPadding(Mat frame, int upsizedWidth, int upsizedHeight)
    {
        int widthDiff = upsizedWidth - frame.Width;
        int heightDiff = upsizedHeight - frame.Height;
        Mat retFrame = new Mat();
        
    
        Cv2.CopyMakeBorder(frame, frame, heightDiff/2, heightDiff-(heightDiff/2), widthDiff/2, widthDiff-(widthDiff/2), BorderTypes.Isolated);
        return;
    }

    private static void performResize(Mat frame, int upsizedWidth, int upsizedHeight)
    {
        Size newsize = new Size(upsizedWidth, upsizedHeight);
        Cv2.Resize(frame,frame, newsize);

        return;
    }


    private static bool ValidateImgDim(Mat frame)
    {
        int height = frame.Height;
        int width = frame. Width;
        if(height%32 != 0 || width%32!= 0){Console.WriteLine("Image validation returned false"); return false;}
        else{Console.WriteLine("Image validation returned true"); return true;}
    }

    private static int AlignUp(int dim)
    {
        return (dim + 32 - 1)/32;
    }
    private static int AlignDown(int dim)
    {
        return dim/32;
    }
    
    
}