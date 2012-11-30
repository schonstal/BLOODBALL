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
    Sprite titleScreen;
    Text pressStart;

    public override void Create() {
      titleScreen = new Sprite();
      titleScreen.screenPositioning = ScreenPositioning.Absolute;
      titleScreen.loadGraphic("titleScreen", 640, 360);
      add(titleScreen);

      pressStart = new Text("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG");
      add(pressStart);
    }

    public override void Update() {
      Input.ForEachInput((index) => {
        if(G.input.JustPressed(index, Buttons.Start)) {
          G.camera.y = -400;
          G.switchState(new PlayState(), "gate");
        }
      });
      base.Update();
    }

    public override void Draw() {
      base.Draw();
    }
  }
}
