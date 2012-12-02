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
  class PauseGroup : Group {
    PauseOverlay pauseOverlay;
    Menu pauseMenu;
    Menu restartMenu;
    Card card;
    Action unPauseCallback;

    public PauseGroup(Card card, Action unPauseCallback) : base() {
      z = HUD.HUGE_Z + 1;

      this.unPauseCallback = unPauseCallback;

      pauseOverlay = new PauseOverlay();
      add(pauseOverlay);

      this.card = card;
      add(card);

      pauseMenu = new Menu(195, 148);
      pauseMenu.addMenuText(new MenuText("RESUME", UnPause));
      pauseMenu.addMenuText(new MenuText("CONTROLS", displayControls));
      pauseMenu.addMenuText(new MenuText("RESTART", restart));
      pauseMenu.addMenuText(new MenuText("QUIT", quit));
      pauseMenu.deactivate();
      add(pauseMenu);

      restartMenu = new Menu(195, 148);
      restartMenu.addMenuText(new MenuText("YES", () => {
        restartMenu.deactivate();
        card.Close();
      }));
      restartMenu.addMenuText(new MenuText("NO", () => {
        Assets.getSound("confirm").Play();
        restartMenu.deactivate();
        card.onComplete = () => {
          card.Show("paused", resume, pauseMenu.activate, 0);
        };
        card.Close();
      }));
      restartMenu.deactivate();
      add(restartMenu);
    }

    public void Pause() {
      Assets.getSound("startButton").Play();
      card.Show("paused", resume, pauseMenu.activate, 0);
      pauseOverlay.Start();
    }

    public void UnPause() {
      pauseMenu.deactivate();
      Assets.getSound("confirm").Play();
      card.Close();
      pauseOverlay.End();
    }

    void resume() {
      if(unPauseCallback != null) unPauseCallback();
    }

    void restart() {
      Assets.getSound("startButton").Play();
      pauseMenu.deactivate();
      card.onComplete = () => {
        card.Show("restart?",
          () => G.switchState(new PlayState(), "fade"),
          restartMenu.activate, 0);
      };
      card.Close();
    }

    void quit() {
      Assets.getSound("startButton").Play();
      pauseMenu.deactivate();
      card.onComplete = () => {
        card.Show("quit?", () => {
            G.DoForSeconds(0.5f, () => {
              MediaPlayer.Volume -= G.elapsed*2;
            });
            G.switchState(new MenuState(), "gate");
          }, restartMenu.activate, 0);
      };
      card.Close();
    }

    void displayControls() {
    }
  }
}
