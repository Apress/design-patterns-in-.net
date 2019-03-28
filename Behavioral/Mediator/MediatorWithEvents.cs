using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.MediatorWithEvents
{
  abstract class GameEventArgs : EventArgs
  {
    public abstract void Print();
  }

  class PlayerScoredEventArgs : GameEventArgs
  {
    public string PlayerName;
    public int GoalsScoredSoFar;

    public PlayerScoredEventArgs
      (string playerName, int goalsScoredSoFar)
    {
      PlayerName = playerName;
      GoalsScoredSoFar = goalsScoredSoFar;
    }

    public override void Print()
    {
      WriteLine($"{PlayerName} has scored! " +
                $"(their {GoalsScoredSoFar} goal)");
    }
  }

  class Game
  {
    public event EventHandler<GameEventArgs> Events;

    public void Fire(GameEventArgs args)
    {
      Events?.Invoke(this, args);
    }
  }

  class Player
  {
    private string name;
    private int goalsScored = 0;
    private Game game;

    public Player(string name, Game game)
    {
      this.name = name;
      this.game = game;
    }

    public void Score()
    {
      goalsScored++;
      var args = new PlayerScoredEventArgs(name, goalsScored);
      game.Fire(args);
    }
  }

  class Coach
  {
    private Game game;

    public Coach(Game game)
    {
      this.game = game;

      // celebrate if player has scored <3 goals
      game.Events += (sender, args) =>
      {
        if (args is PlayerScoredEventArgs scored 
            && scored.GoalsScoredSoFar < 3)
        {
          WriteLine($"coach says: well done, {scored.PlayerName}");
        }
      };
    }
  }


  class Demo
  {
    public static void Main(string[] args)
    {
      var game = new Game();
      var player = new Player("Sam", game);
      var coach = new Coach(game);

      player.Score(); // coach says: well done, Sam
      player.Score(); // coach says: well done, Sam
      player.Score(); // ignored by coach
    }
  }
}
