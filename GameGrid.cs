using System;
using System.Collections.Generic;
using System.Timers;

namespace tetris
{
    public class Game
    {
        /////////////////////////////////////////

        // número de colunas
        public int gridCols = 10;

        // tamanho da gamegrid
        public int gridSize = 200;

        // cores da app
        public string activePieceColor = "Green";
        public string placedPieceColor = "Yellow";
        public string borderColor = "Red";
        private string loadingSpriteColor1 = "Red";
        private string loadingSpriteColor2 = "Yellow";
        private string loadingSpriteColor3 = "Grey";
        private string scoreTitleColor = "Yellow";
        private string scoreColor = "Cyan";
        private string hypeMessageColor = "Magenta";
        private string nextPieceTitleColor = "Yellow";
        private string gameOverSpriteColor1 = "Red";
        private string gameOverSpriteColor2 = "Yellow";
        private string gameOverScoreColor1 = "Cyan";
        private string gameOverScoreColor2 = "White";
        private string gameOverActionColor1 = "Yellow";
        private string gameOverActionColor2 = "Magenta";
        // timmer do autoscroll
        System.Timers.Timer autoscrollerTimer;
        // public game settings 
        public int scoreMultiplier = 25;
        public string fillType = ".";
        public List<string> nextPieceColors = new List<string> { "DarkGreen", "Cyan", "Magenta", "DarkRed", "DarkYellow", "White", "Blue" };

        // ASCII Art Sprites
        public List<string> scoreSprite = new List<string> {
            " __   __   __   __   ___",
            "/__` /  ` /  \\ |__) |__ ",
            ".__/ \\__, \\__/ |  \\ |___"
        };
        private List<string> nextPieceTitleSprite = new List<string> {
            "      ___     ___     __     ___  __   ___",
         "|\\ | |__  \\_/  |     |__) | |__  /  ` |__",
         "| \\| |___ / \\  |     |    | |___ \\__, |___"
         };
        private List<string> gameOverSrite = new List<string> {
            "",
            "",
            "",
            " ██████╗ █████╗███╗   ██████████╗",
            "██╔════╝██╔══██████╗ ██████╔════╝",
            "██║  ████████████╔████╔███████╗",
            "██║   ████╔══████║╚██╔╝████╔══╝",
            "╚██████╔██║  ████║ ╚═╝ █████████╗",
            " ╚═════╝╚═╝  ╚═╚═╝     ╚═╚══════╝",
            "                   ██████╗██╗   ███████████████╗",
            "                  ██╔═══████║   ████╔════██╔══██╗",
            "                  ██║   ████║   ███████╗ ██████╔╝",
            "                  ██║   ██╚██╗ ██╔██╔══╝ ██╔══██╗",
            "                  ╚██████╔╝╚████╔╝█████████║  ██║",
            "                   ╚═════╝  ╚═══╝ ╚══════╚═╝  ╚═╝",
            "",
            ""
        };
        private List<string> welcomeTitleSprite1 = new List<string> {
            "           ████████ ███████ ████████ ██████  ██ ███████",
            "              ██    ██         ██    ██   ██ ██ ██",
            "              ██    █████      ██    ██████  ██ ███████",
            "              ██    ██         ██    ██   ██ ██      ██",
            "              ██    ███████    ██    ██   ██ ██ ███████"
            };
        private List<string> welcomeTitleSprite2 = new List<string> {
            "        __     __        __                         __  _",
            "  ___ _/ /__  / /  ___ _/ / ___  ___  ___ _______ _/ /_(_)__  ___",
            " / _ `/ / _ \\/ _ \\/ _ `/ / / _ \\/ _ \\/ -_) __/ _ `/ __/ / _ \\/ _ \\",
            " \\_, /_/\\___/_.__/\\_,_/_/  \\___/ .__/\\__/_/  \\_,_/\\__/_/\\___/_//_/",
            "/___/                         /_/",
            ""
            };

        // Listas
        // Lista default a ser manupilada aquando do desenho da peça seguinte (tem de ser inicializada com 4 itens)
        private List<string> nextPieceSketch = new List<string> { "", "", "", "" };
        // lista com os valores de todos os campos do jogo
        private List<string> gameGridValues = new List<string>();
        // lista de posições da peça ativa (posições na grid da peça atual / onde a peça vai ficar (nascer, mover ou flipar) caso passe o gamecheck)
        private List<int> activePiecePos = new List<int>();
        // lista default peças (lista fixa com todas as opções, utilizada como referência)
        // private List<string> tetronimoesList = new List<string> { "I" };
        private List<string> tetronimoesList = new List<string> { "I", "O", "S", "Z", "L", "J", "T" }; // deixei aqui no topo para ser mais fácil fazer debug quando quero fazer spawn de uma certa peça
        // lista de peças disponiveis para spawn (a ser manipulada a medida que o jogo corre)
        private List<string> activeTetronimoesList = new List<string>();

