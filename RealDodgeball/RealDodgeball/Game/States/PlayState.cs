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
    float sinAmt = 0f;
    public override void Create() {
      Sprite dot;

      for(int i = 0; i < 640; i += 20) {
        for(int j = 0; j < 360; j += 20) {
          dot = new Sprite(i, j);
          dot.loadGraphic("Dot", 10, 10);
          dot.color = Color.Magenta;
          add(dot);
        }
      }
    }

    public override void Update() {
      if(G.input.JustPressed(PlayerIndex.One, Buttons.A)) {
        sinAmt += G.timeElapsed;
        G.camera.x = (float)Math.Sin(sinAmt / 100) * 50;
        G.camera.y = (float)Math.Cos(sinAmt / 100) * 50;
      }
      base.Update();
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
