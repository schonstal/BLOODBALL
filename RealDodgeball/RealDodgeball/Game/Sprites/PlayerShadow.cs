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
  class PlayerShadow : Sprite {
    public float Y_OFFSET = 20;
    public float X_OFFSET = -9;

    Player player;
    /*Ian:
     * frame 0 is for any time the player doesn't have the ball or is charging,
     * frame 1 is for running forward or backward with the ball,
     * frame 2 is for running up-forward/down-backward with the ball,
     * frame 3 is for running down-forward/up-backward with the ball,
     * (4,5) are for the falling and on the ground frames of the KO animation
     * add 6 to each of those for the other team
11:57 AM 
also each frame is 9x34 pixels now so relative to the player sprite you just need a y offset of 25
 
me: ok
 
Ian: the impact frame uses frame 0 as well
     */

    public PlayerShadow(Player player, float X=0, float Y=0) : base(X,Y) {
      if(player.team == Team.Left) {
        sheetOffset.Y = 9;
        //X_OFFSET;
      }

      z = 0;
      this.player = player;

      loadGraphic("playerShadow", 34, 9);
    }

    public override void Update() {
      base.Update();
    }
  }
}