        // booleans
        // boolean para validar se é o primeiro cyclo ou não (para saber se gera a activepiece, ou apenas a nextpiece)
        private bool pieceFirstCycle = true;
        private bool gameOver = false;
        private bool canMove = true;
        private bool autoScrollCanMove = true;
        // mensagem do combo
        private string propz;

        // private game settings 
        // o jogo começa com -1*(scoremultiplier), porque a primeira peça é "free"
        private int score = -25;
        // contador de rotações da peça
        private int pieceRotation = 0;
        // nome da peça ativa
        private string activePiece;
        // nome da peça seguinte
        private string nextPiece;
        // string da uma linha completa da grid
        private string gameGridRowInString;
        private string busyType = "X";
        private string activeType = "O";

        //////////////////////////////////////////

        // limpa a consola e mostra o title screen
        public Game()
        {
            mainscreenLogo();
        }
        // limpa a consola e mostra o title screen
        private void mainscreenLogo()
        {
            Console.Clear();
            foreach (string line in welcomeTitleSprite1)
            {
                string beautyLine = line.Replace("█", "█");
                Console.WriteLine(beautyLine);
            }
            foreach (string line in welcomeTitleSprite2)
            {
                Console.WriteLine(line + "");
            }
            Console.WriteLine("                            Press Enter");
        }

        // inicia o jogo
        public void gameStart()
        {
            // loop do tamanho da grid
            for (int i = 0; i < gridSize; i++)
            {
                // caso venhamos de um gameover, a grid já tem o tamanho definido, pelo que devemos dar skin
                if (gameGridValues.Count != gridSize)
                {
                    gameGridValues.Add(fillType);
                }
            }

            // faz spawn da primeira peça
            spawnPiece();

            // iniciar timer do autoscroller
            autoscrollerTimer = new System.Timers.Timer();
            autoscrollerTimer.Elapsed += new ElapsedEventHandler(autoscroller);
            autoscrollerTimer.Interval = 1000;
            autoscrollerTimer.Enabled = true;
            
            while (!gameOver)
            {
                // listener para o jogador
                playerKeyListener(Console.ReadKey().Key);
            }
        }

