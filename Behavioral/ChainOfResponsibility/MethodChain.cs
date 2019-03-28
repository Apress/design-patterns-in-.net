using System;
using System.Security.Cryptography;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.ChainOfResponsibility.MethodChain
{
  public class Creature
  {
    public string Name;
    public int Attack, Defense;

    public Creature(string name, int attack, int defense)
    {
      Name = name;
      Attack = attack;
      Defense = defense;
    }

    public override string ToString()
    {
      return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
    }
  }

  public class CreatureModifier
  {
    protected Creature creature;
    protected CreatureModifier next;
    
    public CreatureModifier(Creature creature)
    {
      this.creature = creature;
    }

    public void Add(CreatureModifier cm)
    {
      if (next != null) next.Add(cm);
      else next = cm;
    }

    public virtual void Handle() => next?.Handle();
  }

  public class NoBonusesModifier : CreatureModifier
  {
    public NoBonusesModifier(Creature creature) 
      : base(creature) {}

    public override void Handle()
    {
      // nothing
      WriteLine("No bonuses for you!");
    }
  }

  public class DoubleAttackModifier : CreatureModifier
  {
    public DoubleAttackModifier(Creature creature) 
      : base(creature) {}

    public override void Handle()
    { 
      WriteLine($"Doubling {creature.Name}'s attack");
      creature.Attack *= 2;
      base.Handle();
    }
  }

  public class IncreaseDefenseModifier : CreatureModifier
  {
    public IncreaseDefenseModifier(Creature creature) 
      : base(creature) {}

    public override void Handle()
    {
      if (creature.Attack <= 2)
      {
        WriteLine($"Increasing {creature.Name}'s defense");
        creature.Defense++;
      }

      base.Handle();
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var goblin = new Creature("Goblin", 1, 1);
      WriteLine(goblin);
      
      var root = new CreatureModifier(goblin);

      //root.Add(new NoBonusesModifier(goblin));

      root.Add(new DoubleAttackModifier(goblin));
      root.Add(new DoubleAttackModifier(goblin));

      root.Add(new IncreaseDefenseModifier(goblin));

      // eventually...
      root.Handle();
      WriteLine(goblin);
    }
  }
}