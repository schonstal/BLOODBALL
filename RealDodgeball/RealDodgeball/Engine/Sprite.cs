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
    const string DEFAULT_ANIMATION = "__default__";

    String textureName;
    String currentAnimation = DEFAULT_ANIMATION;
    Dictionary<String, Animation> animations;
    Texture2D atlas;
    Vector2 screenPosition = new Vector2();
    Rectangle renderSlice = new Rectangle();

    public BlendState blend = BlendState.Opaque;
    public Color color = Color.White;
    public bool visible = true;
    public Vector2 offset = new Vector2();

    public bool finished {
      get { return animations[currentAnimation].Finished; }
    }

    public Animation animation {
      get { return animations[currentAnimation]; }
    }

    public Sprite(float x = 0f, float y = 0f, int width = 0, int height = 0) :
        base(x, y, width, height) {
      animations = new Dictionary<string, Animation>();
      addAnimation(DEFAULT_ANIMATION, new List<int>() { 0 });
    }

    public void loadGraphic(String textureName, int width = 0, int height = 0) {
      atlas = Assets.getTexture(textureName);
      if(width > 0) this.width = width;
      if(height > 0) this.height = height;
    }

    public void play(String animation) {
      currentAnimation = animation;
    }

    public void addAnimation(String name, List<int> frames, int fps = 15, bool looped = false) {
      animations.Add(name, new Animation(frames, fps, looped));
    }

    public override void Update() {
      animation.play();
      base.Update();
    }

    public override void Draw() {
      if(visible) {
        screenPosition.X = G.camera.x + offset.X + x;
        screenPosition.Y = G.camera.y + offset.Y + y;

        renderSlice.X = animation.getFrame() * width;
        renderSlice.Y = 0;
        renderSlice.Width = width;
        renderSlice.Height = height;

        G.camera.Render(blend, (spriteBatch) => {
          spriteBatch.Draw(atlas, screenPosition, renderSlice, color);
        });
      }
    }
  }
}
