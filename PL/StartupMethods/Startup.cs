﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BLL;
using System.Linq;
using BLL.Interfaces;
using Core.Models;
using Core.NewModels;
using System.Threading;

namespace PL.StartupMethods
{
    public class Startup
    {
        private readonly IUserService _userService;
        private Dictionary<Direction, (int,int)> directions = new Dictionary<Direction, (int,int)>();
        Core.NewModels.Player player;
        Core.NewModels.Ball ball;
        public Startup(IUserService service)
        {
            _userService = service;
        }

        private const int _mapWidth = 60;
        private const int _mapHeight = 40;

        private const int _screenWidth = _mapWidth * 2;
        private const int _screenHeight = _mapHeight * 2;

        private int _frameRate = 100;

        private readonly ConsoleMethods _console= new ConsoleMethods();
        private readonly Methods _console2= new Methods();
        private readonly DrawMenu _drawMenu = new DrawMenu();

        //public void StartGame()
        //{
        //    //await _userService.AddUser(new User() {Name = "Moonler", Password = "123", Record = "123" });
        //    _console.SetConsole(_screenWidth, _screenHeight);
        //    var menu = new DrawMainMenu();
        //    menu.DrawMenu();
        //    if (Console.ReadKey(true).Key == ConsoleKey.D1)
        //    {
        //        Console.Clear();
        //        _drawMenu.DrawMenuConsole();
        //        Console.ReadKey();
        //        Console.Clear();
        //        Console.CursorVisible = false;

        //        var drawBorder = new DrawBorder(_mapWidth, _mapHeight);
        //        var music = new Music();
        //        var tp = new Teleport(new Random().Next(1, 55), new Random().Next(5, 35));
        //        var timeWatch = new TimeCheck();

        //        var ball = new Ball(10, 5);

        //        var item = new Items();
        //        var items = new List<Pixel>();
        //        var walls = new List<Pixel>();
        //        var player = new Player(8, 4);

        //        _console.DrawItems(drawBorder, music, tp, out items, out walls, player, ball, item);

        //        var count = items.Count;

        //        int score = 0;

        //        var movement = new Movement();

        //        Direction currentDirection = Direction.Right;
        //        Direction currentDirectionWall = Direction.Stop;

        //        Stopwatch sw = new Stopwatch();

        //        timeWatch.StartTimeChecking();
        //        while (true)
        //        {
        //            sw.Restart();
        //            var oldMovementWall = currentDirectionWall;
        //            bool changeWall = false;

        //            while (sw.ElapsedMilliseconds <= _frameRate)
        //            {
        //                if (currentDirectionWall == oldMovementWall)
        //                {
        //                    currentDirectionWall = movement.ReadMovement(currentDirectionWall, ref changeWall);
        //                }
        //            }
        //            player = _console.PlayerCheckings(player, ref ball, ref currentDirection, ref currentDirectionWall, ref walls, ref items);
        //            ball = _console.BallCheckings(ball, ref items, ref score, ref currentDirection, ref walls, ref tp);
        //            ball.Move(currentDirection);
        //            player.Move(currentDirectionWall, changeWall);
        //            changeWall = false;
        //            currentDirectionWall = Direction.Stop;
        //            if (score == count)
        //            {
        //                break;
        //            }
        //        }
        //        ball.Clear();
        //        player.Clear();
        //        tp.Clear();
        //        foreach (var i in walls)
        //        {
        //            i.Clear();
        //        }
        //        foreach (var i in items)
        //        {
        //            i.Clear();
        //        }

        //        Console.SetCursorPosition(_screenWidth / 3, _screenHeight / 3);

        //        timeWatch.StopTimeChecking();

        //        Console.Write($"Game over, your score is {score}");

        //        Console.ReadKey();
        //    }
        //    else if (Console.ReadKey(true).Key == ConsoleKey.D2)
        //    {
        //        Console.WriteLine("Ok...");
        //    }
        //    else if(Console.ReadKey(true).Key == ConsoleKey.Escape)
        //    {
        //        Console.WriteLine("It's not right button, but ok, you won (Easter egg for Vitaliy Nikolaevich)");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Bruh, learn how to read");
        //    }
        //}

