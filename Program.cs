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
            int difficulty = 2;
            bool quit = false;
            while (!quit)
            {
                GameHandler play = new GameHandler(difficulty);
                play.StartGame();
                Console.WriteLine("Play again? (Y/N)");
                ConsoleKeyInfo response = Console.ReadKey();
                switch(response.Key)
                {
                    case ConsoleKey.Y:
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
