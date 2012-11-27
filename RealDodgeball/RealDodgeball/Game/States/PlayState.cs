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

    public const int ARENA_OFFSET_Y = 5;

    Group balls = new Group();
    Group players = new Group();
    HUD hud;

    bool started = false;

    public override void Create() {
      //G.visualDebug = true;
      Sprite arenaBackground = new Sprite(-G.camera.x, G.camera.y + ARENA_OFFSET_Y);
      arenaBackground.loadGraphic("arenaBackground", 640, 360);
      arenaBackground.z = -10;
      //arenaBackground.screenPositioning = ScreenPositioning.Absolute;
      add(arenaBackground);

      Sprite arenaForeground = new Sprite(-G.camera.x, G.camera.y + ARENA_OFFSET_Y);
      arenaForeground.loadGraphic("arenaForeground", 640, 360);
      arenaForeground.z = 9999;
      //arenaForeground.screenPositioning = ScreenPositioning.Absolute;
      add(arenaForeground);

      Sprite arenaVignette = new Sprite(-G.camera.x, G.camera.y + ARENA_OFFSET_Y);
      arenaVignette.loadGraphic("arenaVignette", 640, 360);
      arenaVignette.z = 10000;
      //arenaVignette.screenPositioning = ScreenPositioning.Absolute;
      add(arenaVignette);

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
      players.add(new Player(PlayerIndex.Two, Team.Left, new Vector2(0,0), CourtPosition.TopLeft));
      players.add(new Player(PlayerIndex.One, Team.Right, new Vector2(1,0), CourtPosition.TopRight));
      players.add(new Player(PlayerIndex.Three, Team.Left, new Vector2(0,1), CourtPosition.BottomLeft));
      players.add(new Player(PlayerIndex.Four, Team.Right, new Vector2(1,1), CourtPosition.BottomRight));
      players.Each((player) => add(player));

      hud = new HUD(players);
      add(hud);
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
