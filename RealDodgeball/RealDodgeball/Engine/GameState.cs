using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Dodgeball.Engine {
  public class GameState : Group {
    float totalTime = 0;
    List<Tuple<float, Action, Action>> actions = new List<Tuple<float, Action, Action>>();

    public virtual void Create() {
    }

    public override void Update() {
      totalTime += G.elapsed;
      actions.ForEach((action) => {
        if(totalTime > action.Item1) {
          if(action.Item3 != null) {
            action.Item3();
          }
          actions.Remove(action);
        } else {
          if(action.Item2 != null) action.Item2();
        }
      });
      base.Update();
    }

    public void DoForSeconds(float seconds, Action action, Action onComplete = null) {
      float endTime = totalTime + seconds;
      actions.Add(new Tuple<float, Action, Action>(endTime, action, onComplete));
    }

    public void DoInSeconds(float seconds, Action action) {
      DoForSeconds(seconds, null, action);
    }

    public class Tuple<T1, T2> {
      public T1 Item1 { get; set; }
      public T2 Item2 { get; set; }

      public Tuple(T1 item1, T2 item2) {
        Item1 = item1;
        Item2 = item2;
      }
    }

    public class Tuple<T1, T2, T3> {
      public T1 Item1 { get; set; }
      public T2 Item2 { get; set; }
      public T3 Item3 { get; set; }

      public Tuple(T1 item1, T2 item2, T3 item3) {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
      }
    } 
  }
}
