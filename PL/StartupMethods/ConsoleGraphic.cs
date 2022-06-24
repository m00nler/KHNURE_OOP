﻿using Core.NewModels;
using System;
namespace PL.StartupMethods
{
    public class ConsoleGraphic
    {
        public void Draw(object? sender, EventArgs e)
        {
            if (sender is not BaseElement pixel)
            {
                return;
            }

            var symbol = pixel switch
            {
                Empty => " ",
                Player => ((Player)pixel).reverseSlash ? "\\" : "/",
                EnergyBall => "%",
                Wall => "#",
                Ball => "█",
                _ => "?"
            };

            var color = pixel switch
            {
                Player => ConsoleColor.Red,
                Wall => ConsoleColor.White,
                EnergyBall => ConsoleColor.Green,
                Ball => ConsoleColor.Blue,
                _ => ConsoleColor.Black
            };
            Console.ForegroundColor = color;
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Console.SetCursorPosition(pixel.X * 2 + x, pixel.Y * 2 + y);
                    Console.Write(symbol);
                }
            }
        }
    }
}
