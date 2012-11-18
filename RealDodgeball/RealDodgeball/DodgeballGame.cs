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

    public DodgeballGame() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
      //TODO: BUTTER YO SHIT
      G.camera = new Camera();
      base.Initialize();
    }

    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      G.camera.Initialize(spriteBatch);

      //Debugging
      Texture2D dot = new Texture2D(GraphicsDevice, 1, 1);
      dot.SetData(new Color[] { Color.White });
      Assets.addTexture("Dot", dot);

      G.switchState(new PlayState());
    }

    protected override void UnloadContent() {
    }

    protected override void Update(GameTime gameTime) {
      G.updateTimeElapsed(gameTime);
      G.state.Update();
      
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      SpriteBatch targetBatch = new SpriteBatch(GraphicsDevice);
      //Make the dimensions based on the screen
      RenderTarget2D target = new RenderTarget2D(GraphicsDevice, 320, 240);
      GraphicsDevice.SetRenderTarget(target);

      GraphicsDevice.Clear(Color.Black);
      spriteBatch.Begin();
      G.state.Draw();
      spriteBatch.End();
      base.Draw(gameTime);

      //set rendering back to the back buffer
      GraphicsDevice.SetRenderTarget(null);

      //render target to back buffer
      targetBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone); 
      targetBatch.Draw(target, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
      targetBatch.End();
    }
  }
}
