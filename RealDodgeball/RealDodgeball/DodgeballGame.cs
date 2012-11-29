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
      GameTracker.CurrentRound = 0;
      GameTracker.RoundSeconds = 99;
      GameTracker.RoundsWon[Team.Left] = 0;
      GameTracker.RoundsWon[Team.Right] = 0;
      GameTracker.RoundsToWin = 3;
      GameTracker.TotalSeconds = 99;

      base.Initialize();
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      targetBatch = new SpriteBatch(GraphicsDevice);
      renderTarget = new RenderTarget2D(GraphicsDevice,
        GraphicsDevice.Viewport.Width/2,
        GraphicsDevice.Viewport.Height/2);
      G.camera.Initialize(spriteBatch);
      //Actually put it in the right place...
      G.camera.x = (GraphicsDevice.Viewport.Width/2 - PlayState.ARENA_WIDTH)/2;
      G.camera.y = -200;

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
        "roundMarkerBackground"
      }.ForEach((s) => Assets.addTexture(s, Content.Load<Texture2D>(s)));

      Assets.addSong("GameMusic", Content.Load<Song>("GameMusic"));

      G.switchState(new PlayState());
    }

    protected override void UnloadContent() {
    }

    protected override void Update(GameTime gameTime) {
      G.update(gameTime);

      if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
        G.switchState(new PlayState());      //Actually put it in the right place...
        G.camera.x = (GraphicsDevice.Viewport.Width / 2 - PlayState.ARENA_WIDTH) / 2;
        G.camera.y = -200;
      }

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      //Make the dimensions based on the screen
      GraphicsDevice.SetRenderTarget(renderTarget);

      //Draw game
      GraphicsDevice.Clear(Color.Black);
      spriteBatch.Begin();
      G.state.Draw();
      spriteBatch.End();

      //set rendering back to the back buffer
      GraphicsDevice.SetRenderTarget(null);

      //render target to back buffer
      targetBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone); 
      targetBatch.Draw(renderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
      targetBatch.End();

      base.Draw(gameTime);
    }
  }
}
