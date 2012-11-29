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
  class FadeTransition : Transition {
    public const float FADE_RATE = 0.3f;

    bool forward = false;
    GameState state;

    public FadeTransition() : base() {
      screenPositioning = ScreenPositioning.Absolute;
      loadGraphic("Dot", G.camera.width, G.camera.height);
      color = Color.Black;
      alpha = 0;
      visible = false;
    }

    public override void Start(GameState state) {
      forward = true;
      visible = true;
      alpha = 0;
      this.state = state;
    }

    public override void Update() {
      if(!forward) {
        alpha -= G.elapsed / FADE_RATE;
        if(alpha <= 0) visible = false;
      } else {
        alpha += G.elapsed / FADE_RATE;
        if(alpha >= 1) {
          forward = false;
          G.switchState(state);
        }
      }
      base.Update();
    }
  }
}