        // metodo para fazer spawn a uma nova peça
        private void spawnPiece()
        {
            // se for a primeira peça do jogo
            if (pieceFirstCycle)
            {
                // rng para as peças ativa, e seguinte
                activePiece = rngPiece();
                nextPiece = rngPiece();
                // inverter o bool para falso
                pieceFirstCycle = (!pieceFirstCycle);
            }
            else
            {
                // vai buscar a peça gerada anteriormente 
                activePiece = nextPiece;
                // gera uma nova peça seguinte
                nextPiece = rngPiece();
            }
            // construtor da peça
            Tetronimoes newPiece = new Tetronimoes(activePiece);

            // reseta a rotação da peça
            pieceRotation = 0;

            // limpa a lista de pos da nossas peças ativa
            clearActivePiecePos();

            // loop atualizando verificando os valores grelha de jogo, para o numero de linhas igual ao da altura (para ver se tem espaço para fazer spawn)
            for (int i = 0; i < (newPiece.height * gridCols) - 1; i++)
            {
                // spawnGameCheck ás posições da peça. Entenda-se bit* como um pedacinho do tetronimo
                switch (activePiece)
                {
                    case "I":
                        if (i % gridCols == 4)                              // faz spawn de um bit na 5ª coluna, para as 4 linhas (altura peça)
                        {
                            spawnGameCheck(i);                              // verifica se a pos onde o bit vai fazer spawn, está ocupada ou não
                        }
                        break;
                    case "O":
                        // bit na 5ª e 6ª colunas, para 2 linhas
                        if (i % gridCols == 4 || i % gridCols == 5)
                        { spawnGameCheck(i); }
                        break;
                    case "S":
                        if (
                            (
                                // bit na 5ª e 6ª colunas, para a 1ª linha
                                (i / gridCols < 1) && (i % gridCols == 4 || i % gridCols == 5)
                            )
                            ||
                            (   // bit na 4ª e 5ª colunas, para a 2ª linha
                                (i / gridCols >= 1) && (i % gridCols == 3 || i % gridCols == 4)
                            )
                        ) { spawnGameCheck(i); }
                        break;
                    case "Z":
                        if (
                            (   // bit na 4ª e 5ª colunas, para a 1ª linha
                                (i / gridCols < 1) && (i % gridCols == 3 || i % gridCols == 4)
                            )
                            ||
                            (
                                // bit na 5ª e 6ª colunas, para a 2ª linha
                                (i / gridCols >= 1) && (i % gridCols == 4 || i % gridCols == 5)
                            )
                        ) { spawnGameCheck(i); }
                        break;
                    case "L":
                        if (
                            (   // bit na 5ª coluna, para a 1ª e 2ª linhas
                                (i / gridCols < 2) && (i % gridCols == 4)
                            )
                            ||
                            (   // bit na 5ª e 6ª colunas, para a 3ª linha
                                (i / gridCols >= 2) && (i % gridCols == 4 || i % gridCols == 5)
                            )
                        ) { spawnGameCheck(i); }
                        break;
                    case "J":
                        if (
                            (   // bit na 6ª coluna, para a 1ª e 2ª linhas
                                (i / gridCols < 2) && (i % gridCols == 5)
                            )
                            ||
                            (   // bit na 5ª e 6ª colunas, para a 3ª linha
                                (i / gridCols >= 2) && (i % gridCols == 4 || i % gridCols == 5)
                            )
                        ) { spawnGameCheck(i); }
                        break;
                    case "T":
                        if (
                            (   // bit na 4ª, 5ª, e 6ª coluna, para a 1ª linha
                                (i / gridCols < 1) && (i % gridCols >= 3 && i % gridCols <= 5)
                            )
                            ||
                            (   // bit na 5ª coluna, para a 2ª linha
                                (i / gridCols >= 1) && (i % gridCols == 4)
                            )
                        ) { spawnGameCheck(i); }
                        break;
                }
            }

            // por cada nova peça em jogo, atualiza o score, mesmo antes do gameover
            score += scoreMultiplier;

            // limpa a consola, mostra o score, e pergunta o utilizador o que fazer
            if (gameOver)
            {
                autoscrollerTimer.Stop();
                Console.Clear();
                for (int i = 0; i < gameOverSrite.Count; i++)
                {
                    string beautyLine = "";
                    for (int i2 = 0; i2 < gameOverSrite[i].Length; i2++)
                    {
                        if (gameOverSrite[i][i2] == ' ')
                        {
                            beautyLine += " ";
                        }
                        else if (gameOverSrite[i][i2] == '█')
                        {
                            beautyLine += "█";
                        }
                        else
                        {
                            beautyLine += gameOverSrite[i][i2];
                        }
                    }
                    Console.WriteLine(beautyLine);
                };
                Console.WriteLine("                        Score: " + score);
                Console.WriteLine("");
                Console.WriteLine("           Carrega R para Recomeçar ou Q para Sair.");
                Console.WriteLine("");
                // listener do gameover screen
                finalScreenListener(Console.ReadKey().Key);
            }
            // o jogo continua
            else
            {
                // verificar se fez tetris
                checkForTetris();
                /* (ao fixarmos uma peça, é sempre feito o spawn de uma nova peça,
                por isso basta fazer esta validação aqui, que apanha as linhas de tetris
                da peça posicionada, e da peça que acabou de fazer spawn) */

                // atualiza a os valores da grelha de jogo, com a peça que fez spawn
                restoreLastActivePiecePos("active");

                // atualiza o preview com a próxima peça
                updateNextPiecePreview(nextPiece);

                // desenha a grelha de jogo atualizada 
                refreshGameGrid();

                // listener para input do jogador
                //playerKeyListener(Console.ReadKey().Key);
            }
        }

