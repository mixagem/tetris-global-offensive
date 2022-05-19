using System.Collections.Generic;

namespace tetris
{
    public class Tetronimoes
    {
        public int height;
        public int width;
        public List<string> desenho;
        public Tetronimoes(string piece)
        {
            switch (piece)
            {
                case "O":
                    height = 2;
                    width = 2;
                    desenho = new List<string>{"O","O","O","O","O","X","X","O","O","X","X","O","O","O","O","O"};
                    break;
                case "I":
                    height = 4;
                    width = 1;
                    desenho = new List<string>{"O","O","X","O","O","O","X","O","O","O","X","O","O","O","X","O"};
                    break;
                case "S":
                    height = 2;
                    width = 3;
                    desenho = new List<string>{"O","O","O","O","O","O","X","X","O","X","X","O","O","O","O","O"};
                    break;
                case "Z":
                    height = 2;
                    width = 3;
                    desenho = new List<string>{"O","O","O","O","O","X","X","O","O","O","X","X","O","O","O","O"};
                    break;
                case "L":
                    height = 3;
                    width = 2;
                    desenho = new List<string>{"O","O","O","O","O","X","O","O","O","X","O","O","O","X","X","O"};
                    break;
                case "J":
                    height = 3;
                    width = 2;
                    desenho = new List<string>{"O","O","O","O","O","O","X","O","O","O","X","O","O","X","X","O"};
                    break;
                case "T":
                    height = 2;
                    width = 3;
                    desenho = new List<string>{"O","O","O","O","O","X","X","X","O","O","X","O","O","O","O","O"};
                    break;
            }
        }
    }
}