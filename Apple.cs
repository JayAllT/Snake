using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Apple
{
    public Vector2 pos;

    public Apple()
    {
        pos = new Vector2(Main.rnd.Next() % 32, Main.rnd.Next() % 24);
    }
}
