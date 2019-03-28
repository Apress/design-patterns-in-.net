using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Adapter.Lazy
{
  public class Point
  {
    public int X;
    public int Y;

    public Point(int x, int y)
    {
      X = x;
      Y = y;
    }

    protected bool Equals(Point other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((Point)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Y;
      }
    }

    public override string ToString()
    {
      return $"({X}, {Y})";
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

    protected bool Equals(Line other)
    {
      return Equals(Start, other.Start) && Equals(End, other.End);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((Line)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((Start != null ? Start.GetHashCode() : 0) * 397) ^ (End != null ? End.GetHashCode() : 0);
      }
    }
  }

  public abstract class VectorObject : Collection<Line>
  {
  }

  public class VectorRectangle : VectorObject
  {
    public VectorRectangle(int x, int y, int width, int height)
    {
      Add(new Line(new Point(x, y), new Point(x + width, y)));
      Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
      Add(new Line(new Point(x, y), new Point(x, y + height)));
      Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
    }
  }

  public class LineToPointAdapter
    : IEnumerable<Point>
  {
    private static int count = 0;
    static Dictionary<Line, List<Point>> cache = new Dictionary<Line, List<Point>>();
    private Line line;

    public LineToPointAdapter(Line line)
    {
      this.line = line;
      
    }

    private void Prepare()
    {
      if (cache.ContainsKey(line)) return; // we already have it

      WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}] (with caching)");

      List<Point> points = new List<Point>();

      int left = Math.Min(line.Start.X, line.End.X);
      int right = Math.Max(line.Start.X, line.End.X);
      int top = Math.Min(line.Start.Y, line.End.Y);
      int bottom = Math.Max(line.Start.Y, line.End.Y);
      int dx = right - left;
      int dy = line.End.Y - line.Start.Y;

      if (dx == 0)
      {
        for (int y = top; y <= bottom; ++y)
        {
          points.Add(new Point(left, y));
        }
      }
      else if (dy == 0)
      {
        for (int x = left; x <= right; ++x)
        {
          points.Add(new Point(x, top));
        }
      }

      cache.Add(line, points);
    }

    public IEnumerator<Point> GetEnumerator()
    {
      Prepare();
      return cache[line].GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    
  }

  public class Demo
  {
    private static readonly List<VectorObject> vectorObjects = new List<VectorObject>
    {
      new VectorRectangle(1, 1, 10, 10),
      new VectorRectangle(3, 3, 6, 6)
    };

    // the interface we have
    public static void DrawPoint(Point p)
    {
      Write(".");
    }

    public static void Main(string[] args)
    {
      int count = 0;
      for (int x1 = 397; x1 <= 400; ++x1)
      for (int y1 = 397; y1 <= 400; ++y1)
      for (int x2 = 397; x2 <= 400; ++x2)
      for (int y2 = 397; y2 <= 400; ++y2)
      {
        if (x1 == x2 && y1 == y2) continue;
        var p1 = new Point(x1, y1);
        var p2 = new Point(x2, y2);
        if (p1.GetHashCode() == p2.GetHashCode())
        {
          //WriteLine($"{p1} hash == {p2} hash == {p1.GetHashCode()}");
          count++;
        }
      }
      WriteLine($"{count} collisions");
    }

    private static void Draw()
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