using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using OpenCvSharp;

class ImgResizev1 
{
    public static void Run(string imgPath)
    {
        Console.WriteLine("ImgResizev1");
        Mat frame = Cv2.ImRead(imgPath);
        if(ValidateImgDim(frame) == false)
        {Console.WriteLine("Image validation returned false");}
        else{Console.WriteLine("Image validation returned true");}
   
        int aUWidth = AlignUp(frame.Width)*32;
        int aUHeight = AlignUp(frame.Height)*32;
        int aDWidth = AlignDown(frame.Width)*32;
        int aDHeight = AlignDown(frame.Height)*32;
        Console.WriteLine("Original dims: " + frame.Width + "x" + frame.Height);
        Console.WriteLine("Aligned up dims: " + aUWidth  + "x" + aUHeight); //target resize
        Console.WriteLine("Aligned down dims: " + aDWidth + "x" + aDHeight);

    
        //Letterboxing
        performResize(frame, aUWidth, frame.Height*(aUWidth/frame.Width));
        performPaddingVert(frame, aUHeight);
        
        Console.WriteLine("Final Dims: " + frame.Width + "x" + frame.Height);
        Cv2.ImWrite("imrs-frame.jpg", frame);
        return;
    }

    public static Rect GetRectOfOriginalFrame(Mat frame)
    {
        int aUWidth = AlignUp(frame.Width)*32;
        int aUHeight = AlignUp(frame.Height)*32;
        Rect retRect = new Rect(aUWidth-frame.Width,aUHeight-frame.Height, frame.Width,frame.Height-(aUHeight-frame.Height) );
        return retRect;
    }

    public static void performPaddingVert(Mat frame, int targetHeight)
    {
        int heightDiff = targetHeight - frame.Height;        
        Cv2.CopyMakeBorder(frame, frame, heightDiff/2, heightDiff-(heightDiff/2),0, 0, BorderTypes.Isolated);
        return;
    }

    public static void performPaddingHori(Mat frame, int targetWidth)
    {
        int widthDiff = targetWidth - frame.Width;
        Cv2.CopyMakeBorder(frame, frame, 0, 0, widthDiff/2, widthDiff-(widthDiff/2), BorderTypes.Isolated);
        return;
    }

    public static void performResize(Mat frame, int targetWidth, int targetHeight)
    {
        Size newsize = new Size(targetWidth, targetHeight);
        Cv2.Resize(frame,frame, newsize);
        return;
    }


    public static bool ValidateImgDim(Mat frame)
    {
        int height = frame.Height;
        int width = frame. Width;
        if(height%32 != 0 || width%32!= 0){return false;}
        else{return true;}
    }

    public static int AlignUp(int dim)
    {
        return (dim + 32 - 1)/32;
    }
    public static int AlignDown(int dim)
    {
        return dim/32;
    }
    
    
}