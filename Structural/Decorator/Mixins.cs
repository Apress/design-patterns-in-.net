namespace DotNetDesignPatternDemos.Structural.Decorator.Mixins
{
  // not recommended in production code!

  public interface IManagement
  {
    void GiveBonusTo(Person p);
  }

  public class Person
  {
    public string Name { get; set; }
  }

  public class Manager : Person, IManagement
  {
    public void GiveBonusTo(Person p)
    {

    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {

    }
  }
}