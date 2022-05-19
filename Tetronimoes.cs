using System.Collections.Generic;

namespace tetris
{
    public class Tetronimoes
    {
        public int height;
        public int width;
        public Tetronimoes(string piece)
        {
            switch (piece)
            {
                case "O":
                    height = 2;
                    width = 2;
                    break;
                case "I":
                    height = 4;
                    width = 1;
                    break;
                case "S":
                    height = 2;
                    width = 3;
                    break;
                case "Z":
                    height = 2;
                    width = 3;
                    break;
                case "L":
                    height = 3;
                    width = 2;
                    break;
                case "J":
                    height = 3;
                    width = 2;
                    break;
                case "T":
                    height = 2;
                    width = 3;
                    break;
            }
        }

        private List<string> Piece_O()
        {
            string[] hitbox = { "+", "+", "+", "+" };
            return new List<string>(hitbox);
        }

        private List<string> Piece_I()
        {
            string[] hitbox = { "+", "+", "+", "+" };
            return new List<string>(hitbox);
        }

        private List<string> Piece_S()
        {
            string[] hitbox = { "+", "+", "O", "O", "+", "+" };
            return new List<string>(hitbox);
        }

        private List<string> Piece_Z()
        {
            string[] hitbox = { "O", "+", "+", "+", "+", "O" };
            return new List<string>(hitbox);
        }

        private List<string> Piece_L()
        {
            string[] hitbox = { "+", "+", "+", "O", "+", "O" };
            return new List<string>(hitbox);
        }

        private List<string> Piece_J()
        {
            string[] hitbox = { "+", "+", "O", "+", "O", "+" };
            return new List<string>(hitbox);
        }

        private List<string> Piece_T()
        {
            string[] hitbox = { "O", "+", "O", "+", "+", "+" };
            return new List<string>(hitbox);
        }

    }
}