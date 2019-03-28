using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.AccessControl;
using MoreLinq;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Adapter.NoCaching
{
  public class Point
  {
    public int X, Y;

    public Point(int x, int y)
    {
      X = x;
      Y = y;
    }

    public override string ToString()
    {
      return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
    }
  }

  public class Line
  {
    public Point Start, End;

    public Line(Point start, Point end)
    {
      Start = start;
      End = end;
    }
  }

  public abstract class VectorObject 
    : Collection<Line> {}

  public class VectorRectangle : VectorObject
  {
    public VectorRectangle(int x, int y, int width, int height)
    {
      Add(new Line(new Point(x,y), new Point(x+width, y) ));
      Add(new Line(new Point(x+width,y), new Point(x+width, y+height) ));
      Add(new Line(new Point(x,y), new Point(x, y+height) ));
      Add(new Line(new Point(x,y+height), new Point(x+width, y+height) ));
    }
  }

  public class LineToPointAdapter : Collection<Point>
  {
    private static int count = 0;

    public LineToPointAdapter(Line line)
    {
      WriteLine($"{++count}: Generating points for line" 
        + $" [{line.Start.X},{line.Start.Y}]-" 
        + $"[{line.End.X},{line.End.Y}] (no caching)");

      int left = Math.Min(line.Start.X, line.End.X);
      int right = Math.Max(line.Start.X, line.End.X);
      int top = Math.Min(line.Start.Y, line.End.Y);
      int bottom = Math.Max(line.Start.Y, line.End.Y);

      if (right - left == 0)
      {
        for (int y = top; y <= bottom; ++y)
        {
          Add(new Point(left, y));
        }
      } else if (line.End.Y - line.Start.Y == 0)
      {
        for (int x = left; x <= right; ++x)
        {
          Add(new Point(x, top));
        }
      }
    }
  }

  public class Demo
  {
    private static readonly List<VectorObject> vectorObjects 
      = new List<VectorObject>
    {
      new VectorRectangle(1, 1, 10, 10),
      new VectorRectangle(3, 3, 6, 6)
    };

    // the interface we have
    public static void DrawPoint(Point p)
    {
      Write(".");
    }

    static void Main()
    {
      DrawPoints();
      DrawPoints();
    }

    private static List<Point> points = new List<Point>();
    private static bool prepared = false;

    private static void Prepare()
    {
      if (prepared) return;
      foreach (var vo in vectorObjects)
      {
        foreach (var line in vo)
        {
          var adapter = new LineToPointAdapter(line);
          adapter.ForEach(p => points.Add(p));
        }
      }
      prepared = true;
    }

    private static void DrawPointsLazy()
    {
      Prepare();
      points.ForEach(DrawPoint);
    }

    private static void DrawPoints()
    {
      foreach (var vo in vectorObjects)
      {
        foreach (var line in vo)
        {
          var adapter = new LineToPointAdapter(line);
          adapter.ForEach(DrawPoint);
        }
      }
    }
  }
}