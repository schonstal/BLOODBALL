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
  class HUD : Group {
    public const float HUGE_Z = 100000000;
    public const int SCOREBOARD_HEIGHT = 42;
    public const int SCOREBOARD_WIDTH = 322;
    public const int SCOREBOARD_OFFSET = 4;
    public const int ICON_OFFSET_X = 17;
    public const int ICON_OFFSET_Y = 1;
    public const int ICON_SIZE = 13;
    public const int ROUND_MARKER_OFFSET_X = 25;
    public const int ROUND_MARKER_OFFSET_Y = 33;
    public const int ROUND_MARKER_SPACING = 9;

    Group players;
    public Sprite scoreBoard;
    public Sprite scoreBoardBackground;

    public HUD(Group players) : base() {
      scoreBoardBackground = new Sprite(0, 0);
      scoreBoardBackground.loadGraphic("scoreBoardBackground", SCOREBOARD_WIDTH, SCOREBOARD_HEIGHT);
      scoreBoardBackground.x = (PlayState.ARENA_WIDTH - SCOREBOARD_WIDTH) / 2;
      scoreBoardBackground.y = -SCOREBOARD_HEIGHT - SCOREBOARD_OFFSET;
      scoreBoardBackground.color = Color.White;
      add(scoreBoardBackground);

      scoreBoard = new Sprite(0, 0);
      scoreBoard.loadGraphic("scoreBoard", SCOREBOARD_WIDTH, SCOREBOARD_HEIGHT);
      scoreBoard.x = (PlayState.ARENA_WIDTH - SCOREBOARD_WIDTH) / 2;
      scoreBoard.y = -SCOREBOARD_HEIGHT - SCOREBOARD_OFFSET;
      scoreBoard.color = new Color(0x2b, 0xab, 0x67);
      add(scoreBoard);

      players.Each<Player>((player) => {
        HealthBar healthBar = new HealthBar(player, scoreBoard);
        Sprite controllerIcon = new Sprite(
          healthBar.x + (player.onLeft ? -ICON_OFFSET_X : HealthBar.BAR_WIDTH + ICON_OFFSET_X - ICON_SIZE),
          healthBar.y + ICON_OFFSET_Y);
        controllerIcon.loadGraphic("controllerIcon", ICON_SIZE, ICON_SIZE);
        controllerIcon.color = scoreBoard.color;
        controllerIcon.sheetOffset.X = controllerIcon.GraphicWidth * (int)player.playerIndex;
        add(healthBar);
        add(controllerIcon);
      });

      add(new Timer(scoreBoard.color));

      for(int i = 1; i <= GameTracker.RoundsToWin; i++) {
        add(new RoundMarker(
          PlayState.ARENA_WIDTH/2 - 8 - ROUND_MARKER_OFFSET_X - (i * ROUND_MARKER_SPACING),
          scoreBoard.y + ROUND_MARKER_OFFSET_Y,
          GameTracker.RoundsWon[Team.Left] >= i, scoreBoard.color));
        add(new RoundMarker(
          PlayState.ARENA_WIDTH/2 + ROUND_MARKER_OFFSET_X + (i * ROUND_MARKER_SPACING),
          scoreBoard.y + ROUND_MARKER_OFFSET_Y,
          GameTracker.RoundsWon[Team.Right] >= i, scoreBoard.color));
      }

      z = HUGE_Z;
    }
  }
}
