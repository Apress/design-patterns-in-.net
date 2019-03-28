using System;
using System.Text;
using NUnit.Framework;

namespace DotNetDesignPatternDemos.Behavioral.Memento
{
  namespace Coding.Exercise
  {
    public class MyString
    {
      private StringBuilder sb = new StringBuilder();
  
      public MyString(string s)
      {
        sb.Append(s);
      }
  
      public StringRange GetRange(int start, int length)
      {
        return new StringRange(this, start, length);
      }
  
      public override string ToString()
      {
        return sb.ToString();
      }
  
      public class StringRange
      {
        private MyString str;
        private int start, length;
  
        public StringRange(MyString str, int start, int length)
        {
          this.str = str;
          this.start = start;
          this.length = length;
        }
  
        public StringRange Set(string s)
        {
          str.sb.Remove(start, length);
          str.sb.Insert(start, s);
          length = s.Length;
  
          return this;
        }
      }
    }
  
  
  }
  namespace Coding.Exercise.Tests
  {
    [TestFixture]
    public class Tests
    {
      [Test]
      public void WipeoutTest()
      {
        var s = new MyString("notation");
        s.GetRange(0, 8).Set("");
        Assert.That(s.ToString(), Is.Empty);
      }
      
      [Test]
      public void SimpleTest()
      {
        var s = new MyString("notation");
        s.GetRange(2,2).Set("");
        Assert.That(s.ToString(), Is.EqualTo("notion"));
      }
  
      [Test]
      public void DoubleOverwriteTest()
      {
        var s = new MyString("notation");
        var range = s.GetRange(2, 2).Set("");
        range.Set("tifica");
        Assert.That(s.ToString(), Is.EqualTo("notification"));
      }
    }
  }
}