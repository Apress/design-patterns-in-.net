using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDesignPatternDemos.Behavioral.Iterator.ArrayBackedProperties
{
  public class Creature : IEnumerable<int>
  {
    private int [] stats = new int[3];

    public IEnumerable<int> Stats => stats;

    private const int strength = 0;

    public int Strength
    {
      get => stats[strength];
      set => stats[strength] = value;
    }

    public int Agility { get; set; }
    public int Intelligence { get; set; }



    public double AverageStat => stats.Average();

    //public double AverageStat => SumOfStats / 3.0;

    //public double SumOfStats => Strength + Agility + Intelligence;
    public double SumOfStats => stats.Sum();

    //public double MaxStat => Math.Max(
    //  Math.Max(Strength, Agility), Intelligence);

    public double MaxStat => stats.Max();

    public IEnumerator<int> GetEnumerator()
    {
      return stats.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int this[int index]
    {
      get => stats[index];
      set => stats[index] = value;
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var creature = new Creature();
      creature.Strength = 10;
      creature.Intelligence = 11;
      creature.Agility = 12;
      Console.WriteLine($"Creature has average stat = {creature.AverageStat}, " +
                        $"max stat = {creature.MaxStat}, " +
                        $"sum of stats = {creature.SumOfStats}.");
    }
  }
}