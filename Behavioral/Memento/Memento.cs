using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Memento
{
  public class Memento
  {
    public int Balance { get; }

    public Memento(int balance)
    {
      Balance = balance;
    }
  }

  public class BankAccount
  {
    private int balance;

    public BankAccount(int balance)
    {
      this.balance = balance;
    }

    public Memento Deposit(int amount)
    {
      balance += amount;
      return new Memento(balance);
    }

    public void Restore(Memento m)
    {
      balance = m.Balance;
    }

    public override string ToString()
    {
      return $"{nameof(balance)}: {balance}";
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var ba = new BankAccount(100);
      var m1 = ba.Deposit(50);
      var m2 = ba.Deposit(25);
      WriteLine(ba); // 175

      // restore to m1
      ba.Restore(m1);
      WriteLine(ba); // 150

      // restore to m2
      ba.Restore(m2);
      WriteLine(ba); // 175
    }
  }
}