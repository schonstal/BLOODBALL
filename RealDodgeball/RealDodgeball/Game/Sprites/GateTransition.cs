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
  class GateTransition : Transition {
    public const float SHAKE_SECONDS = 0.3f;
    public const int SHAKE_AMOUNT = 8;
    public const float SHAKE_RUMBLE = 0.7f;

    public const float CLOSED_SECONDS = 1f;

    List<int> openFrames;
    List<int> closeFrames;

    GameState state;

    public GateTransition() : base() {
      screenPositioning = ScreenPositioning.Absolute;
      loadGraphic("transition", 320, 180);
      closeFrames = Enumerable.Range(0, 24).ToList();
      openFrames = Enumerable.Range(24, 27).ToList();

      addAnimation("close", closeFrames, 40, false);
      addOnCompleteCallback("close", onCloseComplete);
      
      addAnimation("open", openFrames, 40, false);
      addOnCompleteCallback("open", onOpenComplete);
      
      visible = false;
    }

    public void onCloseComplete(int frameIndex) {
      G.DoForSeconds(SHAKE_SECONDS, () => {
        G.camera.offset.X = G.RNG.Next(-SHAKE_AMOUNT, SHAKE_AMOUNT);
        G.camera.offset.Y = G.RNG.Next(-SHAKE_AMOUNT, SHAKE_AMOUNT);
        Input.ForEachInput((playerIndex) => {
          GamePad.SetVibration(playerIndex, SHAKE_RUMBLE, SHAKE_RUMBLE);
        });
      }, () => {
        G.camera.offset.X = 0;
        G.camera.offset.Y = 0;
        Input.ForEachInput((playerIndex) => {
          GamePad.SetVibration(playerIndex, 0, 0);
        });
        G.switchState(state);
      });
      G.DoInSeconds(CLOSED_SECONDS, () => {
        play("open");
        animation.reset();
      });
    }

    public void onOpenComplete(int frameIndex) {
      visible = false;
    }

    public override void Start(GameState state) {
      this.state = state;
      visible = true;
      play("close");
      animation.reset();
    }

    public override void Update() {
      base.Update();
    }
  }
}
