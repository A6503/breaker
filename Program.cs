﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

namespace ConsoleApp1
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("HelloWorld");
            GameHandler bb = new GameHandler();
            bb.Game();
            Console.WriteLine("Exiting... (any key to exit)");
            Console.ReadKey();
            return;

        }
    }
}
