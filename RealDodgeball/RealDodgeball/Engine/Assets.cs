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
  class Assets {
    private static Assets instance;
    public Dictionary<String, Texture2D> sprites;
    public Dictionary<String, SoundEffect> sounds;

    public static Assets Instance {
      get {
        if (instance == null) {
          instance = new Assets();
          instance.sprites = new Dictionary<string, Texture2D>();
          instance.sounds = new Dictionary<string, SoundEffect>();
        }
        return instance;
      }
    }

    public static void addTexture(String name, Texture2D texture) {
      Instance.sprites.Add(name, texture);
    }

    public static Texture2D getTexture(String name) {
      return Instance.sprites[name];
    }

    public static void addSound(String name, SoundEffect sound) {
      Instance.sounds.Add(name, sound);
    }

    public static SoundEffect getSound(String name) {
      return Instance.sounds[name];
    }
  }
}
