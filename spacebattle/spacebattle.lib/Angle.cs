public class Angle 
{
  float X { get; }

  public Angle(float x) 
  {
    X = x;
  }

  // необходимо проработать случаи, когда угловая скорость принимает отрицательные значения

  public static Angle CalculateAngle(Angle angle1, Angle speed)
  {
      if (angle1.X + speed.X > 360) 
      {
        return new Angle(angle1.X + speed.X - 360);
      }

    return new Angle(angle1.X + speed.X);
  }
  
}
