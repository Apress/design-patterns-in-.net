using System;
using DotNetDesignPatternDemos.Creational.Prototype;
using NUnit.Framework.Internal.Commands;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.PrototypeFactory
{
  public class Address
  {
    public string StreetAddress, City;
    public int Suite;

    public Address(string streetAddress, string city, int suite)
    {
      StreetAddress = streetAddress;
      City = city;
      Suite = suite;
    }

    public Address(Address other)
    {
      StreetAddress = other.StreetAddress;
      City = other.City;
      Suite = other.Suite;
    }
  }

  public partial class Person
  {
    public string Name;
    public Address Address;

    public Person(string name, Address address)
    {
      Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
      Address = address ?? throw new ArgumentNullException(paramName: nameof(address));
    }

    public Person(Person other)
    {
      Name = other.Name;
      Address = new Address(other.Address);
    }

    public override string ToString()
    {
      return $"{nameof(Name)}: {Name}, {nameof(Address)}: {Address}";
    }
    
    //partial class EmployeeFactory {}
  }

  public class EmployeeFactory
  {
    private static Person main = 
      new Person(null, new Address("123 East Dr", "London", 0));
    private static Person aux = 
      new Person(null, new Address("123B East Dr", "London", 0));
  
    private static Person NewEmployee(Person proto, string name, int suite)
    {
      var copy = proto.DeepCopy();
      copy.Name = name;
      copy.Address.Suite = suite;
      return copy;
    }
  
    public static Person NewMainOfficeEmployee(string name, int suite) =>
      NewEmployee(main, name, suite);
  
    public static Person NewAuxOfficeEmployee(string name, int suite) =>
      NewEmployee(aux, name, suite);
  }
  
  public class CopyConstructors
  {
    static Person main = new Person(null, new Address("123 East Dr", "London", 0));
    
    static void Main(string[] args)
    {
      var main = new Person(null, new Address("123 East Dr", "London", 0));
      var aux = new Person(null, new Address("123B East Dr", "London", 0));
      
      
      var john = new Person("John", new Address("123 London Road", "London", 123));

      //var chris = john;
      var jane = new Person(john);

      jane.Name = "Jane";
      
      WriteLine(john); // oops, john is called chris
      WriteLine(jane);


    }
  }
}
