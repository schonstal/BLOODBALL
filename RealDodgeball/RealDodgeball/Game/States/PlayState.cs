﻿using System;
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
    public const float START_DELAY = 0.25f;
  
    public const int ARENA_OFFSET_Y = 5;

    public State state = State.Panning;
    public Dictionary<Team, Group> teamPlayers = new Dictionary<Team, Group>();

    Group balls = new Group();
    Group players = new Group();
    List<Team> teams = new List<Team> { Team.Left, Team.Right }; //Xbox BS (enums not enumerable)

    HUD hud;
    TimeSpan startingGameTime;
    Card card;
    PauseGroup pauseGroup;

    float yDest = PlayState.ARENA_OFFSET_Y - (
        360
        - PlayState.ARENA_HEIGHT
        + HUD.SCOREBOARD_HEIGHT
        - HUD.SCOREBOARD_OFFSET
      ) / 2;

    bool started = false;
    bool timeSet = false;
    bool restarted = false;
    bool paused = false;
    bool pausable = false;

    public PlayState(bool restart=false) : base() {
      restarted = restart;
    }

    public override void Create() {
      G.playMusic("GameMusic");
      //G.visualDebug = true;
      Sprite arenaBackground = new Sprite(-G.camera.x, -68 + ARENA_OFFSET_Y - 280);
      arenaBackground.loadGraphic("arenaBackground", 640, 640);
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

      Ball ball = new Ball();
      ball = new Ball(ARENA_WIDTH/2 - 30, ARENA_HEIGHT/2 - 30);
      add(ball);
      balls.add(ball);
      ball = new Ball(ARENA_WIDTH/2 + 20, ARENA_HEIGHT/2 - 30);
      add(ball);
      balls.add(ball);
      ball = new Ball(ARENA_WIDTH/2 - 30, ARENA_HEIGHT/2 + 15);
      add(ball);
      balls.add(ball);
      ball = new Ball(ARENA_WIDTH/2 + 20, ARENA_HEIGHT/2 + 15);
      add(ball);
      balls.add(ball);

      balls.Each((b) => {
        b.addOnMoveCallback(onBallMove);
      });

      teamPlayers.Add(Team.Left, new Group());
      teamPlayers.Add(Team.Right, new Group());

      //probably want to let people pick their team later
      teamPlayers[Team.Left].add(
        new Player(GameTracker.LeftPlayers[0], Team.Left, new Vector2(0, 0), CourtPosition.TopLeft));
      teamPlayers[Team.Left].add(
        new Player(GameTracker.LeftPlayers[1], Team.Left, new Vector2(0, 1), CourtPosition.BottomLeft));

      teamPlayers[Team.Right].add(
        new Player(GameTracker.RightPlayers[0], Team.Right, new Vector2(1, 0), CourtPosition.TopRight));
      teamPlayers[Team.Right].add(
        new Player(GameTracker.RightPlayers[1], Team.Right, new Vector2(1, 1), CourtPosition.BottomRight));

      teamPlayers[Team.Left].Each((player) => players.add(player));
      teamPlayers[Team.Right].Each((player) => players.add(player));
      players.Each((player) => add(player));

      hud = new HUD(players);
      hud.visible = false;
      add(hud);

      card = new Card();
      add(card);

      pauseGroup = new PauseGroup(card, onUnPause);
      add(pauseGroup);

      GameTracker.RoundSeconds = GameTracker.TotalSeconds;
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

    void onUnPause() {
      paused = false;
    }

    public override void Update() {
      if(paused) {
        pauseGroup.Update();
        return;
      }
      if(G.camera.y > yDest - 1 || restarted) {
        if(state == State.Panning) {
          G.camera.y = yDest;
          hud.visible = true;
          G.DoInSeconds(0.3f, () => {
            card.Show(
              (GameTracker.RoundsWon[Team.Left] == GameTracker.RoundsToWin - 1 &&
              GameTracker.RoundsWon[Team.Right] == GameTracker.RoundsToWin - 1) ? "final round" : "round",
              () => DoInSeconds(START_DELAY, () => card.Show("start",
                  () => pausable = true,
                  () => state = State.Playing)
                ));
          });
          state = State.GetReady;
        } else if(state == State.GetReady) {
        } else if(state == State.Playing) {
          countTime();
          teams.ForEach((team) => {
            if(teamPlayers[team].Members.All((player) => ((Player)player).HP <= 0)) {
              pausable = false;
              G.timeScale = 0.2f;
              state = State.KO;
              G.DoInSeconds(1.25f, () => {
                Team otherTeam = team == Team.Left ? Team.Right : Team.Left;
                bool otherTeamDead = teamPlayers[otherTeam].Members.All(
                  (player) => ((Player)player).HP <= 0);

                if(otherTeamDead) {
                  card.Show("double ko", () => {
                    GameTracker.CurrentRound++;
                    if(!(GameTracker.GamePoint(Team.Right) && GameTracker.GamePoint(Team.Left))) {
                      GameTracker.RoundsWon[Team.Left]++;
                      GameTracker.RoundsWon[Team.Right]++;
                    }
                    G.switchState(new PlayState(), "fade");
                  });
                } else {
                  card.Show("ko", () => {
                    GameTracker.CurrentRound++;
                    GameTracker.RoundsWon[team == Team.Left ? Team.Right : Team.Left]++;
                    if(GameTracker.TeamWon(Team.Left) || GameTracker.TeamWon(Team.Right)) {
                      DoInSeconds(0.2f, () => {
                        card.Show(GameTracker.TeamWon(Team.Left) ? "magenta wins" : "cyan wins", () => {
                          G.switchState(new WinState(), "gate");

                          DoForSeconds(0.5f, () => {
                            MediaPlayer.Volume -= G.elapsed;
                          });
                        }, null, 1.5f);
                      });
                    } else {
                      G.switchState(new PlayState(), "fade");
                    }
                  });
                }
              });
            }
          });

          Input.ForEachInput((index) => {
            if(G.input.JustPressed(index, Buttons.Start)) {
              if(pausable) {
                G.keyMaster = index;
                pauseGroup.Pause();
                paused = true;
              }
            }
          });
        } else if(state == State.KO) {
          G.timeScale += G.elapsed * 1.5f;
          if(G.timeScale >= 1) G.timeScale = 1;
        } else if(state == State.Paused) {
        }
      } else {
        G.camera.y = MathHelper.Lerp(G.camera.y,
          yDest,
          G.elapsed * 2f);
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

  public enum State {
    Panning,
    GetReady,
    Playing,
    KO,
    Paused
  }
}
