﻿using Core.Models;

namespace Core.NewModels
{
    public class Ball : BaseElement
    {
        public Ball(int x, int y) : base(x, y)
        {

        }
        public void Move(Direction direction, bool eat = false)
        {
            Clear(X,Y);
            var tempDir = direction switch
            {
                Direction.Right => X += 1,
                Direction.Left => X -= 1,
                Direction.Up => Y -= 1,
                Direction.Down => Y += 1,
            };
            Draw(X, Y);
        }
    }
}
