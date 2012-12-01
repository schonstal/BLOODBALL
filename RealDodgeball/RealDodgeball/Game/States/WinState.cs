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
    Sprite arenaVignette;

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

      arenaVignette = new Sprite(0, 0);
      arenaVignette.loadGraphic("arenaVignette", 640, 360);
      arenaVignette.z = 10000;
      arenaVignette.screenPositioning = ScreenPositioning.Absolute;
      add(arenaVignette);

      //Reset stuff in case the players want to play again
      GameTracker.CurrentRound = 0;
      GameTracker.RoundsWon[Team.Left] = 0;
      GameTracker.RoundsWon[Team.Right] = 0;

      int tallies = (int)Math.Ceiling(GameTracker.MatchesWon[Team.Left]/5f);
      Sprite tally;
      for(int i = 0; i < tallies; i++) {
        tally = new Sprite();
        tally.loadGraphic("tallies", 48, 30);
        tally.screenPositioning = ScreenPositioning.Absolute;
        tally.sheetOffset.Y = i == tallies - 1 && GameTracker.MatchesWon[Team.Left] % 5 != 0 ? 
          (GameTracker.MatchesWon[Team.Left]%5 - 1) * tally.height :
          120;
        tally.y = 108 + (38 * i);
        tally.x = winningTeam == Team.Left ? 346 : 152;
        tally.color = winningTeam == Team.Left ?
          //new Color(0x92, 0xca, 0xd9) :
          new Color(0x8c, 0xbe, 0xcd) :
          new Color(0xb9, 0x8b, 0xce);
        add(tally);
      }

      tallies = (int)Math.Ceiling(GameTracker.MatchesWon[Team.Right] / 5f);
      for(int i = 0; i < tallies; i++) {
        tally = new Sprite();
        tally.loadGraphic("tallies", 48, 30);
        tally.screenPositioning = ScreenPositioning.Absolute;
        tally.sheetOffset.Y = i == tallies - 1 && GameTracker.MatchesWon[Team.Right] % 5 != 0 ?
          ((GameTracker.MatchesWon[Team.Right] % 5) - 1) * tally.height :
          120;
        tally.y = 108 + (38 * i);
        tally.x = winningTeam == Team.Left ? 432 : 238;
        tally.color = winningTeam == Team.Left ?
          new Color(0xc6, 0x79, 0xcd) :
          new Color(0x70, 0xc7, 0xce);
        add(tally);
      }
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
