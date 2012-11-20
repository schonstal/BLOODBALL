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
    public const int ARENA_WIDTH = 512;
    public const int ARENA_HEIGHT = 288;
    Sprite dot;
    Ball ball;

    Player player;

    public override void Create() {
      G.visualDebug = true;

      dot = new Sprite(0,0);
      dot.loadGraphic("Dot", ARENA_WIDTH, ARENA_HEIGHT);
      dot.color = Color.DarkCyan;
      add(dot);

      dot = new Sprite(1, 1);
      dot.loadGraphic("Dot", ARENA_WIDTH-2, ARENA_HEIGHT-2);
      dot.color = Color.Black;
      add(dot);

      ball = new Ball();
      ball.x = 40;
      ball.y = 40;
      add(ball);

      player = new Player(PlayerIndex.One, Team.Left);
      add(player);
    }

    public override void Update() {
      if(G.input.Held(PlayerIndex.One, Buttons.A)) {
      }

      if(player.Hitbox.Intersects(ball.Hitbox)) {
        remove(ball);
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
