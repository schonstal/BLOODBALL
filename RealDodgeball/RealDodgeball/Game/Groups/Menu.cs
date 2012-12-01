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
  class Menu : Group {
    public const int MENU_SPACING = 16;
    public const float THRESHOLD = 0.8f;
    public const float RETURN_THRESHOLD = 0.7f;

    int selectedIndex = 0;

    List<MenuText> menuItems = new List<MenuText>();

    Dictionary<string, bool> pushActive = new Dictionary<string,bool>();
    Dictionary<string, bool> lastPushActive = new Dictionary<string,bool>();
    Dictionary<string, Action> pushCallbacks = new Dictionary<string, Action>();

    List<string> direcions = new List<string> { "left", "right", "up", "down" };

    public Menu(float X, float Y) : base() {
      x = X;
      y = Y;
      direcions.ForEach((s) => {
        pushActive.Add(s, false);
        lastPushActive.Add(s, false);
      });
      pushCallbacks.Add("left", onLeft);
      pushCallbacks.Add("right", onRight);
      pushCallbacks.Add("up", onUp);
      pushCallbacks.Add("down", onDown);
    }

    public override void Update() {
      for(int i = 0; i < menuItems.Count; i++) {
        menuItems[i].x = x;
        menuItems[i].y = y + (i * MENU_SPACING);
        menuItems[i].selected = false;
      }

      if(menuItems[selectedIndex] != null) {
        menuItems[selectedIndex].selected = true;
      }

      handleInput(G.keyMaster);
      base.Update();
    }

    public void addMenuText(MenuText menuText) {
      menuItems.Add(menuText);
      add(menuText);
    }

    void onLeft() {
    }

    void onRight() {
    }

    void onUp() {
      selectedIndex--;
      if(selectedIndex < 0) selectedIndex = menuItems.Count - 1;
      Assets.getSound("select").Play(0.6f, -0.1f, 0);
    }

    void onDown() {
      selectedIndex++;
      selectedIndex %= menuItems.Count;
      Assets.getSound("select").Play(0.6f, 0, 0);
    }

    void onSelect() {
    }

    void onBack() {
    }

    void handleInput(PlayerIndex player) {
      direcions.ForEach((s) => lastPushActive[s] = pushActive[s]);
      float X = G.input.ThumbSticks(player).Left.X;
      float Y = G.input.ThumbSticks(player).Left.Y;

      if(X > THRESHOLD) pushActive["right"] = true;
      else if(X < RETURN_THRESHOLD) pushActive["right"] = false;

      if(X < -THRESHOLD) pushActive["left"] = true;
      else if(X > -RETURN_THRESHOLD) pushActive["left"] = false;

      if(Y > THRESHOLD) pushActive["up"] = true;
      else if(Y < RETURN_THRESHOLD) pushActive["up"] = false;

      if(Y < -THRESHOLD) pushActive["down"] = true;
      else if(Y > -RETURN_THRESHOLD) pushActive["down"] = false;

      direcions.ForEach((s) => {
        if(!lastPushActive[s] && pushActive[s]) {
          pushCallbacks[s]();
        }
      });
    }
  }
}
