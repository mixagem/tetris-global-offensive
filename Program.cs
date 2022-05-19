using System;
using System.Collections.Generic;

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
            mytetrisgame.activePieceColor = "Red";        // Peça atual
            mytetrisgame.placedPieceColor = "Yellow";       // Peças fixas
            mytetrisgame.borderColor = "Cyan";               // Border do Jogo
            mytetrisgame.gameOverSpriteColor1 = "Magenta";      // Cor principal gameover screen 
            mytetrisgame.gameOverSpriteColor2 = "White";   // Cor secundária gameover screen
            mytetrisgame.nextPieceColors =                  // Cor do preview das peças seguintes
            new List<string> { "DarkGreen", "Cyan", "Magenta", "DarkRed", "DarkYellow", "White", "Blue" };
            //                    I            O        S          Z           L           J        T

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
