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
    public const int ARENA_WIDTH = 448;
    public const int ARENA_HEIGHT = 252;
    Sprite dot;

    Group balls = new Group();
    Group players = new Group();

    bool started = false;

    public override void Create() {
      //G.visualDebug = true;

      dot = new Sprite(0,0);
      dot.loadGraphic("Dot", ARENA_WIDTH, ARENA_HEIGHT);
      dot.color = new Color(0x2c,0x2c,0x2c);
      dot.z = -1;
      add(dot);

      dot = new Sprite(-100, ARENA_HEIGHT);
      dot.loadGraphic("Dot", 1000, ARENA_HEIGHT);
      dot.color = Color.Black;
      dot.z = 100000;
      add(dot);

      Ball ball = new Ball(80, 80);
      add(ball);
      balls.add(ball);
      ball = new Ball(80, 120);
      add(ball);
      balls.add(ball);
      ball = new Ball(ARENA_WIDTH - 80 - ball.width, 80);
      add(ball);
      balls.add(ball);
      ball = new Ball(ARENA_WIDTH - 80 - ball.width, 120);
      add(ball);
      balls.add(ball);

      balls.Each((b) => {
        b.addOnMoveCallback(onBallMove);
      });


      //probably want to let people pick their team later
      players.add(new Player(PlayerIndex.Two, Team.Left, new Vector2(0,0), 80, 40));
      players.add(new Player(PlayerIndex.One, Team.Right, new Vector2(1,0), ARENA_WIDTH - 80, 40));
      players.add(new Player(PlayerIndex.Three, Team.Left, new Vector2(0,0), 80, ARENA_HEIGHT - 100));
      players.add(new Player(PlayerIndex.Four, Team.Right, new Vector2(1,0), ARENA_WIDTH - 80, ARENA_HEIGHT - 100));
      players.Each((player) => add(player));
    }

    void onBallMove(GameObject ball) {
      if(started) {
        players.Each((player) => {
          if(Util.Overlaps(((Player)player).shadow, ((Ball)ball).shadow)) {
            bool playerWasDead = ((Player)player).Dead;
            ((Player)player).onCollide((Ball)ball);
            ((Ball)ball).onCollide((Player)player, playerWasDead);
            //ball.visible = false;
          }
        });
      }
    }

    public override void Update() {
      base.Update();
      started = true;
    }

    public override void Draw() {
      members = members.OrderBy((member) => member.z).ToList();
      base.Draw();
    }
  }
}
