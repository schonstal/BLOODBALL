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
    Player player;

    public HealthBar(Player player) : base() {
      this.player = player;
      switch(player.courtPosition) {
        case CourtPosition.TopLeft:
          x = 80;
          y = -40;
          break;
        case CourtPosition.TopRight:
          x = PlayState.ARENA_WIDTH - 80;
          y = -40;
          break;
        case CourtPosition.BottomLeft:
          x = 80;
          y = -20;
          break;
        case CourtPosition.BottomRight:
          x = PlayState.ARENA_WIDTH - 80;
          y = -20;
          break;
      }
    }
  }
}
