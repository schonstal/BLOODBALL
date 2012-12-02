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
using Dodgeball.Engine;

namespace Dodgeball.Game {
  class MenuText : Group {
    public bool selected = false;
    public Action onPress = null;
    public Action<int> onChange = null;
    public string bodyText = "";
    public string valueText = "";

    Vector2 offset = new Vector2();

    Color itemColor = new Color(0x77, 0x80, 0x85);

    Text hilightText;
    Text itemText;

    public MenuText(string text, Action onPress=null) : base() {
      bodyText = text;
      this.onPress = onPress;

      hilightText = new Text(text);
      hilightText.visible = false;
      hilightText.color = Color.White;

      itemText = new Text(text);
      itemText.color = itemColor;

      add(itemText);
      add(hilightText);
    }

    public override void Update() {
      itemText.x = hilightText.x = x + offset.X;
      itemText.y = y + offset.Y;
      hilightText.y = y - 1 + offset.Y;

      itemText.text = hilightText.text = bodyText + " " + valueText;

      if(selected) {
        hilightText.visible = true;
        itemText.color = Color.Black;
      } else {
        hilightText.visible = false;
        itemText.color = itemColor;
      }
      base.Update();
    }
  }
}
