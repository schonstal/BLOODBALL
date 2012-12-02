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
    public const float MENU_X = 186;

    Sprite titleScreen;
    Sprite controls;
    Text pressStart;
    Text pressStartShadow;
    Menu mainMenu;

    float flickerTimer = 0;
    bool ready = false;
    bool flicker = true;
    bool layerIn = false;

    public override void Create() {
      G.playMusic("titleMusic");
      titleScreen = new Sprite();
      titleScreen.screenPositioning = ScreenPositioning.Absolute;
      titleScreen.loadGraphic("titleScreen", 640, 360);
      add(titleScreen);

      mainMenu = new Menu(MENU_X, 204);
      mainMenu.addMenuText(new MenuText("START GAME", () => {
        Assets.getSound("superKO").Play();
        flicker = false;
        mainMenu.deactivate();
        G.DoForSeconds(0.5f, () => {
          MediaPlayer.Volume -= G.elapsed;
        }, () => {
          G.switchState(new PlayState(), "gate");
        });
      }));
      mainMenu.addMenuText(new MenuText("CONTROLS", displayControls));
      mainMenu.addMenuText(new MenuText("OPTIONS"));
      mainMenu.addMenuText(new MenuText("CREDITS"));
      mainMenu.addMenuText(new MenuText("EXIT", () => G.exit()));
      mainMenu.deactivate();
      add(mainMenu);

      controls = new Sprite(190, 182);
      controls.loadGraphic("controls", 251, 125);
      controls.visible = false;
      controls.screenPositioning = ScreenPositioning.Absolute;
      add(controls);

      DoInSeconds(2, () => {
        pressStart = new Text("PUSH START BUTTON");
        pressStart.y = 210;
        pressStart.x = 259;
        pressStartShadow = new Text(pressStart.text);
        pressStartShadow.y = pressStart.y + 1;
        pressStartShadow.x = pressStart.x;
        pressStartShadow.color = Color.Black;
        add(pressStartShadow);
        add(pressStart);
        ready = true;
      });

      GameTracker.MatchesWon[Team.Left] = 0;
      GameTracker.MatchesWon[Team.Right] = 0;
      GameTracker.RoundsWon[Team.Left] = 0;
      GameTracker.RoundsWon[Team.Right] = 0;
    }

    public override void Update() {
      if(ready) {
        if(pressStart.visible) {
          Input.ForEachInput((index) => {
            if(G.input.JustPressed(index, Buttons.Start)) {
              G.camera.y = -400;
              G.keyMaster = index;
              pressStart.visible = false;
              pressStartShadow.visible = false;
              mainMenu.activate();
              Assets.getSound("startButton").Play();
            }
          });
        }
      }

      if(flicker) {
        flickerTimer += G.elapsed;
        if(flickerTimer >= MIN_FLICKER_TIME) {
          titleScreen.sheetOffset.Y = (int)G.RNG.Next(0, 100) < 95 ? 0 : 360;
          flickerTimer -= MIN_FLICKER_TIME;
        }
      } else {
        titleScreen.sheetOffset.Y = 360;
      }

      if(layerIn && (G.input.JustPressed(G.keyMaster, Buttons.B) ||
          G.input.JustPressed(G.keyMaster, Buttons.Back)) ||
          (controls.visible && G.input.JustPressed(G.keyMaster, Buttons.A))) {
        goBack();
      }
      base.Update();
    }

    void displayControls() {
      goDeep();
      controls.visible = true;
    }

    void goDeep() {
      layerIn = true;
      Assets.getSound("startButton").Play();
      mainMenu.deactivate();
    }

    void goBack() {
      Assets.getSound("confirm").Play();
      controls.visible = false;
      layerIn = false;
      mainMenu.activate();
    }
  }
}
