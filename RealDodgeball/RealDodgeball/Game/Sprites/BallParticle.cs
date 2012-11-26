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
  class BallParticle : Sprite {
    public float decayRate = 0.5f;
    public float startingAlpha = 1.0f;

    public BallParticle(float X=0f, float Y=0f) : base(X, Y) {
      loadGraphic("ballTrail", 10, 10);
      blend = BlendState.Additive;
    }

    public BallParticle initialize(float X=0f, float Y=0f) {
      visible = true;
      alpha = startingAlpha;
      x = X;
      y = Y;
      return this;
    }

    public void updateAlpha(int steps) {
      alpha -= (G.elapsed/steps) / decayRate;
      if(alpha <= 0) visible = false;
    }

  }
}
