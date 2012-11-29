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
    TimeSpan startingGameTime;
    float yDest = PlayState.ARENA_OFFSET_Y - (
        360
        - PlayState.ARENA_HEIGHT
        + HUD.SCOREBOARD_HEIGHT
        - HUD.SCOREBOARD_OFFSET
      ) / 2;

    bool started = false;
    bool timeSet = false;
    bool frozen = true;

    public override void Create() {
      //G.visualDebug = true;
      Sprite arenaBackground = new Sprite(-G.camera.x, -68 + ARENA_OFFSET_Y);
      arenaBackground.loadGraphic("arenaBackground", 640, 360);
      arenaBackground.z = -10;
      //arenaBackground.screenPositioning = ScreenPositioning.Absolute;
      add(arenaBackground);

      Sprite arenaForeground = new Sprite(-G.camera.x, -68 + ARENA_OFFSET_Y);
      arenaForeground.loadGraphic("arenaForeground", 640, 360);
      arenaForeground.z = 9999;
      //arenaForeground.screenPositioning = ScreenPositioning.Absolute;
      add(arenaForeground);

      Sprite arenaVignette = new Sprite(0, 0);
      arenaVignette.loadGraphic("arenaVignette", 640, 360);
      arenaVignette.z = 10000;
      arenaVignette.screenPositioning = ScreenPositioning.Absolute;
      add(arenaVignette);

      Ball ball = new Ball(15, 45);
      add(ball);
      balls.add(ball);
      ball = new Ball(414, 45);
      add(ball);
      balls.add(ball);
      ball = new Ball(15, 180);
      add(ball);
      balls.add(ball);
      ball = new Ball(414, 180);
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
      hud.visible = false;
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
      if(G.camera.y > yDest - 1) {
        G.camera.y = yDest;
        hud.visible = true;
        if(MediaPlayer.State != MediaState.Playing) {
          MediaPlayer.Play(Assets.getSong("GameMusic"));
        }
      } else {
        G.camera.y = MathHelper.Lerp(G.camera.y,
          yDest,
          G.elapsed * 2f);
      }

      if(!frozen) {
        countTime();
      }
      base.Update();
      started = true;
    }

    public void countTime() {
      if(!timeSet) {
        startingGameTime = G.gameTime.TotalGameTime;
        timeSet = true;
      }
      if(GameTracker.TotalSeconds <= 99) {
        GameTracker.RoundSeconds =
          GameTracker.TotalSeconds - (float)(
          G.gameTime.TotalGameTime.TotalSeconds -
          startingGameTime.TotalSeconds);
      }
      if(GameTracker.RoundSeconds <= 0) GameTracker.RoundSeconds = 0;
    }

    public override void Draw() {
      members = members.OrderBy((member) => member.z).ToList();
      base.Draw();
    }
  }
}
