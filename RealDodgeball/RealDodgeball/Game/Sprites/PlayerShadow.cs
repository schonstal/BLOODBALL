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
  class PlayerShadow : Sprite {
    public float Y_OFFSET = 15;
    public float X_OFFSET = -9;

    Player player;

    public PlayerShadow(Player player, float X=0, float Y=0) : base(X,Y) {
      addAnimation("idle", new List<int> { 0 }, 10, true);

      addAnimation("runForward", new List<int> { 0 }, 15, true);
      addAnimation("runBackward", new List<int> { 0 }, 15, true);
      addAnimation("runUpForward", new List<int> { 1 }, 15, true);
      addAnimation("runDownForward", new List<int> { 2 }, 15, true);
      addAnimation("runUpBackward", new List<int> { 2 }, 15, true);
      addAnimation("runDownBackward", new List<int> { 1 }, 15, true);

      addAnimation("throw", new List<int> { 0 }, 10, false);
      addAnimation("throwReturn", new List<int> { 0 }, 20, false);

      addAnimation("hurt", new List<int> { 0 });
      addAnimation("hurtFall", new List<int> { 1, 2 }, 10, false);
      addAnimation("hurtRecover", new List<int> { 1, 1 }, 20, false);

      z = 0;
      this.player = player;

      loadGraphic("playerShadow", 34, 9);
      height = 21;
      offset.Y = 6;

      if(player.onRight) {
        sheetOffset.X = atlas.Width / 2;
      }
    }

    public override void Update() {
      base.Update();
    }
  }
}
