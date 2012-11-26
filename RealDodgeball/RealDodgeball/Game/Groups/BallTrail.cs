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
  class BallTrail : Group {
    public float spawnRate = 0.001f;
    public float maxCount = 50;

    Ball ball;
    float spawnTimer;

    public BallTrail(Ball ball) : base() {
      this.ball = ball;
    }

    public void spawn(int steps=1) {
      foreach(GameObject o in members) {
        ((BallParticle)o).updateAlpha(steps);
      }

      if(!active) return;

      spawnTimer += G.elapsed/(float)steps;
      if(spawnTimer > spawnRate) {
        members = members.OrderBy((p) => ((BallParticle)p).alpha).ToList();
        BallParticle oldest = (members.Count <= 0 ? null : (BallParticle)members.First());
        if(oldest != null && !oldest.visible) {
          oldest.initialize(ball.x + ball.offset.X + 3, ball.y + ball.offset.Y + 3);
          oldest.alpha = startingAlpha();
        } else if(members.Count < maxCount) {
          BallParticle newParticle = new BallParticle(ball.x + ball.offset.X, ball.y + ball.offset.Y);
          newParticle.alpha = startingAlpha();
          add(newParticle);
        }
        spawnTimer = 0.0f;
      }
    }

    public virtual float startingAlpha() {
      return 0.2f;
    }
  }
}
