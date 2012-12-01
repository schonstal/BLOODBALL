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
  public class GameTracker {
    private static GameTracker _instance;
    public float _roundSeconds;
    public float _totalSeconds;
    public int _roundsToWin;
    public int _currentRound;
    public Dictionary<Team, int> _roundsWon = new Dictionary<Team, int>();
    public Dictionary<Team, int> _matchesWon = new Dictionary<Team, int>();

    private static GameTracker instance {
      get {
        if(_instance == null) {
          _instance = new GameTracker();
        }
        return _instance;
      }
    }

    public static float RoundSeconds {
      get { return instance._roundSeconds; }
      set { instance._roundSeconds = value; }
    }

    public static float TotalSeconds {
      get { return instance._totalSeconds; }
      set { instance._totalSeconds = value; }
    }

    public static int RoundsToWin {
      get { return instance._roundsToWin; }
      set { instance._roundsToWin = value; }
    }

    public static int CurrentRound {
      get { return instance._currentRound; }
      set { instance._currentRound = value; }
    }

    public static Dictionary<Team, int> RoundsWon {
      get { return instance._roundsWon; }
      set { instance._roundsWon = value; }
    }

    public static Dictionary<Team, int> MatchesWon {
      get { return instance._matchesWon; }
      set { instance._matchesWon = value; }
    }

    public static bool TeamWon(Team team) {
      return RoundsWon[team] == RoundsToWin;
    }

    public static bool GamePoint(Team team) {
      return RoundsWon[team] == RoundsToWin - 1;
    }
  }
}
