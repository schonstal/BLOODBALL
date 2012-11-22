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
          action.Item2();
        }
      });
      base.Update();
    }

    public void DoForSeconds(float seconds, Action action, Action onComplete = null) {
      float endTime = totalTime + seconds;
      actions.Add(new Tuple<float, Action, Action>(endTime, action, onComplete));
    }
  }
}
