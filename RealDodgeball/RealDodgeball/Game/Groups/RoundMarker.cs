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
  class RoundMarker : Group {
    Sprite marker;
    Sprite markerShadow;

    public RoundMarker(float X, float Y, bool lit, Color color) : base() {
      marker = new Sprite(X, Y);
      markerShadow = new Sprite(X, Y);

      marker.loadGraphic("roundMarker", 8, 8);
      markerShadow.loadGraphic("roundMarkerBackground", 8, 8);

      marker.color = color;

      if(lit) {
        marker.sheetOffset.X = markerShadow.sheetOffset.X = 8;
      }

      add(markerShadow);
      add(marker);
    }
  }
}
