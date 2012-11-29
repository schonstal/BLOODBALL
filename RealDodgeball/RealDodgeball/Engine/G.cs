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
  //I <3 ANTIPATTERNS
  class G {
    private static G _instance;
    public float _timeElapsed;
    public GameTime _gameTime;
    public GameState _state;
    public Camera _camera;
    public Input _input;
    public Group _transitions;
    public Dictionary<string, Transition> _transitionMap;
    public bool _visualDebug = false;

    private static G instance {
      get {
        if (_instance == null) {
          _instance = new G();
          _instance._transitionMap = new Dictionary<string,Transition>();
          _instance._transitions = new Group();
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

    public static float elapsed {
      get { return instance._timeElapsed; }
    }

    public static GameTime gameTime {
      get { return instance._gameTime; }
    }

    public static Input input {
      get { return instance._input; }
    }

    public static bool visualDebug {
      get { return instance._visualDebug; }
      set { instance._visualDebug = value; }
    }

    public static Group transitions {
      get { return instance._transitions; }
      set { instance._transitions = value; }
    }

    public G() {
      _input = new Input();
      _camera = new Camera();
    }

    public static void update(GameTime gameTime) {
      instance._timeElapsed = gameTime.ElapsedGameTime.Milliseconds/1000f;
      instance._gameTime = gameTime;
      instance._input.Update();
      instance._state.Update();
    }

    public static void switchState(GameState state, string transition = null) {
      //Maybe we'll do some destruction logic here later
      if(transition == null) {
        instance._state = state;
        instance._state.Create();
      } else {
        instance._transitionMap[transition].Start(state);
      }
    }

    public static void addTransition(string name, Transition transition) {
      instance._transitionMap.Add(name, transition);
      transitions.add(transition);
    }
  }
}