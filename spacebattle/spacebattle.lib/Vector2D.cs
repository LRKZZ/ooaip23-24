public class Vector2D
{
    float X {get;};
    float Y {get;};

    public Vector(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector Add(Vector vec1, Vector vec2)
    {
        return new Vector(vec1.X + vec2.X, vec1.Y + vec2.Y)
    }
}