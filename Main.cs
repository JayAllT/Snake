using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Random rnd = new Random();

    Texture2D appleText;
    Texture2D bgText;
    Texture2D snakeText;

    SpriteFont lengthFont;
    SpriteFont deadFont;

    KeyboardState keyState;

    const int TIME = 5;
    const int APPLE_VALUE = 4;

    int counter = 0;
    int eatCounter = 0;

    // keep track of if key was just pressed
    bool keyDownRight = false;
    bool keyDownLeft = false;
    bool keyDownDown = false;
    bool keyDownUp = false;
    
    // queue of movement inputs
    List<int> dirQueue= new List<int>() { -1 };

    bool eating = false;
    bool dead = false;

    // snake
    Snake snake = new Snake();

    // apple
    Apple apple;

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
        // spawn apple
        apple = new Apple(snake);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load textures
        appleText = Content.Load<Texture2D>("apple");
        bgText = Content.Load<Texture2D>("bg");
        snakeText = Content.Load<Texture2D>("snake");

        // fonts
        lengthFont = Content.Load<SpriteFont>("GameFont");
        deadFont = Content.Load<SpriteFont>("DeadFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (!dead)
        {
            // keyboard state
            keyState = Keyboard.GetState();

            // get old direction in case a bad direction is added and needs to be replaced
            int oldDir = dirQueue[0];

            // add direction change to dirQueue if direction value is not already in dirqueue and key isn't being held continuously
            if (keyState.IsKeyDown(Keys.Right) && !dirQueue.Contains(0) && !keyDownRight)
                dirQueue.Add(0);

            if (keyState.IsKeyDown(Keys.Down) && !dirQueue.Contains(1) && !keyDownDown)
                dirQueue.Add(1);

            if (keyState.IsKeyDown(Keys.Left) && !dirQueue.Contains(2) && !keyDownLeft)
                dirQueue.Add(2);

            if (keyState.IsKeyDown(Keys.Up) && !dirQueue.Contains(3) && !keyDownUp)
                dirQueue.Add(3);

            // allow snake to eat apple
            if (snake.position[snake.length - 1] == apple.pos)
            {
                eating = true;
                apple = new Apple(snake);
            }

            // move snake every TIME counts of counter
            counter++;
            if (counter == TIME)
            {
                // remove first in queue if other values are present
                if (dirQueue.Count > 1)
                    dirQueue.Remove(dirQueue[0]);

                // do not allow snake to go backwards into itself
                if (snake.length > 2)
                {
                    if (snake.position[snake.length - 2] == new Vector2(snake.position[snake.length - 1].X + 1, snake.position[snake.length - 1].Y) && dirQueue[0] == 0)
                    {
                        dirQueue.Remove(dirQueue[0]);
                        dirQueue.Insert(0, oldDir);
                    }

                    if (snake.position[snake.length - 2] == new Vector2(snake.position[snake.length - 1].X, snake.position[snake.length - 1].Y + 1) && dirQueue[0] == 1)
                    {
                        dirQueue.Remove(dirQueue[0]);
                        dirQueue.Insert(0, oldDir);
                    }

                    if (snake.position[snake.length - 2] == new Vector2(snake.position[snake.length - 1].X - 1, snake.position[snake.length - 1].Y) && dirQueue[0] == 2)
                    {
                        dirQueue.Remove(dirQueue[0]);
                        dirQueue.Insert(0, oldDir);
                    }

                    if (snake.position[snake.length - 2] == new Vector2(snake.position[snake.length - 1].X, snake.position[snake.length - 1].Y - 1) && dirQueue[0] == 3)
                    {
                        dirQueue.Remove(dirQueue[0]);
                        dirQueue.Insert(0, oldDir);
                    }
                }

                // update snake dir to next in dir queue
                snake.dir = dirQueue[0];

                // move snake
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

                // iterate eatCounter
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

                // check if snake has hit itself
                for (int i = 0; i < snake.length - 1; i++)
                    if (snake.position[snake.length - 1] == snake.position[i])
                        dead = true;
                
                // check if snake is out of bounds
                if (snake.position[snake.length - 1].X > 31 || snake.position[snake.length - 1].Y > 23 || snake.position[snake.length - 1].X < 0 || snake.position[snake.length - 1].Y < 0)
                    dead = true;

                counter = 0;
            }

            // update key press bools
            keyDownRight = keyState.IsKeyDown(Keys.Right) ? true : false;
            keyDownDown = keyState.IsKeyDown(Keys.Down) ? true : false;
            keyDownLeft = keyState.IsKeyDown(Keys.Left) ? true : false;
            keyDownUp = keyState.IsKeyDown(Keys.Up) ? true : false;

            base.Update(gameTime);
        }

        else  // restart if enter is pressed
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Enter))
                Restart();
        }
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

        if (dead)
        {
            // display you died message and score/try again message
            _spriteBatch.DrawString(deadFont, "YOU DIED", new Vector2(400 - deadFont.MeasureString("YOU DIED").X / 2, 300 - deadFont.MeasureString("YOU DIED").Y / 2 - 20), Color.White);
            _spriteBatch.DrawString(lengthFont, $"Final Score: {snake.length} | Press ENTER to try again", new Vector2(400 - lengthFont.MeasureString($"Final Score: {snake.length} | Press ENTER to try again").X / 2, 300 - lengthFont.MeasureString($"Final Score: {snake.length} | Press ENTER to try again").Y / 2 + 30), Color.White);
        }

        // display length
        string lengthDisplay = $"Score: {snake.length}";
        _spriteBatch.DrawString(lengthFont, lengthDisplay, new Vector2(10, 590 - lengthFont.MeasureString(lengthDisplay).Y), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void Restart()
    {
        // reset snake
        snake.length = 1;
        snake.position.Clear();
        snake.position.Add(new Vector2(rnd.Next() % 31, rnd.Next() % 24));
        snake.dir = -1;
        eatCounter = 0;
        eating = false;
        counter = 0;

        dirQueue.Clear();
        dirQueue.Add(-1);

        // reset apple
        apple.pos = new Vector2(rnd.Next() % 31, rnd.Next() % 24);

        dead = false;
    }
}
