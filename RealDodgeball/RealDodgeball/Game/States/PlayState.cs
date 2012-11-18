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
    Sprite box;

    public override void Create() {
      dot = new Sprite(0,0);
      dot.loadGraphic("Dot", 512, 288);
      dot.color = Color.DarkCyan;
      add(dot);

      box = new Sprite(0,0);
      box.loadGraphic("Dot", 24, 24);
      box.color = Color.DarkMagenta;
      //box.acceleration.Y = 10f;
      add(box);
    }

    public override void Update() {
      if(G.input.Held(PlayerIndex.One, Buttons.A)) {
      }
//      dot.velocity.X = G.input.ThumbSticks(PlayerIndex.One).Left.X;
//      dot.velocity.Y = -G.input.ThumbSticks(PlayerIndex.One).Left.Y;
      base.Update();
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
