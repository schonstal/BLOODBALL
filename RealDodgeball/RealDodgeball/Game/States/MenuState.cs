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
  public class MenuState : GameState {
    public const float MIN_FLICKER_TIME = 0.03f;

    Sprite titleScreen;
    Text pressStart;

    float flickerTimer = 0;

    public override void Create() {
      titleScreen = new Sprite();
      titleScreen.screenPositioning = ScreenPositioning.Absolute;
      titleScreen.loadGraphic("titleScreen", 640, 360);
      add(titleScreen);

      pressStart = new Text("PUSH START BUTTON");
      pressStart.y = 210;
      pressStart.x = 259;
      add(pressStart);
    }

    public override void Update() {
      Input.ForEachInput((index) => {
        if(G.input.JustPressed(index, Buttons.Start)) {
          G.camera.y = -400;
          G.switchState(new PlayState(), "gate");
        }
      });
      flickerTimer += G.elapsed;
      if(flickerTimer >= MIN_FLICKER_TIME) {
        titleScreen.sheetOffset.Y = (int)G.RNG.Next(0, 100) < 95 ? 0 : 360;
        flickerTimer -= MIN_FLICKER_TIME;
      }
      base.Update();
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
