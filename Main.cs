using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
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
    const int APPLE_VALUE = 4;

    int counter = 0;
    int eatCounter = 0;
    
    List<int> dirQueue= new List<int>() { 0 };

    bool eating = false;

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

        // add direction change to dirQueue
        if (keyState.IsKeyDown(Keys.Right) && dirQueue[dirQueue.Count - 1] != 0)
            dirQueue.Add(0);

        if (keyState.IsKeyDown(Keys.Down) && dirQueue[dirQueue.Count - 1] != 1)
            dirQueue.Add(1);

        if (keyState.IsKeyDown(Keys.Left) && dirQueue[dirQueue.Count - 1] != 2)
            dirQueue.Add(2);

        if (keyState.IsKeyDown(Keys.Up) && dirQueue[dirQueue.Count - 1] != 3)
            dirQueue.Add(3);

        // allow snake to eat apple
        if (snake.position[snake.length - 1] == apple.pos)
        {
            eating = true;
            apple.pos = new Vector2(rnd.Next() % 32, rnd.Next() % 24);
        }
        
        // move snake every TIME counts of counter
        counter++;
        if (counter == TIME)
        {
            for (int i = 0; i < dirQueue.Count;i++)
            {
                Console.Write(dirQueue[i]);
            }
            Console.WriteLine("\n");
            Console.WriteLine($"ddd, {dirQueue.Count}");
            // update snake dir to next in dir queue
            snake.dir = dirQueue[0];

            // remove first in queue if other values are present
            if (dirQueue.Count > 1)
                dirQueue.Remove(0);

            switch (snake.dir)
            {
                case 0:
                    snake.position.Add(new Vector2(snake.position[snake.length - 1].X + 1, snake.position[snake.length - 1].Y));

                    // delete end of snake unless snake is growing
                    if (!eating)
                        snake.position.Remove(snake.position[0]);
                    break;

                case 1:
                    snake.position.Add(new Vector2(snake.position[snake.length - 1].X, snake.position[snake.length - 1].Y + 1));

                    // delete end of snake unless snake is growing
                    if (!eating)
                        snake.position.Remove(snake.position[0]);
                    break;

                case 2:
                    snake.position.Add(new Vector2(snake.position[snake.length - 1].X - 1, snake.position[snake.length - 1].Y));

                    // delete end of snake unless snake is growing
                    if (!eating)
                        snake.position.Remove(snake.position[0]);
                    break;

                case 3:
                    snake.position.Add(new Vector2(snake.position[snake.length - 1].X, snake.position[snake.length - 1].Y - 1));

                    // delete end of snake unless snake is growing
                    if (!eating)
                        snake.position.Remove(snake.position[0]);
                    break;
            }

            if (eating && eatCounter < APPLE_VALUE)
            {
                snake.length++;
                eatCounter++;
            }

            if (eatCounter == APPLE_VALUE)
            {
                eatCounter = 0;
                eating = false;
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
