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

namespace Dodgeball.Engine {
  public class Camera : GameObject {
    SpriteBatch spriteBatch = null;
    BlendState currentBlendState = null;

    public Camera() {
    }

    public void Initialize(SpriteBatch spriteBatch) {
      this.spriteBatch = spriteBatch;
    }

    public void Render(BlendState blendState, Action<SpriteBatch> draw) {
      if(blendState == null) return;
      if(blendState == currentBlendState) {
        draw(spriteBatch);
      } else {
        spriteBatch.End();
        currentBlendState = blendState;
        spriteBatch.Begin(SpriteSortMode.Deferred, blendState);
        draw(spriteBatch);
      }
    }
  }
}
