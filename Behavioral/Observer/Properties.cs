using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DotNetDesignPatternDemos.Annotations;
using MoreLinq;

namespace DotNetDesignPatternDemos.Behavioral.Observer.Properties
{
  class PropertyNotificationSupport : INotifyPropertyChanged
  {
    private readonly Dictionary<string, HashSet<String>> affectedBy
      = new Dictionary<string, HashSet<string>>();

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged
      ([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

      foreach (var affector in affectedBy.Keys)
        if (affectedBy[affector].Contains(propertyName))
          OnPropertyChanged(propertyName);
    }
    
    protected Func<T> property<T>(string name, Expression<Func<T>> expr)
    {
      Console.WriteLine($"Creating computed property for expression {expr}");

      var visitor = new MemberAccessVisitor(GetType());
      visitor.Visit(expr);

      if (visitor.PropertyNames.Any())
      {
        if (!affectedBy.ContainsKey(name))
          affectedBy.Add(name, new HashSet<string>());

        foreach (var propName in visitor.PropertyNames)
          if (propName != name)
            affectedBy[name].Add(propName);
      }

      return expr.Compile();
    }

    private class MemberAccessVisitor : ExpressionVisitor
    {
      private readonly Type declaringType;
      public IList<string> PropertyNames = new List<string>();

      public MemberAccessVisitor(Type declaringType)
      {
        this.declaringType = declaringType;
      }

      public override Expression Visit(Expression expr)
      {
        if (expr != null && expr.NodeType == ExpressionType.MemberAccess)
        {
          var memberExpr = (MemberExpression)expr;
          if (memberExpr.Member.DeclaringType == declaringType)
          {
            PropertyNames.Add(memberExpr.Member.Name);
          }
        }

        return base.Visit(expr);
      }
    }
  }

  class Person : PropertyNotificationSupport
  {
    private int age;

    public int Age
    {
      get => age;
      set
      {
        //var oldCanVote = CanVote;

        if (value == age) return;
        age = value;
        OnPropertyChanged();

        //if (oldCanVote != CanVote)
        //  OnPropertyChanged(nameof(CanVote));
      }
    }

    private readonly Func<bool> canVote;
    public bool CanVote => canVote();

    public Person()
    {
      canVote = property(nameof(CanVote), () => Age >= 16);
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var p = new Person();
      p.PropertyChanged += (sender, eventArgs) =>
      {
        Console.WriteLine($"{eventArgs.PropertyName} changed");
      };
      p.Age = 15;
      p.Age++;
    }
  }
}