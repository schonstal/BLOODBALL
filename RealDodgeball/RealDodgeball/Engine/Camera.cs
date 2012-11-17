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
  public abstract class Camera {
    //I'm just going to assume we'll never need
    //To do anything fancy with spriteBatching.
    SpriteBatch spriteBatch;

    public Camera() {
    }

    public void Initialize() {
    }

    public void Update() {
    }

    public void Draw() {
    }
  }
}
