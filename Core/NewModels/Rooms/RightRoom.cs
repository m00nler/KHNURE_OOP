﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.NewModels.Rooms
{
    public class Room
    {
        static Random random = new Random();
        public virtual void Create(ref BaseElement[,] map, int x, int y)
        {
            for (int i = x; i < x + 5; i++)
            {
                for (int j = y; j < y + 5; j++)
                {
                    if ((i == x) || (i == x + 4) || j == y + 4 || j == y)
                    {
                        if (i == x + 4 && j == y + 2)
                        {
                            map[i, j] = new Empty(i, j);
                            continue;
                        }
                        map[i, j] = new Wall(i, j);
                        continue;
                    }
                    int q = random.Next(100);
                    map[i, j] = q switch
                    {
                        <= 25 => new EnergyBall(i, j),
                        _ => new Empty(i, j),
                    };
                }
            }
        }
    }
}