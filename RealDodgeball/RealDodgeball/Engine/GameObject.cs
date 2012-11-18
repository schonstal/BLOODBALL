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
  public class GameObject {
    public float x;
    public float y;
    public int width;
    public int height;

    public Vector2 velocity = new Vector2();
    public Vector2 acceleration = new Vector2();

    public GameObject(float x = 0f, float y = 0f, int width = 0, int height = 0) {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
    }

    public virtual void preUpdate() {
    }

    public virtual void Update() {
    }

    public virtual void postUpdate() {
    }

    public virtual void Draw() {
    }

    public virtual void Render(SpriteBatch spriteBatch) {
    }
  }
}
