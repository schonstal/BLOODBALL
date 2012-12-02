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
  //Boilerplate for state
  public class BadingState : GameState {
    Sprite logo;

    public override void Create() {
      logo = new Sprite();
      logo.screenPositioning = ScreenPositioning.Absolute;
      logo.loadGraphic("loadingScreen", 640, 360);
      add(logo);

      Assets.getSound("bading").Play();
      G.DoInSeconds(1.5f, () => {
        G.switchState(new MenuState(), "gate");
      });
    }
  }
}
