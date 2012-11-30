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
  public class Text : GameObject {
    public const string DEFAULT_FONT = "BloodballMenu";

    public BlendState blend = BlendState.AlphaBlend;
    public Color color = Color.White;
    public float alpha = 1.0f;
    public bool visible = true;
    public string text;
    public ScreenPositioning screenPositioning = ScreenPositioning.Absolute;
    public string font = DEFAULT_FONT;

    Vector2 screenPosition = new Vector2();
    Color alphaColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public Text(string text, float x = 0f, float y = 0f, int width = 0, int height = 0) :
        base(x, y, width, height) {
          this.text = text;
    }

    public override void Draw() {
      if(visible) {
        if(screenPositioning == ScreenPositioning.Relative) {
          screenPosition.X = (int)(G.camera.x + x);
          screenPosition.Y = (int)(-G.camera.y + y);
        } else {
          screenPosition.X = (int)(x);
          screenPosition.Y = (int)(y);
        }

        float localAlpha = MathHelper.Clamp(alpha, 0.0f, 1.0f);
        color.A = (byte)(int)Math.Floor(localAlpha == 1.0f ? 255 : localAlpha * 256.0f);

        G.camera.Render(blend, (spriteBatch) => {
          spriteBatch.DrawString(Assets.getFont(font), text, screenPosition, color);
        });
      }
    }
  }
}
