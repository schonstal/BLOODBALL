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
  public class GameState {
    List<GameObject> gameObjects = new List<GameObject>();

    public virtual void Create() {
    }

    public virtual void Update() {
      //gameObjects.ForEach((o) => { o.Update(); });
      foreach(GameObject gameObject in gameObjects) {
        gameObject.Update();
      }
    }

    public virtual void Draw() {
      foreach(GameObject gameObject in gameObjects) {
        gameObject.Draw();
      }
    }

    public void add(GameObject gameObject) {
      gameObjects.Add(gameObject);
    }
  }
}
