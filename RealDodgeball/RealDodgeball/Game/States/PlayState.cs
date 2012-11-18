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
    Sprite dot;

    public override void Create() {
      dot = new Sprite(0,0);
      dot.loadGraphic("Dot", 512, 288);
      dot.color = Color.DarkCyan;
      add(dot);

    }

    public override void Update() {
      if(G.input.Held(PlayerIndex.One, Buttons.A)) {
        G.camera.x += G.timeElapsed * G.input.ThumbSticks(PlayerIndex.One).Left.X;
        G.camera.y -= G.timeElapsed * G.input.ThumbSticks(PlayerIndex.One).Left.Y;
      }
      base.Update();
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
