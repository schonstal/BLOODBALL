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
  public class Group : GameObject {
    protected List<GameObject> members = new List<GameObject>();

    public List<GameObject> Members {
      get { return members; }
    }

    public override void Update() {
      //gameObjects.ForEach((o) => { o.Update(); });
      foreach(GameObject gameObject in members) {
        gameObject.preUpdate();
      }
      foreach(GameObject gameObject in members) {
        gameObject.Update();
      }
      foreach(GameObject gameObject in members) {
        gameObject.postUpdate();
      }
    }

    public override void Draw() {
      foreach(GameObject gameObject in members) {
        gameObject.Draw();
      }

      //Draw debug boxes if necessary
      if(G.visualDebug) {
        members.ForEach((o) => {
          Rectangle offsetBox = new Rectangle(
            o.Hitbox.X + (int)G.camera.x,
            o.Hitbox.Y + (int)G.camera.y,
            o.Hitbox.Width,
            o.Hitbox.Height);

          G.camera.Render(BlendState.AlphaBlend, (spriteBatch) => {
            //Up
            spriteBatch.Draw(Assets.getTexture("Dot"),
              new Rectangle(offsetBox.Left, offsetBox.Top, offsetBox.Width, 1),
              Color.Red);
            //Down
            spriteBatch.Draw(Assets.getTexture("Dot"),
              new Rectangle(offsetBox.Left, offsetBox.Bottom, offsetBox.Width, 1),
              Color.Red);
            //Left
            spriteBatch.Draw(Assets.getTexture("Dot"),
              new Rectangle(offsetBox.Left, offsetBox.Top, 1, offsetBox.Height),
              Color.Red);
            //Right
            spriteBatch.Draw(Assets.getTexture("Dot"),
              new Rectangle(offsetBox.Right, offsetBox.Top, 1, offsetBox.Height + 1),
              Color.Red);
          });
        });
      }
    }

    public void add(GameObject gameObject) {
      members.Add(gameObject);
    }

    public void remove(GameObject gameObject) {
      members.Remove(gameObject);
    }
  }
}
