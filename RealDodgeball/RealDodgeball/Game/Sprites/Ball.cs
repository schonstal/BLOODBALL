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
    public const int FLOAT_HEIGHT = -4;
    public const int FLOOR_HEIGHT = 7;
    public const float BOUNCE_AMOUNT = -1f;
    public const float BOUNCE_DECAY = 0.85f;
    public const float GRAVITY = 6;
    public const float DANGER_SPEED = 750;

    public bool dangerous = false;
    public Sprite shadow;

    float bounceVelocity = 0;
    float bounceRate = BOUNCE_AMOUNT;

    public Ball() : base(0,0) {
      loadGraphic("ball", 16, 16);
      height = 6;
      offset.Y = 8;
      offset.X = -3;
      //drag = new Vector2(750,750);
      linearDrag = 0.005f;
      width = 9;
      //acceleration.X = 100;
      moves = true;

      shadow = new Sprite(0, 0);
      shadow.loadGraphic("ballShadow", 10, 10);
      shadow.color = new Color(0x1c, 0x1c, 0x1c);
      shadow.z = 0;
      G.state.add(shadow);
    }

    public override void Update() {
      bounceVelocity += GRAVITY * G.elapsed;
      offset.Y += bounceVelocity;
      if(bounceRate > -0.2f) {
        offset.Y = FLOOR_HEIGHT;
      } else if(offset.Y >= FLOOR_HEIGHT) {
        dangerous = dangerous && velocity.Length() > DANGER_SPEED;
        bounceVelocity = bounceRate;
        bounceRate *= BOUNCE_DECAY;
      }

      if(dangerous) {
        color = Color.Red;
      } else {
        color = Color.White;
      }
      base.Update();
    }

    public override void postUpdate() {
      if(x < 0) {
        x = 0;
        velocity.X = -velocity.X;
        maxSpeed = 200f;
        linearDrag = 0.02f;
        dangerous = false;
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
        maxSpeed = 200f;
        linearDrag = 0.02f;
        dangerous = false;
      }

      z = shadow.y;// y + offset.Y;
      shadow.x = x;
      shadow.y = y + 14;

      base.postUpdate();
    }

    public void pickedUp() {
      visible = false;
      shadow.visible = false;
      bounceVelocity = 0;
      bounceRate = BOUNCE_AMOUNT;
    }

    public void Fling(float flingX, float flingY, float charge) {
      offset.Y = FLOAT_HEIGHT;
      visible = true;
      dangerous = true;
      shadow.visible = true;
      linearDrag = 0.005f;
      maxSpeed = 0f;
      bounceVelocity = -MathHelper.Clamp(charge/2000f,0,1);
      velocity.X = flingX * charge;
      velocity.Y = flingY * charge;
    }
  }
}
