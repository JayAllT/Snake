using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Apple
{
    public Vector2 pos = new Vector2(-1, -1);

    public Apple(Snake snake)
    {
        pos = new Vector2(Main.rnd.Next() % 31, Main.rnd.Next() % 24);

        // make sure apple doesn't spawn in snake
        while (snake.position.Contains(pos))
            pos = new Vector2(Main.rnd.Next() % 32, Main.rnd.Next() % 24);
    }
}
