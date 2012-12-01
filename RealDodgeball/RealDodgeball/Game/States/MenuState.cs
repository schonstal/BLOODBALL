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
    Menu mainMenu;

    float flickerTimer = 0;
    bool ready = false;

    public override void Create() {
      titleScreen = new Sprite();
      titleScreen.screenPositioning = ScreenPositioning.Absolute;
      titleScreen.loadGraphic("titleScreen", 640, 360);
      add(titleScreen);

      mainMenu = new Menu(186, 204);
      mainMenu.addMenuText(new MenuText("START GAME"));
      mainMenu.addMenuText(new MenuText("CONTROLS"));
      mainMenu.addMenuText(new MenuText("OPTIONS"));
      mainMenu.addMenuText(new MenuText("CREDITS"));
      mainMenu.addMenuText(new MenuText("EXIT"));
      mainMenu.visible = false;
      mainMenu.active = false;
      add(mainMenu);

      DoInSeconds(2, () => {
        pressStart = new Text("PUSH START BUTTON");
        pressStart.y = 210;
        pressStart.x = 259;
        add(pressStart);
        ready = true;
      });
    }

    public override void Update() {
      if(ready && pressStart.visible) {
        Input.ForEachInput((index) => {
          if(G.input.JustPressed(index, Buttons.Start)) {
            G.camera.y = -400;
            G.keyMaster = index;
            pressStart.visible = false;
            mainMenu.visible = true;
            mainMenu.active = true;
            Assets.getSound("startButton").Play();
            //G.switchState(new PlayState(), "gate");
          }
        });
      }

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