        // atualiza a consola com a grelha de jogo atualizada
        private void refreshGameGrid()
        {
            Console.Clear();
            // introduzir a margem superior
            writeRedMargins();

            int i = 0;
            // para cada valor da grelha
            foreach (var gridValue in gameGridValues)
            {
                // se for a primeira posição da linha, reseta o gameGridRowInString com o border
                if (i == 0 || i % gridCols == 0) { gameGridRowInString = "█"; }
                // adiciona o valor da pos, com o evido espaçamento
                gameGridRowInString += " " + gridValue + " ";
                // se for a última posição da linha
                if ((i + 1) % gridCols == 0)
                {
                    // fecha o border
                    gameGridRowInString += " █";
                    // formula para obter o número da linha onde o loop está
                    var numLinha = ((i + 1) / gridCols);
                    // adicionar o título score  [preciso 3 linhas]
                    if (numLinha > 0 && numLinha < 4)
                    {
                        gameGridRowInString += "              " + scoreSprite[numLinha - 1] + "";
                    };
                    // mostrar o score
                    if (numLinha == 5)
                    {
                        gameGridRowInString += "                        " + score;
                    }
                    // mostrar a mensagem
                    if (numLinha == 8)
                    {
                        gameGridRowInString += "            " + propz;
                    }
                    //  título da próxima peça  
                    if (numLinha > 9 && numLinha < 13)
                    {
                        gameGridRowInString += "     " + nextPieceTitleSprite[numLinha - 10];
                    };
                    // mostrar a próxima peça
                    if (numLinha > 14 && numLinha < 19)
                    {
                        gameGridRowInString += "                    " + nextPieceSketch[numLinha - 15];
                    }
                    // escreve a linha completa
                    Console.WriteLine(gameGridColorfulDisplay(gameGridRowInString));
                }
                // incrementar o contador
                i++;
            }
            // introduzir a margem inferior
            writeRedMargins();
        }

        // escreve as margens da grelha de jogo
        private void writeRedMargins()
        {
            Console.WriteLine("==================================");
        }

        // remove os chars das peças ativas e fixas, e pinta o background com a cor selecionada  
        private string gameGridColorfulDisplay(string text)
        {
            // string newtext = text;
            return text;
        }

        // limpa a lista de pos da peça ativa
        private void clearActivePiecePos()
        { activePiecePos = new List<int>(); }

        // seletor de peça aleatório
        private string rngPiece()
        {
            // se a lista de peças ativas estiver vazia, dá reset 
            if (activeTetronimoesList.Count == 0) { activeTetronimoesList = new List<string>(tetronimoesList); }
            // gera um numero
            Random generated = new Random();
            // entre 1 e o número de tetronimos ativos
            int generatedIndex = generated.Next(0, activeTetronimoesList.Count);
            // vai buscar o nome da peça
            string selectedPiece = activeTetronimoesList[generatedIndex];
            // remove a peça da lista de peças ativas
            activeTetronimoesList.RemoveAt(generatedIndex);
            return selectedPiece;
        }

        // atualiza o preview da próxima peça
        private void updateNextPiecePreview(string nextTetroPiece)
        {
            // construtor da peça, para obter o desenho para preview
            Tetronimoes newPiece = new Tetronimoes(nextTetroPiece);

            // contador de linhas completas
            int linhasCompletas = 0;

            // string utilizada para atualizar a lista com a peça seguinte 
            string newPieceLineString = "";

            // para cada bit da peça
            for (int i = 0; i < newPiece.desenho.Count; i++)
            {
                // atualizar string com os bits
                newPieceLineString += stringConverter(newPiece.desenho[i]);

                // se está na última col do desenho
                if ((i + 1) % 4 == 0)
                {
                    // atualiza a grid do desenho a linha completa
                    nextPieceSketch[linhasCompletas] = beautifyPiece(newPieceLineString, nextTetroPiece);
                    // atualiza o contador de linhas completas
                    linhasCompletas++;
                    // reseta a linha
                    newPieceLineString = "";
                }
            }
        }

        // conversor do desenho das peças de acordo com o selecionado nos parâmetros
        private string stringConverter(string toconvert)
        {
            string convertedString = toconvert.Replace("X", busyType);
            string convertedString2 = convertedString.Replace("O", fillType);
            return convertedString2;
        }

        // transforma a peça do preview, de acordo com as cores selecionadas
        private string beautifyPiece(string pieceLineToBeautify, string piece)
        {
            // utilizada para escolher a cor da proxima peça
            string nextPieceColor = "";

            // update piece color 
            switch (piece)
            {
                case "I":
                    nextPieceColor = nextPieceColors[0];
                    break;
                case "O":
                    nextPieceColor = nextPieceColors[1];
                    break;
                case "Z":
                    nextPieceColor = nextPieceColors[2];
                    break;
                case "S":
                    nextPieceColor = nextPieceColors[3];
                    break;
                case "L":
                    nextPieceColor = nextPieceColors[4];
                    break;
                case "J":
                    nextPieceColor = nextPieceColors[5];
                    break;
                case "T":
                    nextPieceColor = nextPieceColors[6];
                    break;
            }

            // string newtext = pieceLineToBeautify.Replace(busyType, "{BC=" + nextPieceColor + "}   {/BC}");
            // string newtext2 = newtext.Replace(fillType, "   ");
            return pieceLineToBeautify;
        }

