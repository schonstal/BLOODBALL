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
  class Retical : Group {
    public const int DOT_COUNT = 5;
    public const int DOT_SPREAD = 10;
    public const float DISTORTION_AMOUNT = 0.15f;
    public const float ANGLE_LIMIT = 45f;
    public const float AIM_THRESHOLD = 0.6f;

    public float charge = 0f;

    PlayerIndex playerIndex;
    Team team;

    Sprite[] dots;

    Vector2 direction = new Vector2();
    Vector2 computedDirection = new Vector2();

    public float X {
      set {
        x = value;
        for(int i = 0; i < DOT_COUNT; i++) {
          dots[i].x = x + ((i + 1) * DOT_SPREAD * direction.X);
        }
      }
      get { return x; }
    }

    public float Y {
      set {
        y = value;
        for(int i = 0; i < DOT_COUNT; i++) {
          dots[i].y = y + ((i + 1) * DOT_SPREAD * direction.Y) * 
            (direction.Y < 0 ? -(1 + DISTORTION_AMOUNT) : -(1 - DISTORTION_AMOUNT));
        }
      }
      get { return y; }
    }

    public Vector2 Direction {
      get {
        computedDirection.X = dots[DOT_COUNT - 1].x - dots[0].x;
        computedDirection.Y = dots[DOT_COUNT - 1].y - dots[0].y;
        return computedDirection;
      }
    }

    public bool onLeft {
      get { return team == Team.Left; }
    }

    public bool onRight {
      get { return team == Team.Right; }
    }


    public Retical(PlayerIndex playerIndex, Team team) : base() {
      this.team = team;
      this.playerIndex = playerIndex;
      z = 0;

      direction.X = onLeft ? 1 : -1;

      dots = new Sprite[DOT_COUNT];
      for(int i = 0; i < DOT_COUNT; i++) {
        dots[i] = new Sprite(i * DOT_SPREAD);
        dots[i].loadGraphic("Dot", 1, 1);
        dots[i].color = Color.MediumPurple;
        dots[i].z = 0;
        add(dots[i]);
      }
    }

    public override void Update() {
      updateDirection();
      if(charge > 0) {
        for(int i = 0; i < DOT_COUNT; i++) {
          dots[i].color = i < (int)(DOT_COUNT * charge) ? Color.Red : Color.MediumPurple;
        }
      }
      base.Update();
    }

    void updateDirection() {
      if(G.input.ThumbSticks(playerIndex).Right.Length() > AIM_THRESHOLD) {
        if((team == Team.Right && G.input.ThumbSticks(playerIndex).Right.X < 0) ||
            (team == Team.Left && G.input.ThumbSticks(playerIndex).Right.X > 0)) {
          if(onRight) {
            direction.X = MathHelper.Clamp(
              Vector2.Normalize(G.input.ThumbSticks(playerIndex).Right).X,
              -1f, -(float)Math.Cos(MathHelper.ToRadians(ANGLE_LIMIT)));
          } else {
            direction.X = MathHelper.Clamp(
              Vector2.Normalize(G.input.ThumbSticks(playerIndex).Right).X,
              (float)Math.Cos(MathHelper.ToRadians(ANGLE_LIMIT)), 1f);
          }
          direction.Y = G.input.ThumbSticks(playerIndex).Right.Y;
        }
        direction.Normalize();
      }
    }
  }
}
