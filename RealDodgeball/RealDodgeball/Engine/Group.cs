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
  public class Group {
    List<GameObject> members = new List<GameObject>();

    public virtual void Update() {
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

    public virtual void Draw() {
      foreach(GameObject gameObject in members) {
        gameObject.Draw();
      }
    }

    public void add(GameObject gameObject) {
      members.Add(gameObject);
    }
  }
}
