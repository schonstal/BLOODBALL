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
      
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      G.state.Draw();

      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}
