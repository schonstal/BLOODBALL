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
  class HealthBar : Group {
    public const int BAR_WIDTH = 200;

    Player player;
    Sprite background;
    Sprite temporaryHealth;
    Sprite realHealth;

    public HealthBar(Player player) : base() {
      this.player = player;
      switch(player.courtPosition) {
        case CourtPosition.TopLeft:
          x = 0;
          y = -15;
          break;
        case CourtPosition.TopRight:
          x = PlayState.ARENA_WIDTH - BAR_WIDTH;
          y = -15;
          break;
        case CourtPosition.BottomLeft:
          x = 0;
          y = PlayState.ARENA_HEIGHT + 5;
          break;
        case CourtPosition.BottomRight:
          x = PlayState.ARENA_WIDTH - BAR_WIDTH;
          y = PlayState.ARENA_HEIGHT + 5;
          break;
      }

      background = new Sprite(x, y);
      background.loadGraphic("Dot", BAR_WIDTH, 10);
      background.color = Color.White;
      add(background);
    }
  }
}
