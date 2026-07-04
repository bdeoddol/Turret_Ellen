namespace ByteTrackCSharp;

public class Object
{
    public Rect rect;
    public int label;
    public float prob; //conf scores

    public Object(Rect rect, int label, float prob)
    {
        this.rect = new Rect(rect);
        this.label = label;
        this.prob = prob;
    }
}