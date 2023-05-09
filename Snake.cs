using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Snake
{
    // keeps track of places snake is, in order
    public List<Vector2> position = new List<Vector2>();
    public Vector2 headPos = new Vector2();

    public int length = 1;

    // 0 = right, 1 = down, 2 = left, 3 = up
    public int dir = 0;

    public Snake()
    {
        // snake spawn pos
        headPos = new Vector2(Main.rnd.Next() % 33, Main.rnd.Next() % 24);
        position.Add(headPos);
    }
}
