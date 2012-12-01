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
  class Card : Group {
    public const float HOLD_SECONDS = 0.75f;
    public const int FRAMERATE = 30;

    Sprite background;
    Sprite text;
    Sprite roundNumber;
    Action onComplete;
    Action onReady;
    Dictionary<string, CardInfo> cards =  new Dictionary<string,CardInfo>();
    CardInfo currentCard;

    //0 == forever
    float holdSeconds = HOLD_SECONDS;

    public Card() : base() {
      background = new Sprite(0, 84);
      text = new Sprite(0, 89);
      roundNumber = new Sprite(290, 89);

      add(background);
      add(text);
      add(roundNumber);

      background.screenPositioning = text.screenPositioning = roundNumber.screenPositioning =
        ScreenPositioning.Absolute;

      background.loadGraphic("cardBackground", 640, 53);
      background.addAnimation("open", new List<int> { 0, 1, 2, 3 }, FRAMERATE);
      background.addAnimation("openLarge", new List<int> { 0, 1, 2, 4 }, FRAMERATE);
      background.addAnimation("hold", new List<int> { 4 }, FRAMERATE);
      background.addAnimation("holdLarge", new List<int> { 5 }, FRAMERATE);
      background.addAnimation("close", new List<int> { 3, 2, 1, 0 }, FRAMERATE);
      background.addAnimation("closeLarge", new List<int> { 4, 2, 1, 0 }, FRAMERATE);

      background.addOnCompleteCallback("open", onOpen);
      background.addOnCompleteCallback("openLarge", onOpen);
      background.addOnCompleteCallback("close", onClose);
      background.addOnCompleteCallback("closeLarge", onClose);

      text.loadGraphic("cards", 550, 46);
      text.addAnimation("appear", new List<int> { 0, 1 }, FRAMERATE);
      text.addAnimation("flash", new List<int> { 2, 1 }, FRAMERATE, true);
      text.addAnimation("hold", new List<int> { 1 }, FRAMERATE);
      text.addAnimation("fade", new List<int> { 0 }, FRAMERATE);
      text.addOnCompleteCallback("appear", textAppeared);
      text.addOnCompleteCallback("fade", textComplete);

      roundNumber.loadGraphic("roundNumber", 40, 40);
      roundNumber.addAnimation("appear", new List<int> { 0, 1 }, FRAMERATE);
      roundNumber.addAnimation("flash", new List<int> { 2, 1 }, FRAMERATE, true);
      roundNumber.addAnimation("hold", new List<int> { 1 }, FRAMERATE);
      roundNumber.addAnimation("fade", new List<int> { 0 }, FRAMERATE);
      roundNumber.visible = false;

      cards.Add("round", new CardInfo(false, false, true, 0));
      cards.Add("final round", new CardInfo(true, false, false, 1));
      cards.Add("start", new CardInfo(false, true, false, 2));
      cards.Add("ko", new CardInfo(false, true, false, 3));
      cards.Add("double ko", new CardInfo(false, true, false, 4));
      cards.Add("time up", new CardInfo(true, false, false, 5));
      cards.Add("magenta wins", new CardInfo(true, true, false, 6));
      cards.Add("cyan wins", new CardInfo(true, true, false, 7));
      cards.Add("paused", new CardInfo(true, false, false, 8));

      visible = false;
      text.visible = false;
      text.x = (G.camera.width - text.graphicWidth) / 2;
      roundNumber.x = text.x + 375;

      z = HUD.HUGE_Z;
    }

    void textAppeared(int frameIndex) {
      text.play(currentCard.flashes ? "flash" : "hold");
      roundNumber.play("hold");
    }

    void textComplete(int frameIndex) {
      text.visible = false;
      roundNumber.visible = false;
    }

    void onClose(int frameIndex) {
      visible = false;
      if(onComplete != null) onComplete();
    }

    void onOpen(int frameIndex) {
      text.visible = true;
      roundNumber.visible = currentCard.displayRoundNumber;
      text.play("appear");
      text.animation.reset();
      roundNumber.play("appear");
      roundNumber.animation.reset();
      background.play(currentCard.large ? "holdLarge" : "hold");
      G.DoInSeconds(holdSeconds, Close);
      if(onReady != null) onReady();
    }

    public void Close() {
      text.play("fade");
      text.animation.reset();
      roundNumber.play("fade");
      roundNumber.animation.reset();
      background.play("close");
      background.animation.reset();
    }

    public void Show(string cardName, Action onComplete=null, Action onReady=null, float holdSeconds=HOLD_SECONDS) {
      visible = true;
      this.onComplete = onComplete;
      this.onReady = onReady;
      this.holdSeconds = holdSeconds;
      currentCard = cards[cardName];
      roundNumber.sheetOffset.Y = GameTracker.CurrentRound * roundNumber.GraphicHeight;
      text.sheetOffset.Y = currentCard.offsetY * text.GraphicHeight;

      background.play(currentCard.large ? "openLarge" : "open");
      background.animation.reset();
      text.offset.Y = currentCard.large ? -1 : 0;
    }
  }

  class CardInfo {
    public bool large = false;
    public bool flashes = false;
    public bool displayRoundNumber = false;
    public int offsetY = 0;

    public CardInfo(bool large, bool flashes, bool displayRoundNumber, int offsetY) {
      this.large = large;
      this.flashes = flashes;
      this.displayRoundNumber = displayRoundNumber;
      this.offsetY = offsetY;
    }
  }
}
