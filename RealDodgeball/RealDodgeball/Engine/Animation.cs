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

namespace Dodgeball.Engine {
  public class Animation {
    public List<int> frames;
    public int frameDelay;
    bool looped;

    bool hasPlayed = false;
    bool finished = false;

    public float elapsed;

    public int currentFrame = 0;

    public float FPS {
      set { frameDelay = (int)((1 / value) * 1000); }
      get { return 60000 / (float)frameDelay; }
    }

    public Animation(List<int> frames, int fps, bool looped) {
      this.frames = frames;
      this.frameDelay = (int)((1 / (float)fps) * 1000);
      this.looped = looped;
    }

    public bool Finished {
      get { return finished; }
    }

    public void play() {
      elapsed += G.timeElapsed;
      if(!hasPlayed && elapsed > frameDelay) {
        if(currentFrame < frames.Count - 1) {
          currentFrame++;
          finished = false;
        } else {
          if(looped) {
            currentFrame = 0;
          } else {
            hasPlayed = true;
          }

          finished = true;
        }
        elapsed = 0;
      }
    }

    public int getFrame() {
      return frames[currentFrame];
    }

    public void reset() {
      hasPlayed = false;
      currentFrame = 0;
    }
  }
}
