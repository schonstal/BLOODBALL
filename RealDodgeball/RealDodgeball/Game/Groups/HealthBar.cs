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
    public const int BAR_WIDTH = 125;
    public const int BAR_HEIGHT = 12;

    public const int BAR_OFFSET_X = 2;
    public const int BAR_OFFSET_TOP_Y = 3;
    public const int BAR_OFFSET_BOTTOM_Y = 19;

    public const float FADE_RATE = 50f;

    Player player;
    Sprite temporaryHealth;
    Sprite realHealth;
    Vector2 imageIndex;
    Group healthBars = new Group();

    float tempWidth = BAR_WIDTH;

    public HealthBar(Player player, Sprite scoreBoard) : base() {
      this.player = player;
      switch(player.courtPosition) {
        case CourtPosition.TopLeft:
          x = ((PlayState.ARENA_WIDTH - HUD.SCOREBOARD_WIDTH) / 2) + BAR_OFFSET_X;
          y = -HUD.SCOREBOARD_HEIGHT - HUD.SCOREBOARD_OFFSET + BAR_OFFSET_TOP_Y;
          imageIndex = new Vector2(0, 0);
          break;
        case CourtPosition.TopRight:
          x = scoreBoard.x + scoreBoard.width - BAR_WIDTH - BAR_OFFSET_X;
          y = -HUD.SCOREBOARD_HEIGHT - HUD.SCOREBOARD_OFFSET + BAR_OFFSET_TOP_Y;
          imageIndex = new Vector2(1, 0);
          break;
        case CourtPosition.BottomLeft:
          x = ((PlayState.ARENA_WIDTH - HUD.SCOREBOARD_WIDTH) / 2) + BAR_OFFSET_X;
          y = -HUD.SCOREBOARD_HEIGHT - HUD.SCOREBOARD_OFFSET + BAR_OFFSET_BOTTOM_Y;
          imageIndex = new Vector2(0, 1);
          break;
        case CourtPosition.BottomRight:
          x = scoreBoard.x + scoreBoard.width - BAR_WIDTH - BAR_OFFSET_X;
          y = -HUD.SCOREBOARD_HEIGHT - HUD.SCOREBOARD_OFFSET + BAR_OFFSET_BOTTOM_Y;
          imageIndex = new Vector2(1, 1);
          break;
      }

      temporaryHealth = new Sprite(x, y);
      temporaryHealth.loadGraphic("healthBar", BAR_WIDTH, BAR_HEIGHT);
      temporaryHealth.color = new Color(227, 0, 0);
      healthBars.add(temporaryHealth);

      realHealth = new Sprite(x, y);
      realHealth.loadGraphic("healthBar", BAR_WIDTH, BAR_HEIGHT);
      realHealth.color = new Color(217, 206, 11);
      healthBars.add(realHealth);

      add(healthBars);

      temporaryHealth.sheetOffset = realHealth.sheetOffset =
        new Vector2(imageIndex.X * BAR_WIDTH, imageIndex.Y * BAR_HEIGHT);
    }

    public override void postUpdate() {
      realHealth.graphicWidth = (int)MathHelper.Clamp(
        BAR_WIDTH * player.HP / Player.MAX_HITPOINTS, 0, BAR_WIDTH);
      if(realHealth.graphicWidth < tempWidth) {
        tempWidth -= G.elapsed * FADE_RATE;
        temporaryHealth.graphicWidth = (int)tempWidth;
      } else {
        temporaryHealth.graphicWidth = realHealth.graphicWidth;
      }

      if(player.onLeft) {
        healthBars.EachSprite((bar) => {
          bar.sheetOffset.X = BAR_WIDTH - bar.graphicWidth;
          bar.x = x + BAR_WIDTH - bar.graphicWidth;
        });
      }
      base.postUpdate();
    }
  }
}
