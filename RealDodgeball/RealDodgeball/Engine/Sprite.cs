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
  public class Sprite : GameObject {
    String textureName;
    String animationIndex = "__default__";

    public BlendState blend = BlendState.Opaque;
    public Color color = Color.White;
    public bool visible = true;

    private Dictionary<String, Animation> animations;

    public bool Finished {
      get { return CurrentAnimation.Finished; }
    }

    public Animation CurrentAnimation {
      get { return animations[animationIndex]; }
    }

    public Sprite(String texture, int x, int y, int width, int height) {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
      this.textureName = texture;

      animations = new Dictionary<string, Animation>();
      addAnimation("__default__", new List<int>() { 0 });
    }

    public void play(String animation) {
      animations[animation].play();
      animationIndex = animation;
    }

    public void addAnimation(String name, List<int> frames, int fps = 15, bool looped = false) {
      animations.Add(name, new Animation(frames, fps, looped));
    }

    public void Draw() {
      if(visible) {
        G.camera.Render(blend, (spriteBatch) => {
          spriteBatch.Draw(Assets.getTexture(textureName),
            new Vector2(G.camera.x + x, G.camera.y + y),
            new Rectangle(CurrentAnimation.getFrame() * width, 0, width, height),
            color);
        });
      }
    }
  }
}
