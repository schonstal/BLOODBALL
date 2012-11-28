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
  class Timer : Group {
    public const int DIGIT_OFFSET_Y = -2;

    public Sprite[] digits = new Sprite[2];

    public Timer(Color digitColor) : base() {
      for(int i = 0; i < 2; i++) {
        digits[i] = new Sprite();
        digits[i].loadGraphic("timerDigits", 28, 38);
        digits[i].y = -HUD.SCOREBOARD_HEIGHT + DIGIT_OFFSET_Y;
        digits[i].color = digitColor;
        add(digits[i]);
      }
      digits[0].x = PlayState.ARENA_WIDTH/2 - 28;
      digits[1].x = PlayState.ARENA_WIDTH/2;
    }

    public override void Update() {
      if(GameTracker.RoundSeconds > 99) {
        digits[0].sheetOffset.X = digits[0].width * 10;
        digits[1].sheetOffset.X = digits[1].width * 11;
      } else {
        int tens = (int)Math.Ceiling(GameTracker.RoundSeconds) / 10;
        int ones = (int)Math.Ceiling(GameTracker.RoundSeconds) - (tens*10);
        digits[0].sheetOffset.X = digits[0].width * tens;
        digits[1].sheetOffset.X = digits[1].width * ones;
      }
      base.Update();
    }
  }
}
