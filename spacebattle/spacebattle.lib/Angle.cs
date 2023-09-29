public class Angle 
{
  float X {get;};

  public Angle(float x) 
  {
      X = x;
  }

  public static Angle CalculateAngle(Angle angle1, Angle speed)
  {
      if (angle1.X + speed.X > 360) {
        return new Angle(angle1.X + speed.X - 360);
      }
    return new Angle(angle1.X + speed.X);
  }
  
}
