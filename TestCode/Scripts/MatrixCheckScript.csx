#r "nuget: OpenCvSharp4, 4.13.0.20260602"
#r "nuget: OpenCvSharp4.runtime.win, 4.13.0.20260602"
#r "nuget: OpenCvSharp4.Extensions, 4.13.0.20260602"
#r "nuget: Microsoft.ML.OnnxRuntime.Gpu, 1.26.0"
#r "nuget: Microsoft.ML.OnnxRuntime, 1.26.0"
#nullable enable
//the #r directive is used to reference external assemblies and NuGet packages
    // <PackageReference Include="Microsoft.ML.OnnxRuntime" Version="1.26.0" />
    // <PackageReference Include="Microsoft.ML.OnnxRuntime.Gpu" Version="1.26.0" />
    // <PackageReference Include="OpenCvSharp4" Version="4.13.0.20260602" />
    // <PackageReference Include="OpenCvSharp4.Extensions" Version="4.13.0.20260602" />
    // <PackageReference Include="OpenCvSharp4.runtime.win" Version="4.13.0.20260602" />


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