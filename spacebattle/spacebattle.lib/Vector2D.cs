public class Vector2D
{
    float X {get;}
    float Y {get;}

    public Vector2D(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector2D Add(Vector2D vec1, Vector2D vec2)
    {
        return new Vector2D(vec1.X + vec2.X, vec1.Y + vec2.Y);
    }
}