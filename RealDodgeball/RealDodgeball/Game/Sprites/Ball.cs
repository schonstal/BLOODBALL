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
  class Ball : Sprite {
    public bool dangerous = false;
    Sprite shadow;

    public Ball() : base(0,0) {
      loadGraphic("Dot", 12, 12);
      color = Color.DarkRed;
      height = 6;
      offset.Y = 10;
      //drag = new Vector2(750,750);
      linearDrag = 0.005f;
      //acceleration.X = 100;
      moves = true;

      shadow = new Sprite(0, 0);
      shadow.loadGraphic("Dot", 12, 8);
      shadow.color = new Color(0x1c, 0x1c, 0x1c);
      shadow.z = 0;
      G.state.add(shadow);
    }

    public override void Update() {
      x.ToString();
      if(dangerous) {
        color = Color.Red;
      }
      base.Update();
    }

    public override void postUpdate() {
      if(x < 0) {
        x = 0;
        velocity.X = -velocity.X;
      }
      if(y < 0) {
        y = 0;
        velocity.Y = -velocity.Y;
      }
      if(y > PlayState.ARENA_HEIGHT - height) {
        y = PlayState.ARENA_HEIGHT - height;
        velocity.Y = -velocity.Y;
      }
      if(x > PlayState.ARENA_WIDTH - width) {
        x = PlayState.ARENA_WIDTH - width;
        velocity.X = -velocity.X;
      }

      z = y;// +height;
      shadow.x = x;
      shadow.y = y + 16;

      base.postUpdate();
    }

    public void pickedUp() {
      visible = false;
      shadow.visible = false;
    }

    public void Fling(float flingX, float flingY, float charge) {
      offset.Y = -2;
      dangerous = true;
      visible = true;
      shadow.visible = true;
      velocity.X = flingX * charge;
      velocity.Y = flingY * charge;
    }
  }
}
