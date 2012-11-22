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
  class Player : Sprite {
    public const float MIN_RUN_SPEED = 50;
    public const float BALL_OFFSEET_X = 5f;
    public const float BALL_OFFSEET_Y = 8f;
    public Sprite shadow;

    PlayerIndex playerIndex;
    Team team;
    float movementAccel = 5000.0f;
    Retical retical;

    float charge = 0;
    float chargeAmount = 1000.0f;
    float maxCharge = 2500.0f;
    float minCharge = 700.0f;

    Ball ball = null;

    bool triggerHeld = false;
    bool triggerWasHeld = false;

    public Player(PlayerIndex playerIndex, Team team, float X=0f, float Y=0f) : base(X,Y) {
      this.playerIndex = playerIndex;
      this.team = team;
      
      maxSpeed = 250f;
      drag = new Vector2(2500,2500);
      
      loadGraphic("player", 34, 34);
      addAnimation("idle", new List<int> { 0, 1, 2, 3 }, 15, true);

      addAnimation("runForward", new List<int> { 12, 13, 14, 15 }, 15, true);
      addAnimation("runBackward", new List<int> { 16, 17, 18, 19 }, 15, true);
      addAnimation("runUpForward", new List<int> { 4, 5, 6, 7 }, 15, true);
      addAnimation("runDownForward", new List<int> { 8, 9, 10, 11 }, 15, true);
      addAnimation("runUpBackward", new List<int> { 11, 10, 9, 8 }, 15, true);
      addAnimation("runDownBackward", new List<int> { 7, 6, 5, 4 }, 15, true);

      //No actual hit yet, this should substitute for now
      addAnimation("hit", new List<int> { 12, 13, 14, 15 }, 60, true);

      height = 22;
      offset.Y = -5;
      width = 18;
      offset.X = -9;

      shadow = new Sprite(0, 0);
      shadow.loadGraphic("playerShadow", 13, 12);
      shadow.color = new Color(0x1c, 0x1c, 0x1c);
      shadow.z = 0;
      G.state.add(shadow);

      retical = new Retical(playerIndex, team);
      retical.visible = false;
      G.state.add(retical);
    }

    public override void Update() {
      updateAnimation();

      acceleration.X = G.input.ThumbSticks(playerIndex).Left.X * movementAccel;
      if(Math.Sign(acceleration.X) != Math.Sign(velocity.X)) acceleration.X *= 15;

      acceleration.Y = G.input.ThumbSticks(playerIndex).Left.Y * -movementAccel;
      if(Math.Sign(acceleration.Y) != Math.Sign(velocity.Y)) acceleration.Y *= 15;

      triggerWasHeld = triggerHeld;
      if(G.input.Triggers(playerIndex).Right > 0.3) {
        triggerHeld = true;
      } else {
        triggerHeld = false;
      }

      if(this.ball != null) {
        ball.x = x + BALL_OFFSEET_X;
        ball.y = y + BALL_OFFSEET_Y;
        if(triggerHeld) {
          retical.visible = true;
          triggerHeld = true;
          maxSpeed = 150f;
          if(charge < maxCharge)
            charge += chargeAmount * G.elapsed;
          charge = MathHelper.Clamp(charge, minCharge, maxCharge);
        } else {
          retical.visible = false;
          if(triggerWasHeld) FlingBall();
          triggerHeld = false;
          maxSpeed = 250f;
          charge = 0;
        }
      } else {
        charge = 0;
      }

      retical.charge = charge / maxCharge;

      base.Update();
    }

    public override void postUpdate() {
      if(x < 0) x = 0;
      if(y < 0) y = 0;
      if(y > PlayState.ARENA_HEIGHT - height) y = PlayState.ARENA_HEIGHT - height;
      if(x > PlayState.ARENA_WIDTH - width) x = PlayState.ARENA_WIDTH - width;
      z = shadow.y;
      shadow.y = y + 18;
      shadow.x = x + 4;

      if(team == Team.Right) {
        retical.X = x + 5;
        retical.Y = y + 24;
      } else {
        retical.X = x + 15;
        retical.Y = y + 24;
      }
      base.postUpdate();
    }

    void FlingBall() {
      if(ball != null) {
        Vector2 flingDirection = Vector2.Normalize(retical.Direction);
        if(float.IsNaN(flingDirection.X) || float.IsNaN(flingDirection.Y)) {
          ball.Fling(-1, 0, charge);
        } else {
          ball.Fling(flingDirection.X, flingDirection.Y, charge);
        }
        ball = null;
        G.state.DoForSeconds(0.2f,
          () => GamePad.SetVibration(playerIndex, 0.4f, 0.2f),
          () => GamePad.SetVibration(playerIndex, 0, 0));
      }
    }

    void updateAnimation() {
      if(Math.Abs(velocity.X) > Math.Abs(velocity.Y)) {
        if(velocity.X > MIN_RUN_SPEED) play("runBackward");
        else if(velocity.X < -MIN_RUN_SPEED) play("runForward");
        else play("idle");
      } else {
        if(velocity.Y > MIN_RUN_SPEED) {
          play(velocity.X < 0 ? "runDownForward" : "runDownBackward");
        } else if(velocity.Y < -MIN_RUN_SPEED) {
          play(velocity.X < 0 ? "runUpForward" : "runUpBackward");
        } else play("idle");
      }
    }

    public void PickUpBall(Ball ball) {
      if(!ball.dangerous && this.ball == null && !ball.owned) {
        ball.owned = true;
        this.ball = ball;
        this.ball.pickedUp();
      }
    }
  }

  public enum Team {
    Left = 0x01,
    Right = 0x02
  }
}
