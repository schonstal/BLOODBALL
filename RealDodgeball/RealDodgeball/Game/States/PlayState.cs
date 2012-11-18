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
using Dodgeball.Engine;

namespace Dodgeball.Game {
  public class PlayState : GameState {
    public override void Create() {
      Sprite dot;

      for(int i = 0; i < 200; i += 2) {
        for(int j = 0; j < 200; j += 2) {
          dot = new Sprite(i, j);
          dot.loadGraphic("Dot", 10, 10);
          add(dot);
        }
      }
    }

    public override void Update() {
      base.Update();
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
