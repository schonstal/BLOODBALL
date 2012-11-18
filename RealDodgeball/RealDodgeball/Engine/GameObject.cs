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

    public Vector2 velocity = new Vector2(0,0);
    public Vector2 acceleration = new Vector2(0,0);
    public Vector2 drag = new Vector2(0,0);
    public Vector2 maxVelocity = new Vector2(0,0);

    public bool moves = true;

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
      if(moves) updateMotion();
    }

    public virtual void Draw() {
    }

    public virtual void Render(SpriteBatch spriteBatch) {
    }

    protected void updateMotion() {
			float delta;
			float velocityDelta;

			/*velocityDelta = Util.computeVelocity(angularVelocity,angularAcceleration,angularDrag,maxAngular) - angularVelocity)/2;
			angularVelocity += velocityDelta; 
			angle += angularVelocity*G.elapsed;
			angularVelocity += velocityDelta;*/

			velocityDelta = (
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
			y += delta;
		}
  }
}
