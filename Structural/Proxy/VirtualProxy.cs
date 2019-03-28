using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Proxy.Virtual
{
  interface IImage
  {
    void Draw();
  }

  class Bitmap : IImage
  {
    private readonly string filename;

    public Bitmap(string filename)
    {
      this.filename = filename;
      WriteLine($"Loading image from {filename}");
    }

    public void Draw()
    {
      WriteLine($"Drawing image {filename}");
    }
  }

  class LazyBitmap : IImage
  {
    private readonly string filename;
    private Bitmap bitmap;

    public LazyBitmap(string filename)
    {
      this.filename = filename;
    }


    public void Draw()
    {
      if (bitmap == null)
        bitmap = new Bitmap(filename);
      
      bitmap.Draw();
    }
  }

  class Demo
  {
    public static void DrawImage(IImage img)
    {
      WriteLine("About to draw the image");
      img.Draw();
      WriteLine("Done drawing the image");
      
    }

    static void Main()
    {
      var img = new LazyBitmap("pokemon.png");
      DrawImage(img);
    }
  }
}
