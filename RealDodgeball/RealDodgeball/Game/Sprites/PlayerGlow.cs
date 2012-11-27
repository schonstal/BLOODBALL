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
  class PlayerGlow : Sprite {
    public const float FADE_RATE = 0.3f;
    public const float START_ALPHA = 1f;
    Player player;

    public PlayerGlow(Player player) : base(player.x, player.y) {
      this.player = player;
      blend = BlendState.Additive;

      loadGraphic("player", 34, 34);
      width = player.width;
      height = player.height;
      sheetOffset = player.sheetOffset;
      offset = player.offset;

      if(player.onRight) {
        sheetOffset.X = atlas.Width / 2;
      }
      alpha = 0;
      visible = false;
    }

    public override void postUpdate() {
      x = player.x;
      y = player.y;
      z = player.z + 0.1f;
      sheetOffset = player.sheetOffset;
      offset = player.offset;

      alpha -= G.elapsed / FADE_RATE;
      if(alpha <= 0) visible = false;

      base.postUpdate();
    }

    public void flash() {
      alpha = START_ALPHA;
      visible = true;
    }
  }
}
