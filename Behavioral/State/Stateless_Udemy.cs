using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Stateless;
using static System.Console;

namespace DesignPatterns.Behavioral.State.Stateless
{
  public enum Health
  {
    NonReproductive,
    Pregnant,
    Reproductive
  }

  public enum Activity
  {
    GiveBirth,
    ReachPuberty,
    HaveAbortion,
    HaveUnprotectedSex,
    Historectomy
  }

  class Demo
  { 
    static void Main(string[] args)
    {
      var stateMachine = new StateMachine<Health, Activity>(Health.NonReproductive);
      stateMachine.Configure(Health.NonReproductive)
        .Permit(Activity.ReachPuberty, Health.Reproductive);
      stateMachine.Configure(Health.Reproductive)
        .Permit(Activity.Historectomy, Health.NonReproductive)
        .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
          () => ParentsNotWatching);
      stateMachine.Configure(Health.Pregnant)
        .Permit(Activity.GiveBirth, Health.Reproductive)
        .Permit(Activity.HaveAbortion, Health.Reproductive);

    }

    public static bool ParentsNotWatching { get; set; }
  }
}