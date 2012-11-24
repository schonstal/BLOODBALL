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
    public float frameDelay;
    bool looped;

    bool hasPlayed = false;
    bool finished = false;
    bool paused = false;

    List<Action<int>> animationCallbacks = new List<Action<int>>();
    List<Action<int>> onCompleteCallbacks = new List<Action<int>>();

    public float elapsed;

    public int currentFrame = 0;

    public float FPS {
      set { frameDelay = 1 / value; }
      get { return frameDelay; }
    }

    public Animation(List<int> frames, int fps, bool looped) {
      this.frames = frames;
      this.FPS = fps;
      this.looped = looped;
    }

    public bool Finished {
      get { return finished; }
    }

    public void play() {
      if(!paused) elapsed += G.elapsed;
      if(!hasPlayed && elapsed > frameDelay) {
        if(currentFrame < frames.Count - 1) {
          currentFrame++;
          finished = false;
          animationCallbacks.ForEach((callback) => callback(currentFrame));
        } else {
          if(looped) {
            currentFrame = 0;
          } else {
            hasPlayed = true;
          }
          onCompleteCallbacks.ForEach((callback) => callback(currentFrame));
          finished = true;
        }
        elapsed = 0;
      }
    }

    public void stop() {
      paused = true;
    }

    public void start() {
      paused = false;
    }

    public int getFrame() {
      return frames[currentFrame];
    }

    public void reset() {
      hasPlayed = false;
      currentFrame = 0;
      elapsed = 0;
    }

    public void addAnimationCallback(Action<int> callback) {
      animationCallbacks.Add(callback);
    }

    public void addOnCompleteCallback(Action<int> callaback) {
      onCompleteCallbacks.Add(callaback);
    }
  }
}
