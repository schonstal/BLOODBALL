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

    Group players;
    public Sprite scoreBoard;

    public HUD(Group players) : base() {
      scoreBoard = new Sprite(0, 0);
      scoreBoard.loadGraphic("scoreBoard", SCOREBOARD_WIDTH, SCOREBOARD_HEIGHT);
      scoreBoard.x = (PlayState.ARENA_WIDTH - SCOREBOARD_WIDTH) / 2;
      scoreBoard.y = -SCOREBOARD_HEIGHT - SCOREBOARD_OFFSET;
      scoreBoard.color = new Color(55, 189, 104);
      add(scoreBoard);

      players.Each((player) => {
        add(new HealthBar((Player)player, scoreBoard));
      });
      z = HUGE_Z;
    }
  }
}
