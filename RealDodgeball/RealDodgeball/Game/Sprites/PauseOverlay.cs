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
  class PauseOverlay : Sprite {
    public const float FADE_RATE = 0.3f;
    public const float FINAL_ALPHA = 0.6f;

    bool forward = false;

    public PauseOverlay() : base() {
      screenPositioning = ScreenPositioning.Absolute;
      loadGraphic("Dot", G.camera.width, G.camera.height);
      color = Color.Black;
      alpha = 0;
      visible = false;
    }

    public void Start() {
      forward = true;
      visible = true;
      alpha = 0;
    }

    public void End() {
      forward = false;
      alpha = FINAL_ALPHA;
    }

    public override void Update() {
      if(!forward) {
        alpha -= G.elapsed / FADE_RATE;
        if(alpha <= 0) visible = false;
      } else {
        alpha += G.elapsed / FADE_RATE;
        if(alpha >= FINAL_ALPHA) alpha = FINAL_ALPHA;
      }
      base.Update();
    }
  }
}
