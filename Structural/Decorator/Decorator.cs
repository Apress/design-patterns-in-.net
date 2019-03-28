using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator
{
  public abstract class Shape
  {
    public virtual string AsString() => string.Empty;
  }

  public sealed class Circle : Shape
  {
    private float radius;

    public Circle() : this(0)
    {
      
    }

    public Circle(float radius)
    {
      this.radius = radius;
    }

    public void Resize(float factor)
    {
      radius *= factor;
    }

    public override string AsString() => $"A circle of radius {radius}";
  }

  public sealed class Square : Shape
  {
    private readonly float side;

    public Square() : this(0)
    {
      
    }

    public Square(float side)
    {
      this.side = side;
    }

    public override string AsString() => $"A square with side {side}";
  }

  // dynamic
  public class ColoredShape : Shape
  {
    private readonly Shape shape;
    private readonly string color;

    public ColoredShape(Shape shape, string color)
    {
      this.shape = shape;
      this.color = color;
    }

    public override string AsString() => $"{shape.AsString()} has the color {color}";
  }

  public class TransparentShape : Shape
  {
    private readonly Shape shape;
    private readonly float transparency;

    public TransparentShape(Shape shape, float transparency)
    {
      this.shape = shape;
      this.transparency = transparency;
    }

    public override string AsString() => 
      $"{shape.AsString()} has {transparency * 100.0f}% transparency";
  }

  // CRTP cannot be done
  //public class ColoredShape2<T> : T where T : Shape { }

  public class ColoredShape<T> : Shape 
    where T : Shape, new()
  {
    private readonly string color;
    private readonly T shape = new T();

    public ColoredShape() : this("black")
    {
      
    }

    public ColoredShape(string color) // no constructor forwarding
    {
      this.color = color;
    }

    public override string AsString()
    {
      return $"{shape.AsString()} has the color {color}";
    }
  }

  public class TransparentShape<T> : Shape where T : Shape, new()
  {
    private readonly float transparency;
    private readonly T shape = new T();

    public TransparentShape(float transparency)
    {
      this.transparency = transparency;
    }

    public override string AsString()
    {
      return $"{shape.AsString()} has transparency {transparency * 100.0f}";
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var circle = new Circle(2);
      WriteLine(circle.AsString());

      var redSquare = new ColoredShape(circle, "red");
      WriteLine(redSquare.AsString());

      var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);
      WriteLine(redHalfTransparentSquare.AsString());

      // static
      ColoredShape<Circle> blueCircle = new ColoredShape<Circle>("blue");
      WriteLine(blueCircle.AsString());
      // A circle of radius 0 has the color blue

      TransparentShape<ColoredShape<Square>> blackHalfSquare = new TransparentShape<ColoredShape<Square>>(0.4f);
      WriteLine(blackHalfSquare.AsString());
      // A square with side 0 has the color black has transparency 40
    }
  }
}