        // verifica se a pos onde o bit vai dar spawn está ocupada ou não 
        // de acordo com a verificação, atualiza gameOver ou activePiecePos
        private void spawnGameCheck(int i)
        {
            // verifica se a pos onde o bit vai dar spawn, está ocupado ou não
            if (gameGridValues[i].Contains(busyType))
            {
                // se estiver ocupado, é gameover baby. insert a coin and try again
                gameOver = true;
            }
            else
            {
                // adiciona a pos do bit na lista pos da peça ativa
                activePiecePos.Add(i);
            }
        }

        // verifica se o jogador fez tetris
        private void checkForTetris()
        {
            // contador com o combo do tetris (numero de linhas destruídas)
            int combo = 0;
            // contador para verificar se existe linha 
            int tetrisLine = 0;

            // loop para atualizar os valores da grelha de jogo
            for (int i = (gridSize - 1); i >= 0; i--)
            {
                // se estivermos posicionados na ultima coluna, resetar o tetrisLine
                if ((i + 1) % gridCols == 0) { tetrisLine = 0; }

                // se a pos tiver ocupada, incrementar o tetrisLine 
                if (gameGridValues[i].Contains(busyType)) { tetrisLine++; }

                // se a tetrisLine for do tamanho do número de colunas, temos Tetris baby
                if (tetrisLine == gridCols)
                {
                    // limpa a última linha da grelha de jogo
                    for (int i2 = (i + gridCols - 1); i2 >= i; i2--) { gameGridValues[i2] = fillType; }
                    // puxa as linhas existentes para baixo 
                    for (int i3 = (i - 1); i3 >= gridCols; i3--)
                    {
                        gameGridValues[i3 + gridCols] = gameGridValues[i3];
                        gameGridValues[i3] = gameGridValues[i3 - gridCols];
                    }
                    // criar uma nova primeira linha (fora do range do loop anterior)
                    for (int i4 = 0; i4 < gridCols; i4++) { gameGridValues[i4] = fillType; }
                    // acrescenta o contador do tetris
                    combo++;
                    // resetar o loop, de modo a verificar se existe mais tetris
                    i = gridSize;
                }
            }

            // aumentar resultado de acordo com o combo
            switch (combo)
            {
                case 1:
                    score += (scoreMultiplier * 10);
                    propz = "    bora bora, continua";
                    break;
                case 2:
                    score += (scoreMultiplier * 25);
                    propz = "    isto está a aquecer";
                    break;
                case 3:
                    score += (scoreMultiplier * 45);
                    propz = "vejo que sabes encaixar";
                    break;
                case 4:
                    score += (scoreMultiplier * 70);
                    propz = "   stop blowing my mind";
                    break;

                default:        // invocado quando posicionamos uma peça, resetando  a última mensagem exibida
                    propz = "";
                    break;
            }
        }

        // listeners para inputs do jogador
        private void playerKeyListener(ConsoleKey key)
        {
            if (gameOver) { return; } // se o jogo terminar, fechar todos os listeners
            else
            {
                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        scrollPiece();
                        break;
                    case ConsoleKey.DownArrow:
                        movePiece("down");
                        break;
                    case ConsoleKey.LeftArrow:
                        movePiece("left");
                        break;
                    case ConsoleKey.RightArrow:
                        movePiece("right");
                        break;
                    case ConsoleKey.F:
                        movePiece("flip");
                        break;
                    default:
                        break;
                }
            }