        public void StartGame()
        {
            directions.Add(Direction.Right, (1, 0));
            directions.Add(Direction.Left, (-1, 0));
            directions.Add(Direction.Up, (0, -1));
            directions.Add(Direction.Down, (0, 1));
            _console.SetConsole(100, 60);
            //var drawBorder = new DrawBorder(_mapWidth, _mapHeight);
            //drawBorder.Draw();
            _console2.CreateMap();
            foreach(var i in _console2.map)
            {
                if(i is Core.NewModels.Player)
                {
                    player = (Core.NewModels.Player)i;
                }
                else if(i is Core.NewModels.Ball)
                {
                    ball = (Core.NewModels.Ball)i;
                }
            }
            Console.ReadKey();
            Console.CursorVisible = false;
            var timeWatch = new TimeCheck();
            Direction currentDirection = Direction.Right;
            Direction currentDirectionPlayer = Direction.Stop;
            var oldMovementPlayer = currentDirectionPlayer;
            var movement = new Movement();
            var count = 0;
            while (true)
            {
                bool changeWall = false;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int x = player.X, y = player.Y;
                int ballX = ball.X, ballY = ball.Y;
                while (sw.ElapsedMilliseconds <= _frameRate)
                {
                    if (currentDirectionPlayer == oldMovementPlayer)
                    {
                        currentDirectionPlayer = movement.ReadMovement(currentDirectionPlayer, ref changeWall);
                        currentDirectionPlayer = FrameTick(currentDirectionPlayer, player);
                        player.Move(currentDirectionPlayer, false);
                        _console2.map[x, y] = new BaseElement(x, y);
                        _console2.map[player.X, player.Y] = new Core.NewModels.Player(player.X, player.Y);
                    }
                }

                currentDirectionPlayer = FrameTick(currentDirectionPlayer, player);
                currentDirection = FrameTickBall(currentDirection, ball);
                ball.Move(currentDirection);
                _console2.map[ballX, ballY] = new BaseElement(ballX, ballY);
                _console2.map[ball.X, ball.Y] = new Core.NewModels.Ball(ball.X, ball.Y);
                _console2.UpdateMap();
                currentDirectionPlayer = Direction.Stop;
                sw.Reset();
            }
        }

        private Direction FrameTick(Direction currentDirection, Core.NewModels.Player player)
        {
            var selectedDirection = directions.Where(d => d.Key == currentDirection).FirstOrDefault();
            foreach (var item in _console2.map)
            {
                if(player.X + selectedDirection.Value.Item1 == item.X && player.Y + selectedDirection.Value.Item2 == item.Y &&(item is Wall || item is EnergyBall))
                {
                    return Direction.Stop;
                }
            }
            return currentDirection;
        }

        private Direction FrameTickBall(Direction dir, Core.NewModels.Ball ball)
        {
            var selectedDirection = directions.Where(d => d.Key == dir).FirstOrDefault();
            if(ball.X + 1 == _console2.map.GetLength(0) || ball.Y + 1 == _console2.map.GetLength(1))
            {
                return ChangeDirection(dir);
            }
            foreach (var item in _console2.map)
            {
                if (ball.X + selectedDirection.Value.Item1 == item.X && ball.Y + selectedDirection.Value.Item2 == item.Y && (item is EnergyBall))
                {
                    return dir;
                }

                if (ball.X + selectedDirection.Value.Item1 == item.X && ball.Y + selectedDirection.Value.Item2 == item.Y && (item is Wall))
                {
                    return ChangeDirection(dir);
                }
            }
            return dir;
        }

        private Direction ChangeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Right:
                    return Direction.Left;

                case Direction.Left:
                    return Direction.Right;

                case Direction.Up:
                    return Direction.Down;

                case Direction.Down:
                    return Direction.Up;

            }
            return dir;
        }
    }

}
