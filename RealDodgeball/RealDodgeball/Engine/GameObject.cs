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
    public float z = 0;
    public int width;
    public int height;
    Rectangle hitbox;

    public Vector2 velocity = new Vector2(0,0);
    public Vector2 acceleration = new Vector2(0,0);
    public Vector2 drag = new Vector2(0,0);
    public float linearDrag = 0f;
 
    public float maxSpeed = 0f;
    public Vector2 maxVelocity = new Vector2(0,0);

    public bool moves = true;
    public bool active = true;
    public int motionSteps = 1;
    List<Action<GameObject>> onMoveCallbacks = new List<Action<GameObject>>();

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
      if(!active) return;
    }

    public virtual void Update() {
      if(!active) return;

      if(moves) {
        for(int i = 0; i < motionSteps; i++) {
          updateMotion(motionSteps);
        }
      }
    }

    public virtual void postUpdate() {
      if(!active) return;
    }

    public virtual void Draw() {
    }

    public virtual void Render(SpriteBatch spriteBatch) {
    }

    public void addOnMoveCallback(Action<GameObject> callback) {
      onMoveCallbacks.Add(callback);
    }

    protected void updateMotion(int steps=1) {
      velocity.X += Util.computeVelocity(
          velocity.X, acceleration.X, drag.X, maxVelocity.X, steps
        ) - velocity.X;

      velocity.Y += Util.computeVelocity(
          velocity.Y, acceleration.Y, drag.Y, maxVelocity.Y, steps
        ) - velocity.Y;

      if(maxSpeed > 0 && velocity.Length() > maxSpeed) {
        velocity = Vector2.Normalize(velocity) * maxSpeed;
      }

      if(linearDrag > 0 && velocity.Length() > 0) {
        velocity -= velocity * linearDrag;
      }

      x += G.elapsed/steps * velocity.X;
      y += G.elapsed/steps * velocity.Y;

      onMoveCallbacks.ForEach((callback) => callback(this));
		}
  }
}
