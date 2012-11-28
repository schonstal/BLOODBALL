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
    public int _roundSeconds;
    public int _totalRounds;
    public int _currentRound;
    public Dictionary<Team, int> _roundsWon = new Dictionary<Team, int>();

    private static GameTracker instance {
      get {
        if(_instance == null) {
          _instance = new GameTracker();
        }
        return _instance;
      }
    }

    public int RoundSeconds {
      get { return _instance._roundSeconds; }
      set { _instance._roundSeconds = value; }
    }

    public int TotalRounds {
      get { return _instance._totalRounds; }
      set { _instance._totalRounds = value; }
    }

    public int CurrentRound {
      get { return _instance._currentRound; }
      set { _instance._currentRound = value; }
    }

    public Dictionary<Team, int> RoundsWon {
      get { return _instance._roundsWon; }
      set { _instance._roundsWon = value; }
    }
  }
}
