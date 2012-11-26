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
    public const float GRAVITY = 5;
    public const float PLAYER_VERTICAL_BOUNCE = 1f;
    
    public const float DANGER_SPEED = 750;

    public const float WALL_SPEED = 200;
    public const float WALL_DRAG = 0.004f;

    public bool dangerous = false;
    public Sprite shadow;
    public bool owned = false;
    public Player owner = null;
    public BallTrail trail;
    public bool collectable = true;

    float bounceVelocity = 0;
    float bounceRate = BOUNCE_AMOUNT;

    public Ball(float X=0, float Y=0) : base(X,Y) {
      loadGraphic("ball", 16, 16);
      height = 6;
      offset.Y = 8;
      offset.X = -3;
      //drag = new Vector2(750,750);
      linearDrag = 0.0005f;
      width = 9;
      motionSteps = 5;
      //acceleration.X = 100;

      trail = new BallTrail(this);
      G.state.add(trail);
      addOnMoveCallback(onMove);

      shadow = new Sprite(0, 0);
      shadow.loadGraphic("ballShadow", 10, 10);
      shadow.color = new Color(0x1c, 0x1c, 0x1c);
      shadow.z = 0;
      G.state.add(shadow);
    }

    void onMove(GameObject o) {
      trail.spawn(motionSteps);
      if(!owned) {
        bounceVelocity += GRAVITY * G.elapsed/motionSteps;
        offset.Y += bounceVelocity/motionSteps;
        if(bounceRate > -0.2f) {
          offset.Y = FLOOR_HEIGHT;
          collectable = true;
          assertDanger();
        } else if(offset.Y >= FLOOR_HEIGHT) {
          assertDanger();
          bounceVelocity = bounceRate;
          bounceRate *= BOUNCE_DECAY;
          collectable = true;
          //if(!dangerous) linearDrag = 0.02f;
        }
      }
    }

    public override void Update() {
      if(dangerous) {
        //color = Color.Red;
      } else {
        //color = Color.White;
      }
      base.Update();
    }

    public override void postUpdate() {
      if(x < 0) {
        x = 0;
        hitWall();
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
        hitWall();
      }

      z = shadow.y;// y + offset.Y;
      shadow.x = x;
      shadow.y = y + 14;

      trail.z = z + 0.1f;
      if(!dangerous) trail.active = false;

      base.postUpdate();
    }

    public void pickedUp() {
      visible = false;
      shadow.visible = false;
      bounceVelocity = 0;
      bounceRate = BOUNCE_AMOUNT;
    }

    public void Fling(float flingX, float flingY, float charge) {
      collectable = false;
      offset.Y = FLOAT_HEIGHT;
      visible = true;
      dangerous = true;
      shadow.visible = true;
      linearDrag = 0.005f;
      maxSpeed = 0f;
      bounceVelocity = -MathHelper.Clamp(charge/2000f,0,0.6f);
      velocity.X = flingX * charge;
      velocity.Y = flingY * charge;
      if(charge / Player.maxCharge > 0.99) trail.active = true;
      owned = false;
    }

    public void onCollide(Player player, bool playerWasDead=false) {
      if(dangerous && !playerWasDead && owner != null && player.team != owner.team) {
        bounceVelocity = -PLAYER_VERTICAL_BOUNCE;
        hitWall();
      }
    }

    void assertDanger() {
      dangerous = dangerous && velocity.Length() > DANGER_SPEED;
    }

    void hitWall() {
      collectable = false;
      velocity.X = -velocity.X;
      maxSpeed = WALL_SPEED;
      linearDrag = WALL_DRAG;
      dangerous = false;
    }
  }
}
