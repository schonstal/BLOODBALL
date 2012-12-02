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
    Sprite controls;

    bool layerIn = false;
    bool open = false;

    public PauseGroup(Card card, Action unPauseCallback) : base() {
      z = HUD.HUGE_Z + 1;

      this.unPauseCallback = unPauseCallback;

      pauseOverlay = new PauseOverlay();
      add(pauseOverlay);

      this.card = card;
      add(card);

      controls = new Sprite(190, 148);
      controls.loadGraphic("controls", 251, 125);
      controls.visible = false;
      controls.screenPositioning = ScreenPositioning.Absolute;
      add(controls);

      pauseMenu = new Menu(195, 148);
      pauseMenu.addMenuText(new MenuText("RESUME", UnPause));
      pauseMenu.addMenuText(new MenuText("CONTROLS", displayControls));
      pauseMenu.addMenuText(new MenuText("RESTART", restart));
      pauseMenu.addMenuText(new MenuText("QUIT", quit));
      pauseMenu.deactivate();
      add(pauseMenu);

      restartMenu = new Menu(195, 148);
      restartMenu.addMenuText(new MenuText("YES", () => {
        //Assets.getSound("superKO").Play();
        Assets.getSound("startButton").Play();
        restartMenu.deactivate();
        card.Close();
      }));
      restartMenu.addMenuText(new MenuText("NO", goBack));
      restartMenu.deactivate();
      add(restartMenu);
    }

    public void Pause() {
      Assets.getSound("startButton").Play();
      card.Show("paused", resume, () => {
        pauseMenu.activate();
        open = true;
      }, 0);
      pauseOverlay.Start();
    }

    public void UnPause() {
      pauseMenu.reset();
      restartMenu.reset();
      pauseMenu.deactivate();
      restartMenu.deactivate();
      card.onComplete = resume;
      Assets.getSound("confirm").Play();
      card.Close();
      pauseOverlay.End();
      open = false;
      layerIn = false;
      controls.visible = false;
    }

    void resume() {
      if(unPauseCallback != null) unPauseCallback();
    }

    void restart() {
      goDeep();
      card.onComplete = () => {
        card.Show("restart?",
          () => G.switchState(new PlayState(), "fade"),
          () => {
            restartMenu.activate();
            restartMenu.reset();
          }, 0);
      };
      card.Close();
    }

    void quit() {
      goDeep();
      card.onComplete = () => {
        card.Show("quit?", () => {
          G.DoForSeconds(0.5f, () => {
            MediaPlayer.Volume -= G.elapsed * 2;
          });
          G.switchState(new MenuState(), "gate");
        }, () => {
          restartMenu.activate();
          restartMenu.reset();
        }, 0);
      };
      card.Close();
    }

    void displayControls() {
      goDeep();
      card.onComplete = () => {
        card.Show("controls", () => {
          G.DoForSeconds(0.5f, () => {
            MediaPlayer.Volume -= G.elapsed * 2;
          });
          G.switchState(new MenuState(), "gate");
        }, () => {
          controls.visible = true;
        }, 0);
      };
      card.Close();
    }

    void goDeep() {
      layerIn = true;
      Assets.getSound("startButton").Play();
      pauseMenu.deactivate();
    }

    void goBack() {
      Assets.getSound("confirm").Play();
      restartMenu.deactivate();
      controls.visible = false;
      card.onComplete = () => {
        card.Show("paused", resume, pauseMenu.activate, 0);
      };
      card.Close();
      layerIn = false;
    }

    public override void Update() {
      if(open && G.input.JustPressed(G.keyMaster, Buttons.Start)) {
        UnPause();
      }
      if(layerIn && (G.input.JustPressed(G.keyMaster, Buttons.B) ||
          G.input.JustPressed(G.keyMaster, Buttons.Back)) ||
          (controls.visible && G.input.JustPressed(G.keyMaster, Buttons.A))) {
        goBack();
      }
      base.Update();
    }
  }
}
