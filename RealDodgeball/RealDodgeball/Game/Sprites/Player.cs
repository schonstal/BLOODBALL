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
    PlayerIndex playerIndex;
    Team team;
    float movementAccel = 5000.0f;

    public Player(PlayerIndex playerIndex, Team team) : base(0,0) {
      this.playerIndex = playerIndex;
      this.team = team;
      
      maxSpeed = 350f;
      drag = new Vector2(2500,2500);
      
      loadGraphic("Dot", 24, 24);
      color = Color.DarkMagenta;

      height = 20;
      offset.Y = -4;
    }

    public override void Update() {
      acceleration.X = G.input.ThumbSticks(playerIndex).Left.X * movementAccel;
      if(Math.Sign(acceleration.X) != Math.Sign(velocity.X)) acceleration.X *= 15;

      acceleration.Y = G.input.ThumbSticks(playerIndex).Left.Y * -movementAccel;
      if(Math.Sign(acceleration.Y) != Math.Sign(velocity.Y)) acceleration.Y *= 15;

      if(G.input.Triggers(playerIndex).Right > 0.3)
        maxSpeed = 200f;
      else
        maxSpeed = 350f;

      base.Update();
    }

    public override void postUpdate() {
      if(x < 0) x = 0;
      if(y < 0) y = 0;
      if(y > PlayState.ARENA_HEIGHT - height) y = PlayState.ARENA_HEIGHT - height;
      if(x > PlayState.ARENA_WIDTH - width) x = PlayState.ARENA_WIDTH - width;
      base.postUpdate();
    }
  }

  public enum Team {
    Left = 0x01,
    Right = 0x02
  }
}
