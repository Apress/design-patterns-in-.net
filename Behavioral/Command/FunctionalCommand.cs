using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Behavioral.FunctionalCommand
{
  public class FunctionalCommand
  {
    public class BankAccount
    {
      public int Balance;
    }

    public void Deposit(BankAccount account, int amount)
    {
      account.Balance += amount;
    }

    public void Withdraw(BankAccount account, int amount)
    {
      if (account.Balance >= amount)
        account.Balance -= amount;
    }

    FunctionalCommand()
    {
      var ba = new BankAccount();
      var commands = new List<Action>();

      commands.Add(() => Deposit(ba, 100));
      commands.Add(() => Withdraw(ba, 100));
      
      commands.ForEach(c => c());
    }

    public static void Main(string[] args)
    {
      new FunctionalCommand();
    }
  }
}
