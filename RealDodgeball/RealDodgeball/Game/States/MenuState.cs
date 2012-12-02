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
    Group credits;

    MenuText roundOption;
    List<int> rounds = new List<int> { 1, 3, 5, 7, 9 };

    Menu mainMenu;
    Menu optionsMenu;

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

      roundOption = new MenuText("ROUNDS:", () => {
        Assets.getSound("select").Play(0.6f, 0, 0);
        GameTracker.RoundsToWin = (GameTracker.RoundsToWin % 5) + 1;
        roundOption.valueText = rounds[GameTracker.RoundsToWin - 1].ToString();
      });
      roundOption.valueText = rounds[GameTracker.RoundsToWin - 1].ToString();

      mainMenu = new Menu(MENU_X, 204);
      mainMenu.addMenuText(new MenuText("START GAME", () => {
        Assets.getSound("superKO").Play();
        flicker = false;
        mainMenu.deactivate();
        G.DoForSeconds(0.5f, () => {
          MediaPlayer.Volume -= G.elapsed;
        }, () => {
          G.switchState(new TeamSelectState(), "gate");
        });
      }));
      mainMenu.addMenuText(new MenuText("CONTROLS", displayControls));
      mainMenu.addMenuText(roundOption);
      //mainMenu.addMenuText(new MenuText("OPTIONS", displayOptions));
      mainMenu.addMenuText(new MenuText("CREDITS", displayCredits));
      mainMenu.addMenuText(new MenuText("EXIT", () => G.exit()));
      mainMenu.deactivate();
      add(mainMenu);

      optionsMenu = new Menu(MENU_X, 204);
      //optionsMenu.addMenuText(new MenuText("ROUNDS:"));
      //optionsMenu.addMenuText(new MenuText("ZOOM:"));
      //optionsMenu.addMenuText(new MenuText("FULLSCREEN:", G.toggleFullscreen));
      optionsMenu.deactivate();
      add(optionsMenu);

      credits = new Group();
      Text credit;
      credit = new Text("CODE", MENU_X, 204);
      credit.color = new Color(0x77, 0x80, 0x85);
      credits.add(credit);
      credits.add(new Text("JOSH SCHONSTAL", MENU_X, 220));
      credit = new Text("ART", MENU_X, 236);
      credit.color = new Color(0x77, 0x80, 0x85);
      credits.add(credit);
      credits.add(new Text("IAN BROCK", MENU_X, 252));
      credit = new Text("SOUND", MENU_X, 268);
      credit.color = new Color(0x77, 0x80, 0x85);
      credits.add(credit);
      credits.add(new Text("GUERIN MCMURRY", MENU_X, 284));
      credits.visible = false;
      add(credits);

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
          ((controls.visible || credits.visible) && G.input.JustPressed(G.keyMaster, Buttons.A))) {
        goBack();
      }
      base.Update();
    }

    void displayControls() {
      goDeep();
      controls.visible = true;
    }

    void displayCredits() {
      goDeep();
      credits.visible = true;
    }

    void displayOptions() {
      goDeep();
      optionsMenu.activate();
    }

    void goDeep() {
      layerIn = true;
      Assets.getSound("startButton").Play();
      mainMenu.deactivate();
    }

    void goBack() {
      Assets.getSound("confirm").Play();
      controls.visible = false;
      credits.visible = false;
      optionsMenu.deactivate();
      layerIn = false;
      mainMenu.activate();
    }
  }
}
