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
  public class Input {
    Dictionary<PlayerIndex, GamePadState> currentState;
    public Dictionary<PlayerIndex, GamePadState> previousState;

    public Input() {
      currentState = new Dictionary<PlayerIndex, GamePadState>();
      previousState = new Dictionary<PlayerIndex, GamePadState>();
      ForEachInput((index) => {
        previousState.Add(index, GamePad.GetState(index));
        currentState.Add(index, GamePad.GetState(index));
      });
    }

    public void Update() {
      ForEachInput((index) => {
        previousState[index] = currentState[index];
        currentState[index] = GamePad.GetState(index);
      });
    }

    public bool JustPressed(PlayerIndex index, Buttons buttons) {
      return previousState[index].IsButtonUp(buttons) &&
        currentState[index].IsButtonDown(buttons);
    }

    public bool Held(PlayerIndex index, Buttons buttons) {
      return currentState[index].IsButtonDown(buttons);
    }

    public GamePadTriggers Triggers(PlayerIndex index) {
      return currentState[index].Triggers;
    }

    public GamePadThumbSticks ThumbSticks(PlayerIndex index) {
      return currentState[index].ThumbSticks;
    }

    public void ForEachInput(Action<PlayerIndex> action) {
      for(PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++) {
        action(index);
      }
    }
  }
}
