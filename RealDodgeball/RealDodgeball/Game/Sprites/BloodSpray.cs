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
  class BloodSpatter : Sprite {
    Player player;

    public BloodSpatter(Player player, float X=0, float Y=0) : base(X,Y) {
      if(player.team == Team.Left) {
        sheetOffset.Y = 16;
        offset.X = 14;
        offset.Y = -4;
      } else {
        offset.X = -14;
        offset.Y = -2;
      }

      this.player = player;
      visible = false;

      loadGraphic("bloodSpray", 16, 16);
      addAnimation("spray", new List<int> { 0, 1, 2, 2, 3, 3, 4 }, 20);
      addOnCompleteCallback("spray", onSprayCompleteCallback);
    }

    public void spray() {
      x = player.x;
      y = player.y;
      z = player.z;
      visible = true;
      play("spray");
      animation.reset();
    }

    void onSprayCompleteCallback(int frameIndex) {
      visible = false;
    }
  }
}
