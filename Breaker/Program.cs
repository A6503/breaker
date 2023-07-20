using System;
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
            bool quit = false;
            while (!quit)
            {
                Game.Game play = new Game.Game();
                play.PlayGame();
                Console.WriteLine("Play again? (type Y)");
                ConsoleKeyInfo response = Console.ReadKey();
                switch(response.Key)
                {
                    case ConsoleKey.Y:
                        Console.WriteLine();
                        break;
                    default:
                        quit = true;
                        break;
                }
            }

            return;

        }
    }
}
