using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// todo: fix namespace
namespace DotNetDesignPatternDemos.Behavioral.Command
{
  public class BankAccount
  {
    private int balance;
    private int overdraftLimit = -500;

    public void Deposit(int amount)
    {
      balance += amount;
      Console.WriteLine($"Deposited ${amount}, balance is now {balance}");
    }

    public bool Withdraw(int amount)
    {
      if (balance - amount >= overdraftLimit)
      {
        balance -= amount;
        Console.WriteLine($"Withdrew ${amount}, balance is now {balance}");
        return true;
      }
      return false;
    }

    public override string ToString()
    {
      return $"{nameof(balance)}: {balance}";
    }
  }

  public interface ICommand
  {
    void Call();
    void Undo();
  }

  public class BankAccountCommand : ICommand
  {
    private BankAccount account;

    public enum Action
    {
      Deposit, Withdraw
    }

    private Action action;
    private int amount;
    private bool succeeded;

    public BankAccountCommand(BankAccount account, Action action, int amount)
    {
      this.account = account;
      this.action = action;
      this.amount = amount;
    }

    public void Call()
    {
      switch (action)
      {
        case Action.Deposit:
          account.Deposit(amount);
          succeeded = true;
          break;
        case Action.Withdraw:
          succeeded = account.Withdraw(amount);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void Undo()
    {
      if (!succeeded) return;
      switch (action)
      {
        case Action.Deposit:
          account.Withdraw(amount);
          break;
        case Action.Withdraw:
          account.Deposit(amount);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  class CompositeBankAccountCommand : List<BankAccountCommand>, ICommand
  {
    public void Call()
    {
      ForEach(cmd => cmd.Call());
    }

    public void Undo()
    {
      foreach (var cmd in 
        ((IEnumerable<BankAccountCommand>)this).Reverse())
      {
        cmd.Undo();
      }
    }
  }

  class Demo
  {
    static void Main(string[] args)
    {
      var ba = new BankAccount();
      var cmd = new BankAccountCommand(ba,
        BankAccountCommand.Action.Deposit, 100);
      cmd.Call();
      Console.WriteLine(ba);
      
      
      
      var commands = new List<BankAccountCommand>
      {
        new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
        new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 1000)
      };

      Console.WriteLine(ba);

      foreach (var c in commands)
        c.Call();

      Console.WriteLine(ba);

      foreach (var c in Enumerable.Reverse(commands))
        c.Undo();

      Console.WriteLine(ba);
    }
  }
}