            // listener para input do player
            //playerKeyListener(Console.ReadKey().Key);
        }
        // faz scroll da peça até onde puder
        private void scrollPiece()
        {
            // lista com as pos ao scrollar da peça ativa 
            List<int> newActivePiecePos = new List<int>();
            // remove a peça ativa da greçha
            clearActivePiecesFromGrid();
            // para cada pos da peça ativa
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                // adicionar gridCols  à pos (uma linha / scrolldown) , e adicionar a nova pos à  nova lista
                newActivePiecePos.Add(activePiecePos[i] + gridCols);
                // fazer movecheck á nova posição
                moveCheck(activePiecePos[i] + gridCols);
            }
            // caso falhe o movecheck
            if (!canMove)
            {
                // atualiza a grid com o  sítio onde a peça ativa estava (como "busy" visto que a peça não pode descer mais) 
                restoreLastActivePiecePos("busy");
                // resetar o canMove
                canMove = true;
                // faz spawn à próxima peça, visto que não pode descer mais
                spawnPiece();
            }
            // caso passe o movecheck (ainda consiga descer linhas)
            else
            {
                // atualiza a lista de pos de peças ativas, com as novas posições
                activePiecePos = new List<int>(newActivePiecePos);
                // atualiza a grid com o novo sítio da peça ativa
                restoreLastActivePiecePos("active");
                // reseta a função até não conseguir descer mais
                scrollPiece();
            }
        }

        // limpa a peça ativas da lista de valores da grelha de jogo
        private void clearActivePiecesFromGrid()
        {
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                gameGridValues[activePiecePos[i]] = fillType;
            }
        }

        // volta a escreve a peça ativa na lista de valores da grelha de jogo (onde estava)
        private void restoreLastActivePiecePos(string piecetype)
        {
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                switch (piecetype)
                {
                    case "active":
                        gameGridValues[activePiecePos[i]] = activeType;
                        break;
                    case "busy":
                        gameGridValues[activePiecePos[i]] = busyType;
                        break;
                }
            }
        }

        // método para a movimentar/flippar a peça nas direções suportadas
        private void movePiece(string direction)
        {
            // lista temp para as pos após descer
            List<int> newActivePiecePos = new List<int>();
            // remove a peça ativa da lista de valores da grelha de jogo
            clearActivePiecesFromGrid();

            switch (direction)
            {
                case "down":
                    for (int i = 0; i < activePiecePos.Count; i++)
                    {
                        // adicionar gridCols (uma linha), e adicionar a nova pos a uma nova lista
                        newActivePiecePos.Add(activePiecePos[i] + gridCols);
                        // fazer movecheck á nova posição
                        moveCheck(activePiecePos[i] + gridCols);
                    }
                    break;
                case "left":
                    for (int i = 0; i < activePiecePos.Count; i++)
                    {
                        // se a pos da peça estiver na primeira coluna, não dá para mover para a esquerda
                        if (activePiecePos[i] % gridCols == 0) { canMove = false; }
                        else
                        {
                            // remover 1 unidade da pos, e adicionar a nova pos a uma nova lista
                            newActivePiecePos.Add(activePiecePos[i] - 1);
                            // fazer movecheck á nova posição
                            moveCheck(activePiecePos[i] - 1);
                        }
                    }
                    break;
                case "right":
                    for (int i = 0; i < activePiecePos.Count; i++)
                    {
                        {
                            if ((activePiecePos[i] + 1) % gridCols == 0) { canMove = false; }
                            else
                            {
                                // adicionar 1 unidade da pos, e adicionar a nova pos a uma nova lista
                                newActivePiecePos.Add(activePiecePos[i] + 1);
                                // fazer movecheck á nova posição
                                moveCheck(activePiecePos[i] + 1);
                            }
                        }
                    }
                    break;
                case "flip":
                    for (int i = 0; i < activePiecePos.Count; i++)
                    {
                        // verificar a peça
                        switch (activePiece)
                        {
                            case "I":
                                if (pieceRotation == 0)
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] - i - (i * gridCols) < 0) { canMove = false; }

                                    // overflow exception para a nova pos 
                                    else if (activePiecePos[i] % gridCols <= 2) { canMove = false; }

                                    else
                                    {
                                        // formula para rodar para 90 
                                        newActivePiecePos.Add(activePiecePos[i] - i - (i * gridCols));
                                        // fazer moveCheck á nova posição
                                        moveCheck(activePiecePos[i] - i - (i * gridCols));
                                    }
                                }
                                else
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] + i + (i * gridCols) > gridSize) { canMove = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        newActivePiecePos.Add(activePiecePos[i] + i + (i * gridCols));
                                        // fazer moveCheck á nova posição
                                        moveCheck(activePiecePos[i] + i + (i * gridCols));
                                    }
                                }
                                break;
                            case "O":
                                // não tem rotação, a posição é a mesma, não precisa moveCheck
                                newActivePiecePos.Add(activePiecePos[i]);
                                break;
                            case "S":
                                if (pieceRotation == 0)
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] >= (gridSize - (2 * gridCols))) { canMove = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                // fazer moveCheck á nova posição
                                                moveCheck(activePiecePos[i] + 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                // fazer moveCheck á nova posição
                                                moveCheck(activePiecePos[i] + (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                // fazer moveCheck á nova posição
                                                moveCheck(activePiecePos[i] + 1 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i]);
                                                // fazer moveCheck á nova posição
                                                moveCheck(activePiecePos[i]);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] % gridCols == 0) { canMove = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 - gridCols);
                                                moveCheck(activePiecePos[i] - 1 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                moveCheck(activePiecePos[i] - (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 + gridCols);
                                                moveCheck(activePiecePos[i] - 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i]);
                                                moveCheck(activePiecePos[i]);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "Z":
                                if (pieceRotation == 0)
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] >= (gridSize - gridCols)) { canMove = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + 1);
                                                moveCheck(activePiecePos[i] + 1);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + gridCols);
                                                moveCheck(activePiecePos[i] + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - 1);
                                                moveCheck(activePiecePos[i] - 1);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] - 2 + gridCols);
                                                moveCheck(activePiecePos[i] - 2 + gridCols);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    // out of range exception para a nova pos
                                    if ((activePiecePos[i] + 1) % gridCols == 0) { canMove = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - 1);
                                                moveCheck(activePiecePos[i] - 1);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - gridCols);
                                                moveCheck(activePiecePos[i] - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + 1);
                                                moveCheck(activePiecePos[i] + 1);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] + 2 - gridCols);
                                                moveCheck(activePiecePos[i] + 2 - gridCols);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "L":
                                switch (pieceRotation)
                                {
                                    case 0:
                                        {
                                            // out of range exception para a nova pos
                                            if ((activePiecePos[i] + 1) % gridCols == 0) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] + 2);
                                                        moveCheck(activePiecePos[i] + 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                        moveCheck(activePiecePos[i] + 1 - gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                        moveCheck(activePiecePos[i] - (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1 - gridCols);
                                                        moveCheck(activePiecePos[i] - 1 - gridCols);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] >= (gridSize - gridCols)) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                        moveCheck(activePiecePos[i] + (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                        moveCheck(activePiecePos[i] + 1 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] + 2);
                                                        moveCheck(activePiecePos[i] + 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                        moveCheck(activePiecePos[i] + 1 - gridCols);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] % gridCols == 0) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] - 2 - gridCols);
                                                        moveCheck(activePiecePos[i] - 2 - gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1);
                                                        moveCheck(activePiecePos[i] - 1);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] + gridCols);
                                                        moveCheck(activePiecePos[i] + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1);
                                                        moveCheck(activePiecePos[i] + 1);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] >= (gridSize - gridCols)) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] - gridCols);
                                                        moveCheck(activePiecePos[i] - gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1);
                                                        moveCheck(activePiecePos[i] - 1);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] - 2 + gridCols);
                                                        moveCheck(activePiecePos[i] - 2 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1 + (2 * gridCols));
                                                        moveCheck(activePiecePos[i] - 1 + (2 * gridCols));
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                                break;

                            case "J":
                                switch (pieceRotation)
                                {
                                    case 0:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] % gridCols == 0) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] + gridCols);
                                                        moveCheck(activePiecePos[i] + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1);
                                                        moveCheck(activePiecePos[i] - 1);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1 - (2 * gridCols));
                                                        moveCheck(activePiecePos[i] - 1 - (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] - 2 - gridCols);
                                                        moveCheck(activePiecePos[i] - 2 - gridCols);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] >= (gridSize - gridCols)) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] - 2 + gridCols);
                                                        moveCheck(activePiecePos[i] - 2 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1);
                                                        moveCheck(activePiecePos[i] - 1);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1);
                                                        moveCheck(activePiecePos[i] + 1);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] - gridCols);
                                                        moveCheck(activePiecePos[i] - gridCols);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            // out of range exception para a nova pos
                                            if ((activePiecePos[i] + 1) % gridCols == 0) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                        moveCheck(activePiecePos[i] - (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                        moveCheck(activePiecePos[i] + 1 - gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                        moveCheck(activePiecePos[i] + 1 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] + 2);
                                                        moveCheck(activePiecePos[i] + 2);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] >= (gridSize - gridCols)) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] + 2);
                                                        moveCheck(activePiecePos[i] + 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                        moveCheck(activePiecePos[i] + 1 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1 + gridCols);
                                                        moveCheck(activePiecePos[i] - 1 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                        moveCheck(activePiecePos[i] + (2 * gridCols));
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                                break;
                            case "T":
                                switch (pieceRotation)
                                {
                                    case 0:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] >= (gridSize - gridCols)) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] + 2);
                                                        moveCheck(activePiecePos[i] + 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                        moveCheck(activePiecePos[i] + 1 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                        moveCheck(activePiecePos[i] + (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i]);
                                                        moveCheck(activePiecePos[i]);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] % gridCols == 0) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                        moveCheck(activePiecePos[i] + (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1 + gridCols);
                                                        moveCheck(activePiecePos[i] - 1 + gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] - 2);
                                                        moveCheck(activePiecePos[i] - 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i]);
                                                        moveCheck(activePiecePos[i]);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            // out of range exception para a nova pos
                                            if (activePiecePos[i] < gridCols) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] - 2);
                                                        moveCheck(activePiecePos[i] - 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] - 1 - gridCols);
                                                        moveCheck(activePiecePos[i] - 1 - gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                        moveCheck(activePiecePos[i] - (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i]);
                                                        moveCheck(activePiecePos[i]);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        {
                                            // out of range exception para a nova pos
                                            if ((activePiecePos[i] + 1) % gridCols == 0) { canMove = false; }
                                            else
                                            {
                                                // formula para rodar para 90 
                                                switch (i)
                                                {
                                                    // formula para rodar 90 para o bit 1
                                                    case 0:
                                                        newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                        moveCheck(activePiecePos[i] - (2 * gridCols));
                                                        break;
                                                    // formula para rodar 90 para o bit 2
                                                    case 1:
                                                        newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                        moveCheck(activePiecePos[i] + 1 - gridCols);
                                                        break;
                                                    // formula para rodar 90 para o bit 3
                                                    case 2:
                                                        newActivePiecePos.Add(activePiecePos[i] + 2);
                                                        moveCheck(activePiecePos[i] + 2);
                                                        break;
                                                    // formula para rodar 90 para o bit 4
                                                    case 3:
                                                        newActivePiecePos.Add(activePiecePos[i]);
                                                        moveCheck(activePiecePos[i]);
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    break;
            }
            // caso falhe o movecheck
            if (!canMove)
            {
                // atualiza a lista de valores da grelha de jogo com a última posição ativa válida a peça 
                restoreLastActivePiecePos("active");
                // resetar o canMove
                canMove = true;
                // listener para input do player
                // playerKeyListener(Console.ReadKey().Key);
            }
            // o jogo continua
            else
            {
                // switch atualizar o valor atual da rotação de acordo com a peça
                if (direction == "flip")
                {
                    switch (activePiece)
                    {
                        case "I":
                            // só tem 2 rotações possíveis
                            if (pieceRotation == 0) { pieceRotation++; } else { pieceRotation--; };
                            break;
                        case "S":
                            // só tem 2 rotações possíveis
                            if (pieceRotation == 0) { pieceRotation++; } else { pieceRotation--; };
                            break;
                        case "Z":
                            // só tem 2 rotações possíveis
                            if (pieceRotation == 0) { pieceRotation++; } else { pieceRotation--; };
                            break;
                        case "L":
                            // tem 4 rotações possíveis
                            if (pieceRotation == 3) { pieceRotation = 0; } else { pieceRotation++; };
                            break;
                        case "J":
                            // tem 4 rotações possíveis
                            if (pieceRotation == 3) { pieceRotation = 0; } else { pieceRotation++; };
                            break;
                        case "T":
                            // tem 4 rotações possíveis
                            if (pieceRotation == 3) { pieceRotation = 0; } else { pieceRotation++; };
                            break;
                    }
                }
                // atualiza a lista de pos de peças ativas, com as novas posições
                activePiecePos = new List<int>(newActivePiecePos);
                // atualiza a grid com o novo sítio da peça (utiliza o +)
                restoreLastActivePiecePos("active");
                // atualiza a gamegrid
                refreshGameGrid(); ;
            }
        }

        // verifica se a pos onde o bit vai ficar posicionado, está ocupada ou não
        private void moveCheck(int i)
        {
            // se passar para fora da grid, ou caso a nova posição esteja ocupada
            if (i > (gridSize - 1) || gameGridValues[i].Contains(busyType)) { canMove = false; autoScrollCanMove = false;}
        }

        // listener ecrã gameover
        private void finalScreenListener(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Q:
                    { Environment.Exit(0); }
                    break;
                case ConsoleKey.R:
                    {
                        // reseta os valores da grelha
                        for (int i = 0; i < gameGridValues.Count; i++)
                        {
                            gameGridValues[i] = fillType;
                        }
                        // reseta o primeiro ciclo
                        pieceFirstCycle = true;
                        // reseta a lita de tetronimos disponiveis
                        activeTetronimoesList = new List<string>(tetronimoesList);
                        // reseta o score
                        score = -(scoreMultiplier);
                        // reseta o gameover e o can move
                        canMove = true;
                        autoScrollCanMove = true;
                        gameOver = false;
                        gameStart();
                    }
                    break;
                default:
                    { break; }
            }
        }

        // autoscroller
        private void autoscroller(object source, ElapsedEventArgs e)
        {
            movePiece("down");
            if (!autoScrollCanMove) {
                spawnPiece();
                autoScrollCanMove = true;
            }
        }

        /////////////////// class end ////////////////////////
    }
}