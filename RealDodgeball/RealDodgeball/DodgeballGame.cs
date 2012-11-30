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

    }

    protected override void Initialize() {
      //TODO: BUTTER YO SHIT
      GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

      //DEBUG FUCKNUGGETS
      GameTracker.CurrentRound = 3;
      GameTracker.RoundSeconds = 99;
      GameTracker.RoundsWon[Team.Left] = 1;
      GameTracker.RoundsWon[Team.Right] = 2;
      GameTracker.RoundsToWin = 3;
      GameTracker.TotalSeconds = 99;

      base.Initialize();
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      targetBatch = new SpriteBatch(GraphicsDevice);
      renderTarget = new RenderTarget2D(GraphicsDevice,
        GraphicsDevice.Viewport.Width / 2,
        GraphicsDevice.Viewport.Height / 2);
      transitionTarget = new RenderTarget2D(GraphicsDevice,
        GraphicsDevice.Viewport.Width / 4,
        GraphicsDevice.Viewport.Height / 4);
      G.camera.Initialize(spriteBatch);
      //Actually put it in the right place...
      G.camera.x = (GraphicsDevice.Viewport.Width/2 - PlayState.ARENA_WIDTH)/2;
      G.camera.y = -200;
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
        "roundNumber"
      }.ForEach((s) => Assets.addTexture(s, Content.Load<Texture2D>(s)));

      Assets.addSong("GameMusic", Content.Load<Song>("GameMusic"));

      G.addTransition("fade", new FadeTransition());
      G.addTransition("gate", new GateTransition());
      G.switchState(new PlayState());
    }

    protected override void UnloadContent() {
    }

    protected override void Update(GameTime gameTime) {

      if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
        G.switchState(new PlayState(true));//, "gate");      //Actually put it in the right place...
      }

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
