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
  public class Sprite {
    String textureName;
    String animationIndex = "__default__";

    public Vector2 position = new Vector2(0, 0);
    public int width;
    public int height;

    public bool visible = true;

    private Dictionary<String, Animation> animations;

    public float X {
      get { return position.X; }
      set { position.X = value; }
    }

    public float Y {
      get { return position.Y; }
      set { position.Y = value; }
    }

    public bool Finished {
      get { return CurrentAnimation.Finished; }
    }

    public Animation CurrentAnimation {
      get { return animations[animationIndex]; }
    }

    public Sprite(String texture, int x, int y, int width, int height) {
      position.X = x;
      position.Y = y;
      this.width = width;
      this.height = height;
      this.textureName = texture;

      animations = new Dictionary<string, Animation>();
      addAnimation("__default__", new List<int>() { 0 });
    }

    public void play(String animation, GameTime gameTime) {
      animations[animation].play(gameTime);
      animationIndex = animation;
    }

    public void addAnimation(String name, List<int> frames, int fps = 15, bool looped = false) {
      animations.Add(name, new Animation(frames, fps, looped));
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      if(visible) {
        spriteBatch.Draw(Assets.getTexture(textureName),
          position,
          new Rectangle(CurrentAnimation.getFrame() * width, 0, width, height),
          Color.White);
      }
    }
  }
}
