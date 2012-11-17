﻿using System;
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
  //I <3 ANTIPATTERNS
  class G {
    private static G _instance;
    public int _timeElapsed;
    public GameState _state;
    public Camera _camera;

    private static G instance {
      get {
        if (_instance == null) {
          _instance = new G();
        }
        return _instance;
      }
    }

    public static Camera camera {
      get { return instance._camera; }
      set { instance._camera = value; }
    }

    public static GameState state {
      get { return instance._state; }
    }

    public static int timeElapsed {
      get { return instance._timeElapsed; }
    }

    public static void updateTimeElapsed(GameTime gameTime) {
      instance._timeElapsed = gameTime.ElapsedGameTime.Milliseconds;
    }

    public static void switchState(GameState state) {
      //Maybe we'll do some destruction logic here later
      instance._state = state;
    }
  }
}