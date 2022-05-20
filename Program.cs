using System;

namespace tetris
{
    class Program
    {
        static void Main()
        {
            var mytetrisgame = new Game();

            // configuração da grelha de jogo
            mytetrisgame.gridCols = 10;                 // número de colunas
            mytetrisgame.gridSize =                     // tamanho da grelha 
            20 * mytetrisgame.gridCols;                 // (nº de linhas x nº de colunas)
           
            // desenho* da peça atual
            mytetrisgame.fillType = ".";                // desenho* da grelha quando o espaço está vazio 
                                                        // *não podem ser utilizados espaços, nem os símbolos "X", "x", ou "+";            
            // cores da app
            mytetrisgame.activePieceColor = "Yellow";        // Peça atual
            mytetrisgame.placedPieceColor = "Red";       // Peças fixas
            mytetrisgame.borderColor = "Cyan";               // Border do Jogo

            // configurações do jogo
            mytetrisgame.scoreMultiplier = 25;          // valor base do jogo.

            // listener para o jogo começar
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                mytetrisgame.gameStart();
            }
            else
            {
                System.Console.WriteLine("Volta outra vez quando estiveres pronto.");
            }

        }
    }
}
