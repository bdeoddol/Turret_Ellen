using System.Dynamic;
using System.Security.Cryptography;

public class Detection(float x1a, float y1a, float x2a, float y2a, float confa, float classIDa, int detIDa = -1)
{

        public float x1{ get; set;} = x1a;
        public float y1{get; set;} = y1a;
        public float x2{get; set;} = x2a;
        public float y2{get; set;} = y2a;
        public float conf{get; set;} = confa;
        public float classID{get; set;} = classIDa;
        public int detID{get; set;} = detIDa; //track IDs set to -1 by default, assigned by BYTETRACK
        public int width => (int)(x2-x1);
        public int height => (int)(y2-y1);
        public OpenCvSharp.Point boxCenter => new OpenCvSharp.Point((x1+x2)/2, (y1+y2)/2);
}
