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
  public class WinState : GameState {
    Sprite winScreen;

    public override void Create() {
      Team winningTeam = Team.Left;
      new List<Team> { Team.Left, Team.Right }.ForEach((team) => {
        if(GameTracker.RoundsWon[team] == GameTracker.RoundsToWin) {
          GameTracker.MatchesWon[team]++;
          winningTeam = team;
        }
      });

      G.playMusic("resultsMusic");
      MediaPlayer.Volume = 1f;
      winScreen = new Sprite();
      winScreen.screenPositioning = ScreenPositioning.Absolute;
      winScreen.loadGraphic("winScreen", 640, 360);
      winScreen.sheetOffset.Y = ((int)winningTeam - 1) * 360;
      add(winScreen);

      //Reset stuff in case the players want to play again
      GameTracker.CurrentRound = 0;
      GameTracker.RoundsWon[Team.Left] = 0;
      GameTracker.RoundsWon[Team.Right] = 0;
    }

    public override void Update() {
      Input.ForEachInput((index) => {
        if(G.input.JustPressed(index, Buttons.Start)) {
          G.camera.y = -400;
          fancySwitchState(new PlayState());
        } else if(G.input.JustPressed(index, Buttons.Back)) {
          fancySwitchState(new MenuState());
        }
      });
      base.Update();
    }

    //I DON'T GIVE TWO SHITS
    void fancySwitchState(GameState state) {
      DoForSeconds(0.5f, () => {
        MediaPlayer.Volume -= G.elapsed * 2;
      });
      G.switchState(state, "gate");
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
