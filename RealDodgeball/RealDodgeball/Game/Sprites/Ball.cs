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
    public Ball() : base(0,0) {
      loadGraphic("Dot", 12, 12);
      color = Color.DarkRed;
      height = 6;
      offset.Y = -2;
    }

    public override void Update() {
      base.Update();
    }

    public override void postUpdate() {
      base.postUpdate();
    }
  }
}
