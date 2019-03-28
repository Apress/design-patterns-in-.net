namespace DotNetDesignPatternDemos.Behavioral.Visitor
{
  interface IStuff { }
  class Foo : IStuff { }
  class Bar : IStuff { }
  
  public class Something
  {
    static void func(Foo foo) { }
    static void func(Bar bar) { }
  
    static void Main(string[] args)
    {
      IStuff i = new Foo();
      func((dynamic)i); // cannot resolve w/o (dynamic)
    }
  }
}