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
    Group players;

    public HUD(Group players) : base() {
      players.Each((player) => {
        add(new HealthBar((Player)player));
      });
      z = HUGE_Z;
    }
  }
}
