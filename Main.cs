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

    KeyboardState keyState;

    const int TIME = 5;
    int counter = 0;

    // snake
    Snake snake = new Snake();

    // apple
    Apple apple = new Apple();

    // vectors
    Vector2 zero = new Vector2(0, 0);

    public Main()
    {
        // set screen resolution to 800 x 600
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

        // keyboard state
        keyState = Keyboard.GetState();

        // check if direction of snake is changing
        if (keyState.IsKeyDown(Keys.Right))
            snake.dir = 0;

        else if (keyState.IsKeyDown(Keys.Down))
            snake.dir = 1;

        else if (keyState.IsKeyDown(Keys.Left))
            snake.dir = 2;

        else if (keyState.IsKeyDown(Keys.Up))
            snake.dir = 3;

        // move snake every 1000 counts of counter
        counter++;
        if (counter == TIME)
        {
            for (int i = 0; i < snake.length; i++)
            {
                int reverseIndex = snake.length - i - 1;  // reverse of index, so when i is zero, reverseIndex will be end of list
                switch (snake.dir)
                {
                    case 0:
                        snake.position[reverseIndex] = new Vector2(snake.position[reverseIndex].X + 1, snake.position[reverseIndex].Y);
                        break;

                    case 1:
                        snake.position[reverseIndex] = new Vector2(snake.position[reverseIndex].X, snake.position[reverseIndex].Y + 1);
                        break;

                    case 2:
                        snake.position[reverseIndex] = new Vector2(snake.position[reverseIndex].X - 1, snake.position[reverseIndex].Y);
                        break;

                    case 3:
                        snake.position[reverseIndex] = new Vector2(snake.position[reverseIndex].X, snake.position[reverseIndex].Y - 1);
                        break;
                }
            }

            counter = 0;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // draw textures
        _spriteBatch.Begin();

        _spriteBatch.Draw(bgText, zero, Color.White);

        // draw apple
        _spriteBatch.Draw(appleText, apple.pos * 25, Color.White);

        // draw snake
        for (int i = 0; i < snake.length; i++)
        {
            _spriteBatch.Draw(snakeText, snake.position[i] * 25, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
