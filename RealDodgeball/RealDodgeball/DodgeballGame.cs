using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Dodgeball.Engine;
using Dodgeball.Game;

namespace Dodgeball {
  public class DodgeballGame : Microsoft.Xna.Framework.Game {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteBatch targetBatch;
    RenderTarget2D renderTarget;
    RenderTarget2D transitionTarget;

    public DodgeballGame() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      Window.AllowUserResizing = false;

      //Enable this for prod
      //graphics.IsFullScreen = true;
      //graphics.PreferredBackBufferWidth = 
      //  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
      //graphics.PreferredBackBufferHeight = 
      //  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

      //720p for debugging (and xbox?)
      graphics.PreferredBackBufferHeight = 720;
      graphics.PreferredBackBufferWidth = 1280;
      G.toggleFullscreen = graphics.ToggleFullScreen;
      Window.Title = "BLOODBALL";
    }

    protected override void Initialize() {
      //TODO: BUTTER YO SHIT
      GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      G.exit = Exit;

      //DEBUG FUCKNUGGETS
      GameTracker.CurrentRound = 0;
      GameTracker.RoundSeconds = 99;
      GameTracker.RoundsWon[Team.Left] = 0;
      GameTracker.RoundsWon[Team.Right] = 0;
      GameTracker.MatchesWon[Team.Left] = 0;
      GameTracker.MatchesWon[Team.Right] = 0;
      GameTracker.RoundsToWin = 2;
      GameTracker.TotalSeconds = 100;

      base.Initialize();
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      targetBatch = new SpriteBatch(GraphicsDevice);
      renderTarget = new RenderTarget2D(GraphicsDevice, 640, 360);
      transitionTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
      G.camera.Initialize(spriteBatch);
      //Actually put it in the right place...
      G.camera.x = (640 - PlayState.ARENA_WIDTH) / 2;
      G.camera.width = renderTarget.Width;
      G.camera.height = renderTarget.Height;

      //Debugging
      Texture2D dot = new Texture2D(GraphicsDevice, 1, 1);
      dot.SetData(new Color[] { Color.White });
      Assets.addTexture("Dot", dot);

      new List<string> { 
        "player",
        "ball",
        "ballShadow",
        "playerShadow",
        "bloodSpray",
        "scoreBoard",
        "healthBar",
        "timerDigits",
        "ballTrail",
        "controllerIcon",
        "arenaBackground",
        "arenaForeground",
        "arenaVignette",
        "scoreBoardBackground",
        "roundMarker",
        "roundMarkerBackground",
        "transition",
        "cardBackground",
        "cards",
        "roundNumber",
        "titleScreen",
        "winScreen",
        "tallies",
        "controls",
        "teamSelect",
        "begin",
        "teamSelectIcon",
        "loadingScreen"
      }.ForEach((s) => Assets.addTexture(s, Content.Load<Texture2D>(s)));

      new List<string> {
        "bounce",
        "catch",
        "chargedThrow",
        "confirm",
        "hit1",
        "hit2",
        "KO",
        "parry",
        "pickup",
        "select",
        "startButton",
        "steelGate",
        "superKO",
        "throw1",
        "throw2",
        "wireGate",
        "bading"
      }.ForEach((s) => Assets.addSound(s, Content.Load<SoundEffect>(s)));

      new List<string> {
        "GameMusic",
        "titleMusic",
        "resultsMusic"
      }.ForEach((s) => Assets.addSong(s, Content.Load<Song>(s)));
      Assets.addFont("BloodballMenu", Content.Load<SpriteFont>("BloodballMenu"));

      G.addTransition("fade", new FadeTransition());
      G.addTransition("gate", new GateTransition());
      G.switchState(new BadingState());
    }

    protected override void UnloadContent() {
    }

    protected override void Update(GameTime gameTime) {
      // TODO: Add your update logic here

      base.Update(gameTime);
      G.camera.width = renderTarget.Width;
      G.camera.height = renderTarget.Height;

      G.Update(gameTime);
      G.transitions.Update();
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.SetRenderTarget(renderTarget);

      //Draw game
      GraphicsDevice.Clear(Color.Black);
      spriteBatch.Begin();
      G.state.Draw();
      spriteBatch.End();

      GraphicsDevice.SetRenderTarget(transitionTarget);

      //Draw transitions
      GraphicsDevice.Clear(Color.Transparent);
      spriteBatch.Begin();
      G.transitions.Draw();
      spriteBatch.End();

      //Set rendering to back buffer
      GraphicsDevice.SetRenderTarget(null);
      GraphicsDevice.Clear(Color.Black);

      //Render targets
      targetBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
      targetBatch.Draw(renderTarget, new Rectangle(
          (int)G.camera.offset.X,
          (int)G.camera.offset.Y,
          GraphicsDevice.Viewport.Width,
          GraphicsDevice.Viewport.Height),
        Color.White);
      targetBatch.Draw(transitionTarget, new Rectangle(
          (int)G.camera.offset.X,
          (int)G.camera.offset.Y,
          GraphicsDevice.Viewport.Width,
          GraphicsDevice.Viewport.Height),
        Color.White);
      targetBatch.End();

      base.Draw(gameTime);
    }
  }
}
