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
    public const int BAR_OFFSET_TOP_Y = 4;
    public const int BAR_OFFSET_BOTTOM_Y = 20;

    public const float FADE_RATE = 50f;

    Player player;
    Sprite hurtHealth;
    Sprite realHealth;
    Sprite healHealth;
    Sprite scoreBoard;
    Vector2 imageIndex;
    Group healthBars = new Group();

    float hurtWidth = BAR_WIDTH;
    float healWidth = BAR_WIDTH;

    public HealthBar(Player player, Sprite scoreBoard) : base() {
      this.player = player;
      this.scoreBoard = scoreBoard;

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

      hurtHealth = new Sprite(x, y);
      hurtHealth.loadGraphic("healthBar", BAR_WIDTH, BAR_HEIGHT);
      hurtHealth.color = new Color(0xb5, 0x00, 0x05);
      healthBars.add(hurtHealth);

      realHealth = new Sprite(x, y);
      realHealth.loadGraphic("healthBar", BAR_WIDTH, BAR_HEIGHT);
      healthBars.add(realHealth);

      healHealth = new Sprite(x, y);
      healHealth.loadGraphic("healthBar", BAR_WIDTH, BAR_HEIGHT);
      healthBars.add(healHealth);

      add(healthBars);

      hurtHealth.sheetOffset = realHealth.sheetOffset = healHealth.sheetOffset =
        new Vector2(imageIndex.X * BAR_WIDTH, imageIndex.Y * BAR_HEIGHT);
    }

    public override void postUpdate() {
      realHealth.graphicWidth = (int)MathHelper.Clamp(
        BAR_WIDTH * player.HP / Player.MAX_HITPOINTS, 0, BAR_WIDTH);

      if(realHealth.graphicWidth < hurtWidth) {
        hurtWidth -= G.elapsed * FADE_RATE;
        hurtHealth.graphicWidth = (int)hurtWidth;
      } else {
        hurtHealth.graphicWidth = (int)realHealth.graphicWidth;
        hurtWidth = hurtHealth.graphicWidth;
      }

      if(realHealth.graphicWidth > healWidth) {
        healWidth += G.elapsed * FADE_RATE;
        healHealth.graphicWidth = (int)healWidth;
      } else {
        healHealth.graphicWidth = (int)realHealth.graphicWidth;
        healWidth = healHealth.graphicWidth;
      }

      if(player.onLeft) {
        healthBars.Each<Sprite>((bar) => {
          bar.sheetOffset.X = BAR_WIDTH - bar.graphicWidth;
          bar.x = x + BAR_WIDTH - bar.graphicWidth;
        });
      }

      if(healHealth.graphicWidth == BAR_WIDTH) {
        healHealth.color = scoreBoard.color;
      } else {
        healHealth.color = new Color(0xbe, 0xb9, 0x10);
      }
      base.postUpdate();
    }
  }
}
