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
    Rectangle hitbox;

    public Vector2 velocity = new Vector2(0,0);
    public Vector2 acceleration = new Vector2(0,0);
    public Vector2 drag = new Vector2(0,0);
 
    public float maxSpeed = 0f;
    public Vector2 maxVelocity = new Vector2(0,0);

    public bool moves = true;

    public Rectangle Hitbox {
      get {
        hitbox.X = (int)x;
        hitbox.Y = (int)y;
        hitbox.Width = width;
        hitbox.Height = height;
        return hitbox;
      }
    }

    public GameObject(float x = 0f, float y = 0f, int width = 0, int height = 0) {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;

      hitbox = new Rectangle((int)x, (int)y, width, height);
    }

    public virtual void preUpdate() {
    }

    public virtual void Update() {
      if(moves) updateMotion();
    }

    public virtual void postUpdate() {
    }

    public virtual void Draw() {
    }

    public virtual void Render(SpriteBatch spriteBatch) {
    }

    protected void updateMotion() {
			float delta;
			float velocityDelta;

			/*velocityDelta = (
          Util.computeVelocity(velocity.X, acceleration.X, drag.X, maxVelocity.X) -
          velocity.X
        ) / 2;
			velocity.X += velocityDelta;
			delta = velocity.X*G.elapsed;
			velocity.X += velocityDelta;
			x += delta;

			velocityDelta = (
          Util.computeVelocity(velocity.Y, acceleration.Y, drag.Y, maxVelocity.Y) -
          velocity.Y
        )/2;
			velocity.Y += velocityDelta;
			delta = velocity.Y*G.elapsed;
			velocity.Y += velocityDelta;
			y += delta;*/

      velocity.X += Util.computeVelocity(
          velocity.X, acceleration.X, drag.X, maxVelocity.X
        ) - velocity.X;

      velocity.Y += Util.computeVelocity(
          velocity.Y, acceleration.Y, drag.Y, maxVelocity.Y
        ) - velocity.Y;

      if(velocity.Length() > maxSpeed) {
        velocity = Vector2.Normalize(velocity) * maxSpeed;
      }

      x += G.elapsed * velocity.X;
      y += G.elapsed * velocity.Y;
		}
  }
}
