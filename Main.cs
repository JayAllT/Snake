using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Random rnd = new Random();

    Texture2D appleText;
    Texture2D bgText;
    Texture2D snakeText;

    // snake
    Snake snake = new Snake();

    // vectors
    Vector2 zero = new Vector2(0, 0);

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load textures
        appleText = Content.Load<Texture2D>("apple");
        bgText = Content.Load<Texture2D>("bg");
        snakeText = Content.Load<Texture2D>("snake");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // draw textures
        _spriteBatch.Begin();

        _spriteBatch.Draw(bgText, zero, Color.White);

        // draw snake
        for (int i = 0; i < snake.length; i++)
        {
            _spriteBatch.Draw(snakeText, snake.position[i] * 25, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
