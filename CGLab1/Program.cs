using CGLab1.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGLab1
{
    class Program
    {
        static void Main(string[] args)
        {
            using(Game game = new Game(800,600,"Lab 1"))
            {
                game.Run(60.0);
            }
        }
    }
}
