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
    public const float MIN_RUN_SPEED = 30;
    public const float BALL_OFFSEET_X = 5f;
    public const float BALL_OFFSEET_Y = 8f;
    public const float CATCH_THRESHOLD = 0.06f;
    public const float CONTROL_DRAG = 2500f;

    public const float MAX_RUN_SPEED = 175f;
    public const float CHARGE_RUN_SPEED = 100f;

    public const float MIN_THROW_FPS = 20f;
    public const float MAX_THROW_FPS = 60f;
    public const float MIN_THROW_DELAY = 0;
    public const float MAX_THROW_DELAY = 0.5f;
    public const float maxCharge = 2000.0f;
    public const float minCharge = 700.0f;
    public const float DROP_CHARGE = 80.0f;
    
    public const float DAMAGE_DENOM = 50f;
    public const float HEAL_DENOM = 100f;

    public const float MAX_HITPOINTS = 100.0f;
    public const float HIT_DRAG = 0.1f;
    public const float HIT_STOP_SPEED = 5f;
    public const float MIN_HIT_SECONDS = 0.2f;
    public const float MAX_HIT_SECONDS = 0.5f;
    public const float MIN_HIT_POWER = 0.4f;
    public const float MAX_HIT_POWER = 1f;

    public const float DROP_PICKUP_TIME = 0.5f;

    public const float PARRY_WINDOW_SECONDS = 0.1f;
    public const float PARRY_STUN_SECONDS = 0.1f;
    public const float PARRY_DELAY_SECONDS = 0.5f;

    String[] SPECIAL_ANIMATIONS = new String[] {
      "throw", "idle", "throwReturn", "hurt", "hurtFall", "hurtRecover", "parry", "parryReturn"
    };

    public PlayerShadow shadow;
    public Team team;
    public CourtPosition courtPosition;
    public PlayerIndex playerIndex;

    Heading heading;
    float movementAccel = 5000.0f;
    Retical retical;
    BloodSpatter blood;
    Vector2 spriteSubmatrix = new Vector2(0, 0);
    Vector2 characterOffset = new Vector2(0, 0);

    float charge = 0;
    float flungAtCharge = 0;
    float chargeAmount = 1000.0f;
    float hitPoints = MAX_HITPOINTS;

    Ball ball = null;

    bool rightTriggerHeld = false;
    bool rightTriggerWasHeld = false;
    bool leftTriggerHeld = false;
    bool leftTriggerWasHeld = false;

    //game jam state machine ;)
    bool throwing = false;
    bool hurt = false;
    bool parrying = false;
    bool canPickupBall = true;

    Vector2[][] throwOffsets = new Vector2[5][];
    Vector2[] fallOffsets;

    float parryTimer = PARRY_DELAY_SECONDS;

    public bool onLeft {
      get { return team == Team.Left; }
    }

    public bool onRight {
      get { return team == Team.Right; }
    }

    public SpriteMode Mode {
      set { 
        sheetOffset.Y = (int)value * GraphicHeight + characterOffset.Y;
        shadow.sheetOffset.Y = (int)value * shadow.GraphicHeight;
      }
    }

    public bool Stunned {
      get { return throwing || hurt || parrying; }
    }

    public float HP {
      get { return hitPoints; }
    }

    public bool Dead {
      get { return hitPoints <= 0; }
    }

    public float MaxFling {
      get { return maxCharge; }
    }

    public bool ActiveParry {
      get { return parryTimer < PARRY_WINDOW_SECONDS; }
    }

    public Player(PlayerIndex playerIndex, Team team, Vector2 submatrix,
        CourtPosition position) : base() {
      this.playerIndex = playerIndex;
      this.team = team;
      spriteSubmatrix = submatrix;
      heading = Heading.Forward;

      courtPosition = position;
      switch (position) {
        case CourtPosition.TopLeft:
          x = 80;
          y = 40;
          break;
        case CourtPosition.TopRight:
          x = PlayState.ARENA_WIDTH - 80;
          y = 40;
          break;
        case CourtPosition.BottomLeft:
          x = 80;
          y = PlayState.ARENA_HEIGHT - 100;
          break;
        case CourtPosition.BottomRight:
          x = PlayState.ARENA_WIDTH - 80;
          y = PlayState.ARENA_HEIGHT - 100;
          break;
      }
      
      maxSpeed = MAX_RUN_SPEED;
      drag = new Vector2(CONTROL_DRAG,CONTROL_DRAG);
      
      loadGraphic("player", 34, 34);
      characterOffset.X = submatrix.X * atlas.Width / 2;
      characterOffset.Y = submatrix.Y * atlas.Height / 2;

      sheetOffset.X = characterOffset.X;

      addAnimation("idle", new List<int> { 0, 1, 2, 3 }, 10, true);

      addAnimation("runForward", new List<int> { 12, 13, 14, 15 }, 15, true);
      addAnimation("runBackward", new List<int> { 16, 17, 18, 19 }, 15, true);
      addAnimation("runUpForward", new List<int> { 4, 5, 6, 7 }, 15, true);
      addAnimation("runDownForward", new List<int> { 8, 9, 10, 11 }, 15, true);
      addAnimation("runUpBackward", new List<int> { 8, 11, 10, 9 }, 15, true);
      addAnimation("runDownBackward", new List<int> { 6, 5, 4, 7 }, 15, true);

      addAnimation("throw", new List<int> { 0, 1, 2, 3 }, 10, false);
      addAnimation("throwReturn", new List<int> { 4, 4 }, 20, false);
      addAnimationCallback("throw", onThrowCallback);
      addOnCompleteCallback("throw", onThrowCompleteCallback);
      addOnCompleteCallback("throwReturn", onThrowReturnCompleteCallback);

      addAnimation("hurt", new List<int> { 5 });
      addAnimation("hurtFall", new List<int> { 6, 7 }, 10, false);
      addAnimation("hurtRecover", new List<int> { 8, 8 }, 20, false);
      addOnCompleteCallback("hurtRecover", onHurtRecoverCompleteCallback);
      addAnimationCallback("hurtFall", onHurtFallCallback);

      addAnimation("parry", new List<int> { 10, 9, 9, 10 }, 40, false);
      addAnimation("parryReturn", new List<int> { 10, 10 }, 20, false);
      addOnCompleteCallback("parryReturn", onParryReturn);

      throwOffsets[(int)Heading.Up] = new Vector2[3] {
          new Vector2(0, 0),
          new Vector2(0, 0),
          new Vector2(0, 0)
        };
      throwOffsets[(int)Heading.UpMid] = new Vector2[3] {
          new Vector2(0, 0),
          new Vector2(0, 0),
          new Vector2(0, 0)
        };
      throwOffsets[(int)Heading.Forward] = new Vector2[3] {
          new Vector2(1, 0),
          new Vector2(3, 0),
          new Vector2(1, 0)
        };
      throwOffsets[(int)Heading.DownMid] = new Vector2[3] {
          new Vector2(0, 0),
          new Vector2(0, 0),
          new Vector2(0, 0)
        };
      throwOffsets[(int)Heading.Down] = new Vector2[3] {
          new Vector2(0, 0),
          new Vector2(0, 0),
          new Vector2(0, 0)
        };

      //We can change this to use Heading later if needed
      fallOffsets = new Vector2[2] {
        new Vector2(-4, 0),
        new Vector2(-11, 0)
      };

      //No actual hit yet, this should substitute for now
      addAnimation("hit", new List<int> { 12, 13, 14, 15 }, 60, true);

      height = 22;
      offset.Y = -5;
      width = 18;
      offset.X = -9;

      shadow = new PlayerShadow(this);
      G.state.add(shadow);

      retical = new Retical(playerIndex, team);
      retical.visible = false;
      G.state.add(retical);

      blood = new BloodSpatter(this);
      G.state.add(blood);
    }

    public override void Update() {
      updateAnimation();
      updatePhysics();
      updateHeading();

      parryTimer += G.elapsed;

      rightTriggerWasHeld = rightTriggerHeld;
      if(G.input.Triggers(playerIndex).Right > 0.3) {
        rightTriggerHeld = true;
      } else {
        rightTriggerHeld = false;
      }

      leftTriggerWasHeld = leftTriggerHeld;
      if(G.input.Triggers(playerIndex).Left > 0.3) {
        leftTriggerHeld = true;
      } else {
        leftTriggerHeld = false;
      }

      if(this.ball != null) {
        if(charge > 0) {
          Mode = SpriteMode.Charge;
        } else {
          Mode = SpriteMode.Hold;
        }
        ball.x = x + BALL_OFFSEET_X;
        ball.y = y + BALL_OFFSEET_Y;
        if(rightTriggerHeld) {
          retical.visible = true;
          rightTriggerHeld = true;
          maxSpeed = CHARGE_RUN_SPEED;
          if(charge < maxCharge)
            charge += chargeAmount * G.elapsed;
          charge = MathHelper.Clamp(charge, minCharge, maxCharge);
        } else {
          retical.visible = false;
          charge = MathHelper.Clamp(charge, minCharge, maxCharge);
          if(rightTriggerWasHeld) FlingBall();
          rightTriggerHeld = false;
          maxSpeed = MAX_RUN_SPEED;
          charge = 0;
        }
        if(G.input.JustPressed(playerIndex, Buttons.A) ||
          G.input.JustPressed(playerIndex, Buttons.LeftShoulder)) {
          dropBall();
        }
      } else {
        Mode = SpriteMode.Neutral;
        charge = 0;
      }

      if(!leftTriggerWasHeld && leftTriggerHeld) {
        parry();
      }

      if(throwing || hurt || parrying) Mode = SpriteMode.Misc;
      if(parrying && ball != null) {
        sheetOffset.X = characterOffset.X + (graphicWidth * 2);
      } else {
        sheetOffset.X = characterOffset.X;
      }


      retical.charge = charge / maxCharge;

      base.Update();
    }

    public override void postUpdate() {
      if(onLeft) {
        if(x < 0) x = 0;
        if(x > PlayState.ARENA_WIDTH / 2 - width) x = PlayState.ARENA_WIDTH / 2 - width;
      } else {
        if(x > PlayState.ARENA_WIDTH - width) x = PlayState.ARENA_WIDTH - width;
        if(x < PlayState.ARENA_WIDTH / 2) x = PlayState.ARENA_WIDTH / 2;
      }
      if(y < 0) y = 0;
      if(y > PlayState.ARENA_HEIGHT - height) y = PlayState.ARENA_HEIGHT - height;

      shadow.y = y + shadow.Y_OFFSET;
      shadow.x = x + shadow.X_OFFSET;
      z = shadow.y;

      if(team == Team.Right) {
        retical.X = x + 5;
        retical.Y = y + 24;
      } else {
        retical.X = x + 15;
        retical.Y = y + 24;
      }

      if(Dead) retical.visible = false;
      base.postUpdate();
    }

    void FlingBall() {
      if(ball != null && !Dead) {
        flungAtCharge = charge;

        Vector2 flingDirection = Vector2.Normalize(retical.Direction);
        ball.Fling(flingDirection.X, flingDirection.Y, charge);

        ball = null;
        play("throw");
        throwing = true;
        animation.reset();
        animation.FPS = MIN_THROW_FPS + ((charge / maxCharge) *
            (MAX_THROW_FPS - MIN_THROW_FPS));
        G.state.DoForSeconds(0.2f,
          () => GamePad.SetVibration(playerIndex,
            (flungAtCharge - minCharge) / (maxCharge - minCharge), 0),
          () => GamePad.SetVibration(playerIndex, 0, 0));
      }
    }

    void dropBall() {
      if(ball != null) {
        flungAtCharge = DROP_CHARGE;
        ball.Fling(onRight ? 1f : -1f, 0.5f, DROP_CHARGE);
        //ball.dangerous = false;
        ball = null;
        canPickupBall = false;
        G.state.DoInSeconds(DROP_PICKUP_TIME, () => canPickupBall = true);
        maxSpeed = MAX_RUN_SPEED;
        retical.visible = false;
      }
    }

    void parry() {
      if(parryTimer > PARRY_DELAY_SECONDS && !throwing && !hurt) {
        charge = 0;
        maxSpeed = MAX_RUN_SPEED;
        retical.visible = false;

        parrying = true;
        play("parry");
        animation.reset();
        parryTimer = 0;
        G.state.DoInSeconds(PARRY_STUN_SECONDS, () => {
          if(parrying && !hurt) {
            play("parryReturn");
            animation.reset();
          }
        });
      }
    }

    void onParryReturn(int frameIndex) {
      parrying = false;
    }

    void updateAnimation() {
      if(throwing || parrying) {
        //play("throw");
      } else if(hurt) {
        if(velocity.Length() < HIT_STOP_SPEED &&
            currentAnimation != "hurtFall" &&
            currentAnimation != "hurtRecover") {
          velocity.X = velocity.Y = 0;
          if(Dead) play("hurtFall");
          else play("hurtRecover");
          animation.reset();
        }
      } else if(Math.Abs(velocity.X) > Math.Abs(velocity.Y)) {
        if(velocity.X > MIN_RUN_SPEED) play("run" + forwardOn(onLeft));
        else if(velocity.X < -MIN_RUN_SPEED) play("run" + forwardOn(onRight));
        else play("idle");
      } else {
        if(velocity.Y > MIN_RUN_SPEED) {
          play("runDown" + (velocity.X < 0 ? forwardOn(onRight) : forwardOn(onLeft)));
        } else if(velocity.Y < -MIN_RUN_SPEED) {
          play("runUp" + (velocity.X < 0 ? forwardOn(onRight) : forwardOn(onLeft)));
        } else play("idle");
      }

      if(!SPECIAL_ANIMATIONS.Contains(currentAnimation)) {
        animation.FPS = velocity.Length() / 14f;
      }
    }

    string forwardOn(bool isForward) {
      return isForward ? "Forward" : "Backward";
    }

    void updatePhysics() {
      if(!Stunned) {
        acceleration.X = G.input.ThumbSticks(playerIndex).Left.X * movementAccel;
        if(Math.Sign(acceleration.X) != Math.Sign(velocity.X)) acceleration.X *= 15;

        acceleration.Y = G.input.ThumbSticks(playerIndex).Left.Y * -movementAccel;
        if(Math.Sign(acceleration.Y) != Math.Sign(velocity.Y)) acceleration.Y *= 15;
      } else {
        acceleration.X = acceleration.Y = 0;
      }
    }

    void updateHeading() {
    }

    void onThrowCallback(int frameIndex) {
      int teamDirection = onLeft ? 1 : -1;
      if(frameIndex > 0) {
        x += throwOffsets[(int)heading][frameIndex - 1].X * teamDirection;
        y += throwOffsets[(int)heading][frameIndex - 1].Y * teamDirection;
      }
    }

    void onHurtFallCallback(int frameIndex) {
      int teamDirection = onLeft ? 1 : -1;
      if(frameIndex > 0) {
        x += fallOffsets[frameIndex].X * teamDirection;
        y += fallOffsets[frameIndex].Y * teamDirection;
      }
    }

    void onThrowCompleteCallback(int frameIndex) {
      float seconds = MathHelper.Lerp(
        MIN_THROW_DELAY, MAX_THROW_DELAY,
        (flungAtCharge - minCharge) / (maxCharge - minCharge));

      G.state.DoInSeconds(seconds, () => {
        if(!hurt && !Dead) {
          play("throwReturn");
          animation.reset();
        }
      });
    }

    void onThrowReturnCompleteCallback(int frameIndex) {
      throwing = false;
    }

    void onHurtRecoverCompleteCallback(int frameIndex) {
      hurt = false;
    }

    public void onCollide(Ball ball) {
      if(!ball.dangerous && this.ball == null && !ball.owned &&
          !throwing && !hurt && !Dead && ball.collectable && canPickupBall) {
        takeBall(ball);
      } else if(ball.dangerous && !Dead) {
        hitRumble(ball);
        if(ActiveParry) {
          if(this.ball == null) {
            catchBall(ball);
          } else {
            blockBall(ball);
          }
        } else {
          hitByBall(ball);
        }
      }
    }

    void catchBall(Ball ball) {
      hitPoints += (ball.velocity.Length()) / HEAL_DENOM;
      takeBall(ball);
    }

    void blockBall(Ball ball) {
    }

    void takeBall(Ball ball) {
      ball.dangerous = false;
      ball.owned = true;
      ball.owner = this;
      this.ball = ball;
      this.ball.pickedUp();
    }

    void hitByBall(Ball ball) {
      if(ball.owner != null && ball.owner.team != team) {
        hitPoints -= (ball.velocity.Length()) / DAMAGE_DENOM;
        hurt = true;
        throwing = false;
        parrying = false;
        velocity.X = ball.velocity.X*10;
        velocity.Y = ball.velocity.Y*10;
        play("hurt");
        blood.spray();
        dropBall();
      }
    }

    void hitRumble(Ball ball) {
      float relativeSpeed = ball.velocity.Length() / MaxFling;
      float seconds = MathHelper.Lerp(MIN_HIT_SECONDS, MAX_HIT_SECONDS, relativeSpeed);
      //TODO: FINISH THIS SHIT
      float big = MathHelper.Lerp(MIN_HIT_POWER, MAX_HIT_POWER, relativeSpeed);
      float little = MathHelper.Lerp(MIN_HIT_POWER, MAX_HIT_POWER, relativeSpeed);

      G.state.DoForSeconds(seconds,
        () => GamePad.SetVibration(playerIndex, big, little),
        () => GamePad.SetVibration(playerIndex, 0, 0));
    }

    public override void play(string animation) {
      shadow.play(animation);
      base.play(animation);
    }
  }

  public enum Team {
    Left = 0x01,
    Right = 0x02
  }

  public enum Heading {
    Up = 0,
    UpMid = 1,
    Forward = 2,
    DownMid = 3,
    Down = 4
  }

  public enum SpriteMode {
    Neutral = 0,
    Hold = 1,
    Charge = 2,
    Misc = 3
  }

  public enum CourtPosition {
    TopLeft, BottomLeft,
    TopRight, BottomRight
  }
}
