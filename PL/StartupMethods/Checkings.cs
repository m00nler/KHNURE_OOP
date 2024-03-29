﻿using Core.Models;
using Core.NewModels;
using System;

namespace Console.StartupMethods
{
    public class Checkings
    {
        public Direction FrameTick(Direction currentDirection, Core.NewModels.Player player, Core.NewModels.Map map)
        {
            var (dx, dy) = DirectionsDictionary.directions[currentDirection];
            foreach (var item in map.map)
            {
                var condition = player.X + dx == item.X && player.Y + dy == item.Y;
                if (condition && (item.isStopping || item.isCollecting))
                {
                    return Direction.Stop;
                }
                else if (player.X == 0 || player.Y == 0 || player.X + 1 == map.map.GetLength(0) || player.Y + 1 == map.map.GetLength(1))
                {
                    return ChangeDirection(currentDirection);
                }
            }
            return currentDirection;
        }

        public Direction FrameTickBall(Direction dir, ref Core.NewModels.Ball ball, Core.NewModels.Map map, Core.NewModels.Player player, ref int _score)
        {
            var (dx, dy) = DirectionsDictionary.directions[dir];
            if (ball.X + 1 == map.map.GetLength(0) || ball.Y + 1 == map.map.GetLength(1) || ball.X == 0 || ball.Y == 0)
            {
                return ChangeDirection(dir);
            }

            var item = map[ball.X + dx, ball.Y + dy];
            if (ball.X + dx == item.X && ball.Y + dy == item.Y)
            {
                if(item.isStopping)
                {
                    if(item.isAngleChanging)
                    {
                        ball.Action(ref map, dir);
                        return ChangeDirectionWithPlayer(dir, player);
                    }
                    else
                    {
                        return ChangeDirection(dir);
                    }
                }
                else if (item.isCollecting)
                {
                    _score++;
                    return dir;
                }

            }
            return dir;
        }

        public Direction ChangeDirectionWithPlayer(Direction dir, Core.NewModels.Player player)
        {

            if (!player.reverseSlash)
            {
                if (dir == Direction.Up || dir == Direction.Down)
                {
                    return (Direction)(((int)dir + 1) % 4);
                }
                else
                {
                    return (Direction)(Math.Abs(((int)dir - 1)) % 3);
                }
            }
            else
            {
                if (dir == Direction.Down)
                {
                    return (Direction)(Math.Abs(((int)dir - 1)) % 3);
                }
                else if (dir == Direction.Up)
                {
                    return (Direction)((int)dir + 3);
                }
                else
                {
                    return (Direction)(((int)dir + 1) % 4);
                }
            }
        }

        public Direction ChangeDirection(Direction dir)
        {
            return (Direction)(((int)dir + 2) % 4);
        }
    }
}
