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
    Card card;
    Action unPauseCallback;

    public PauseGroup(Card card, Action unPauseCallback) : base() {
      z = HUD.HUGE_Z + 1;

      this.unPauseCallback = unPauseCallback;

      pauseOverlay = new PauseOverlay();
      add(pauseOverlay);

      this.card = card;
      add(card);

      pauseMenu = new Menu(180, 148);
      pauseMenu.addMenuText(new MenuText("RESUME", UnPause));
      pauseMenu.addMenuText(new MenuText("CONTROLS"));
      pauseMenu.addMenuText(new MenuText("RESTART"));
      pauseMenu.addMenuText(new MenuText("QUIT"));
      pauseMenu.deactivate();
      add(pauseMenu);
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
  }
}
