using System;
using System.Linq;
using System.Collections.Generic;

namespace tetris
{
    public class GameGrid
    {
        // tamanho da gamegrid
        private int gridSize = 200;
        private int gridRows = 20;
        private int gridCols = 10;

        // cores da grid
        private string activePieceColor = "Green";
        private string placedPieceColor = "Yellow";
        private string borderColor = "Red";
        private string scoreTitleColor = "Cyan";
        private string scoreValueColor = "Green";
        private string messageTextColor = "DarkRed";
        private string nextPieceTitleColor = "DarkMagenta";
        private string nextPieceTextColor = "Blue";

        // lista com os valores de todos os campos do jogo
        private List<string> gameGridValues = new List<string>();

        // o jogo começa com -10pts, porque a primeira peça é "free"
        private int score = -10;
        // rotação da peça
        private int pieceRotation = 0;
        // validador para saber se é o primeiro spawn ou não
        private bool pieceFirstCycle = true;
        // lista default peças
        private List<string> tetronimoesList = new List<string> { "I", "O", "S", "Z", "L", "J", "T" };
        // // lista de peças disponiveis para spawn
        private List<string> activeTetronimoesList = new List<string> { "I", "O", "S", "Z", "L", "J", "T" };
        // peça ativa, necessário saber para flips e whatevers
        private string activePiece;
        // necessários para mostrar no preview
        private string nextPiece;
        // lista de posições ativas (onde a peça está para ficar + onde está)
        private List<int> activePiecePos = new List<int>();

        // string da linha completa 
        private string gameGridRowInString;

        // booleans
        private bool gameOver = false;
        private bool canMove = true;
        private bool canFlip = true;

        // mensagem do combo
        private string propz;


        public GameGrid()
        {
            System.Console.WriteLine("Tetris zoeira. Carrega no enter para começar");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                gameStart();
            }
            else
            {
                System.Console.WriteLine("Volta outra vez quando estiveres pronto.");
            }
        }

        private void gameStart()
        {
            // reseta a grid
            for (int i = 0; i < gridSize; i++)
            {
                gameGridValues.Add("O");
            }
            // faz spawn da primeira peça
            spawnPiece();
            // listener para o jogador
            playerKeyListener(Console.ReadKey().Key);
        }

        // metodo para fazer spawn a uma nova peça
        private void spawnPiece()
        {
            // por cada nova peça em jogo, 10pts
            score += 10;
            // reseta a rotação da peça
            pieceRotation = 0;
            // limpa as nossas peças ativas
            clearActivePiecePos();
            if (pieceFirstCycle)
            {
                // rng para as peças
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
            // faz loop para todos os items das linhas da grid, conforme altura da peça
            for (int i = 0; i < (newPiece.height * gridCols) - 1; i++)
            {
                // gamechecks para as várias peças
                switch (activePiece)
                {
                    // faz spawn na pos 3
                    case "I":
                        if (i % gridCols == 3)
                        {
                            // verifica se a peça pode fazer spawn
                            spawnGameCheck(i);
                        }
                        break;
                    // faz spawn nas pos 3 e 4
                    case "O":
                        if (i % gridCols == 3 || i % gridCols == 4)
                        {
                            spawnGameCheck(i);
                        }
                        break;
                    /* faz spawn nas pos 3 e 4 na linha 1
                    pos 2 e 3 na linha 2 */
                    case "S":
                        if (
                            (
                                (i / gridCols < 1)
                                &&
                                (i % gridCols == 3 || i % gridCols == 4)
                            )
                            ||
                            (
                                (i / gridCols >= 1)
                                &&
                                (i % gridCols == 2 || i % gridCols == 3)
                            )
                        )
                        {
                            spawnGameCheck(i);
                        }
                        break;
                    /* faz spawn nas pos 3 e 4 na linha 1
                    pos 4 e 5 na linha 2 */
                    case "Z":
                        if (
                            (
                                (i / gridCols < 1)
                                &&
                                (i % gridCols == 3 || i % gridCols == 4)
                            )
                            ||
                            (
                                (i / gridCols >= 1)
                                &&
                                (i % gridCols == 4 || i % gridCols == 5)
                            )
                        )
                        {
                            spawnGameCheck(i);
                        }
                        break;
                    /* faz spawn na pos 3 nas linha 1 e 2 
                    pos 3 e 4 na linha 3 */
                    case "L":
                        if (
                            (
                                (i / gridCols < 2)
                                &&
                                (i % gridCols == 3)
                            )
                            ||
                            (
                                (i / gridCols >= 2)
                                &&
                                (i % gridCols == 3 || i % gridCols == 4)
                            )
                        )
                        {
                            spawnGameCheck(i);
                        }
                        break;
                    /* faz spawn na pos 4 nas linha 1 e 2 
                    pos 3 e 4 na linha 3 */
                    case "J":
                        if (
                            (
                                (i / gridCols < 2)
                                &&
                                (i % gridCols == 4)
                            )
                            ||
                            (
                                (i / gridCols >= 2)
                                &&
                                (i % gridCols == 3 || i % gridCols == 4)
                            )
                        )
                        {
                            spawnGameCheck(i);
                        }
                        break;
                    /* faz spawn na pos 3, 4, e 5 na linha 1 
                    pos 4 na linha 2 */
                    case "T":
                        if (
                            (
                                (i / gridCols < 1)
                                &&
                                (i % gridCols == 3 || i % gridCols == 4 || i % gridCols == 5)
                            )
                            ||
                            (
                                (i / gridCols >= 1)
                                &&
                                (i % gridCols == 4)
                            )
                        )
                        {
                            spawnGameCheck(i);
                        }
                        break;
                }
            }
            // se depois das validações houver gameover, podemos bazar do programa
            if (gameOver)
            {
                System.Console.WriteLine("jafoste. rip. press space to bazar");
            }
            else
            {
                // verificar se houve tetris
                checkForTetris();
                updateGameGridWithNewPiece();
                refreshGameGrid();
                playerKeyListener(Console.ReadKey().Key);
            }
        }

        private void clearActivePiecePos()
        {
            // limpa nossa peça ativa
            activePiecePos = new List<int>();
        }

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
            // remove a peça da lista
            activeTetronimoesList.RemoveAt(generatedIndex);
            return selectedPiece;
        }

        private void spawnGameCheck(int i)
        {
            // se o sitio onde a peça vai fazer spawn estiver ocupado
            if (gameGridValues[i].Contains("X"))
            {
                // gameover baby
                gameOver = true;
            }
            else
            {
                // guarda a pos onde a peça vai ficar guardada na lista de peças ativas
                activePiecePos.Add(i);
            }
        }

        private void checkForTetris()
        {
            // contador com o combo do tetris (numero de linhas limpas)
            int combo = 0;
            // contador para verificar se existe linha (precisa ter 8)
            int tetrisLine = 0;
            // loop decrescente com o numero de pos da grid
            for (int i = (gridSize - 1); i >= 0; i--)
            {
                /* se estivermos posicionados na ultima pos da linha
                 resetar o tetrisLine, visto que estamos a decresacer */
                if ((i + 1) % gridCols == 0)
                {
                    tetrisLine = 0;
                }
                /* se a pos tiver "X", acrescentar o tetrisLine */
                if (gameGridValues[i].Contains("X"))
                {
                    tetrisLine++;
                }
                // Tetris baby
                if (tetrisLine == gridCols)
                {
                    // limpa a ultima linha
                    for (int i2 = (i + gridCols - 1); i2 >= i; i2--)
                    {
                        gameGridValues[i2] = "O";
                    }
                    // puxa as linhas existentes para baixo 
                    for (int i3 = (i - 1); i3 >= gridCols; i3--)
                    {
                        gameGridValues[i3 + gridCols] = gameGridValues[i3];
                        gameGridValues[i3] = gameGridValues[i3 - gridCols];
                    }
                    // puxar a primeira linha (fora do range do loop anterior)
                    for (int i4 = 0; i4 < gridCols; i4++)
                    {
                        gameGridValues[i4] = "O";
                    }
                    // acrescenta o contador do tetris
                    combo++;
                    // resetar o loop, de modo a verificar se existe mais tetris
                    i = gridSize;
                }
            }
            // depois de atualizada a grelha e obter o numero de combos
            switch (combo)
            {
                case 1:
                    score += 100;
                    propz = "Very nice";
                    break;
                case 2:
                    score += 250;
                    propz = "Oh la la";
                    break;
                case 3:
                    score += 450;
                    propz = "Sabes encaixar tu";
                    break;
                case 4:
                    score += 1000;
                    propz = "Oh my god stop blowing my mind";
                    break;
                /* invocado quando posicionamos uma peça, 
                para esconder a últiam mensagem exibida */
                default:
                    propz = "";
                    break;
            }
        }

        private void playerKeyListener(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Spacebar:
                    if (!gameOver)
                    {
                        scrollPiece();
                    }
                    break;
                case ConsoleKey.DownArrow:
                    moveDown();
                    break;
                case ConsoleKey.LeftArrow:
                    moveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    moveRight();
                    break;
                case ConsoleKey.F:
                    flipPiece();
                    break;
            }
        }

        private void scrollPiece()
        {
            // lista com as pos ao scrollar a peça ativas 
            List<int> newActivePiecePos = new List<int>();
            // remove os "X" da peça ativa (para não dar overlaps )
            clearActivePiecesFromGrid();
            // para cada pos da peça ativa
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                {
                    // adicionar gridCols (uma linha), e adicionar a nova pos à  nova lista
                    newActivePiecePos.Add(activePiecePos[i] + gridCols);
                    // fazer movecheck á nova posição
                    moveCheck(activePiecePos[i] + gridCols);
                }
            }
            // caso falhou o movecheck
            if (!canMove)
            {
                // atualiza a grid com o novo sítio da peça (utiliza o X, visto nao poder descer mais)
                restoreActivePiecesFromGrid();
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
                // atualiza a grid com o novo sítio da peça (utiliza o +)
                updateGameGridWithNewPiece();
                // reseta a função até não conseguir descer mais
                scrollPiece();
            }
        }

        private void clearActivePiecesFromGrid()
        {
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                gameGridValues[activePiecePos[i]] = "O";
            }
        }

        private void restoreActivePiecesFromGrid()
        {
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                // como não pode mexer mais, a peça fica maracad com "X"
                gameGridValues[activePiecePos[i]] = "X";
            }
        }

        private void updateGameGridWithNewPiece()
        {
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                // como pode mexer  a peça fica maracad com "+"
                gameGridValues[activePiecePos[i]] = "+";
            }
        }

        private void moveDown()
        {
            // lista temp para as novas pos
            List<int> newActivePiecePos = new List<int>();
            // remove os "+" da peça ativa
            clearActivePiecesFromGrid();
            // para cada pos da peça ativa
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                {
                    // adicionar gridCols (uma linha), e adicionar a nova pos a uma nova lista
                    newActivePiecePos.Add(activePiecePos[i] + gridCols);
                    // fazer movecheck á nova posição
                    moveCheck(activePiecePos[i] + gridCols);
                }
            }
            // caso falhe o movecheck
            if (!canMove)
            {
                // atualiza a grid com o último sítio da peça (utiliza o X, visto nao poder descer mais)
                restoreActivePiecesFromGrid();
                // resetar o canMove
                canMove = true;
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
            // caso passe o movecheck (ainda consiga descer linhas)
            else
            {
                // atualiza a lista de pos de peças ativas, com as novas posições
                activePiecePos = new List<int>(newActivePiecePos);
                // atualiza a grid com o novo sítio da peça (utiliza o +)
                updateGameGridWithNewPiece();
                // atualiza a gamegrid
                refreshGameGrid();
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
        }

        private void moveLeft()
        {
            // lista temp para as novas pos
            List<int> newActivePiecePos = new List<int>();
            // remove os "+" da peça ativa
            clearActivePiecesFromGrid();
            // para cada pos da peça ativa
            for (int i = 0; i < activePiecePos.Count; i++)
            {
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
            }
            // caso falhe o movecheck
            if (!canMove)
            {
                // atualiza a grid com o último sítio da peça (utiliza o X, visto nao poder descer mais)
                restoreActivePiecesFromGrid();
                // resetar o canMove
                canMove = true;
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
            else
            {
                // atualiza a lista de pos de peças ativas, com as novas posições
                activePiecePos = new List<int>(newActivePiecePos);
                // atualiza a grid com o novo sítio da peça (utiliza o +)
                updateGameGridWithNewPiece();
                // atualiza a gamegrid
                refreshGameGrid();
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
        }

        private void moveRight()
        {
            /// lista temp para as novas pos
            List<int> newActivePiecePos = new List<int>();
            // remove os "+" da peça ativa
            clearActivePiecesFromGrid();
            // para cada pos da peça ativa
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
            if (!canMove)
            {
                // atualiza a grid com o último sítio da peça (utiliza o X, visto nao poder descer mais)
                restoreActivePiecesFromGrid();
                // resetar o canMove
                canMove = true;
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
            else
            {
                // atualiza a lista de pos de peças ativas, com as novas posições
                activePiecePos = new List<int>(newActivePiecePos);
                // atualiza a grid com o último sítio da peça (utiliza o +)
                updateGameGridWithNewPiece();
                // atualiza a gamegrid
                refreshGameGrid();
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
        }

        private void moveCheck(int i)
        {
            // se passar para fora da grid, ou caso a nova posição esteja ocupada
            if (i > (gridSize - 1) || gameGridValues[i].Contains("X"))
            {
                canMove = false;
            }
        }

        //////////////////////  flips section - wip ///////////////////////////

        private void flipPiece()
        {
            // lista temp para as novas pos
            List<int> newActivePiecePos = new List<int>();
            // remove os "+" da peça ativa
            clearActivePiecesFromGrid();
            // para cada pos da peça ativa
            for (int i = 0; i < activePiecePos.Count; i++)
            {
                // verificar a peça
                switch (activePiece)
                {
                    case "I":
                        if (pieceRotation == 0)
                        {
                            // out of range exception para a nova pos
                            if (activePiecePos[i] - i - (i * gridCols) < 0) { canFlip = false; }

                            // overflow exception para a nova pos 
                            else if (activePiecePos[i] % gridCols <= 2) { canFlip = false; }

                            else
                            {
                                // formula para rodar para 90 
                                newActivePiecePos.Add(activePiecePos[i] - i - (i * gridCols));
                                // fazer flipcheck á nova posição
                                flipcheck(activePiecePos[i] - i - (i * gridCols));
                            }
                        }
                        else
                        {
                            // out of range exception para a nova pos
                            if (activePiecePos[i] + i + (i * gridCols) > gridSize) { canFlip = false; }
                            else
                            {
                                // formula para rodar para 90 
                                newActivePiecePos.Add(activePiecePos[i] + i + (i * gridCols));
                                // fazer flipcheck á nova posição
                                flipcheck(activePiecePos[i] + i + (i * gridCols));
                            }
                        }
                        break;
                    case "O":
                        // não tem rotação, a posição é a mesma, não precisa flipcheck
                        newActivePiecePos.Add(activePiecePos[i]);
                        break;
                    case "S":
                        if (pieceRotation == 0)
                        {
                            // out of range exception para a nova pos
                            if (activePiecePos[i] >= (gridSize - (2 * gridCols))) { canFlip = false; }
                            else
                            {
                                // formula para rodar para 90 
                                switch (i)
                                {
                                    // formula para rodar 90 para o bit 1
                                    case 0:
                                        newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                        break;
                                    // formula para rodar 90 para o bit 2
                                    case 1:
                                        newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                        break;
                                    // formula para rodar 90 para o bit 3
                                    case 2:
                                        newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                        break;
                                    // formula para rodar 90 para o bit 4
                                    case 3:
                                        newActivePiecePos.Add(activePiecePos[i]);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            // out of range exception para a nova pos
                            if (activePiecePos[i] % gridCols == 0) { canFlip = false; }
                            else
                            {
                                // formula para rodar para 90 
                                switch (i)
                                {
                                    // formula para rodar 90 para o bit 1
                                    case 0:
                                        newActivePiecePos.Add(activePiecePos[i] - 1 - gridCols);
                                        break;
                                    // formula para rodar 90 para o bit 2
                                    case 1:
                                        newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                        break;
                                    // formula para rodar 90 para o bit 3
                                    case 2:
                                        newActivePiecePos.Add(activePiecePos[i] - 1 + gridCols);
                                        break;
                                    // formula para rodar 90 para o bit 4
                                    case 3:
                                        newActivePiecePos.Add(activePiecePos[i]);
                                        break;
                                }
                            }
                        }
                        break;
                    case "Z":
                        if (pieceRotation == 0)
                        {
                            // out of range exception para a nova pos
                            if (activePiecePos[i] >= (gridSize - gridCols)) { canFlip = false; }
                            else
                            {
                                // formula para rodar para 90 
                                switch (i)
                                {
                                    // formula para rodar 90 para o bit 1
                                    case 0:
                                        newActivePiecePos.Add(activePiecePos[i] + 1);
                                        break;
                                    // formula para rodar 90 para o bit 2
                                    case 1:
                                        newActivePiecePos.Add(activePiecePos[i] + gridCols);
                                        break;
                                    // formula para rodar 90 para o bit 3
                                    case 2:
                                        newActivePiecePos.Add(activePiecePos[i] - 1);
                                        break;
                                    // formula para rodar 90 para o bit 4
                                    case 3:
                                        newActivePiecePos.Add(activePiecePos[i] - 2 + gridCols);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            // out of range exception para a nova pos
                            if ((activePiecePos[i] + 1) % gridCols == 0) { canFlip = false; }
                            else
                            {
                                // formula para rodar para 90 
                                switch (i)
                                {
                                    // formula para rodar 90 para o bit 1
                                    case 0:
                                        newActivePiecePos.Add(activePiecePos[i] - 1);
                                        break;
                                    // formula para rodar 90 para o bit 2
                                    case 1:
                                        newActivePiecePos.Add(activePiecePos[i] - gridCols);
                                        break;
                                    // formula para rodar 90 para o bit 3
                                    case 2:
                                        newActivePiecePos.Add(activePiecePos[i] + 1);
                                        break;
                                    // formula para rodar 90 para o bit 4
                                    case 3:
                                        newActivePiecePos.Add(activePiecePos[i] + 2 - gridCols);
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
                                    if ((activePiecePos[i] + 1) % gridCols == 0) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + 2);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 - gridCols);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] >= (gridSize - gridCols)) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + 2);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] % gridCols == 0) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - 2 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - 1);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] + 1);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] >= (gridSize - gridCols)) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - 1);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - 2 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 + (2 * gridCols));
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
                                    if (activePiecePos[i] % gridCols == 0) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - 1);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 - (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] - 2 - gridCols);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] >= (gridSize - gridCols)) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - 2 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - 1);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + 1);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] - gridCols);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    // out of range exception para a nova pos
                                    if ((activePiecePos[i] + 1) % gridCols == 0) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] + 2);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] >= (gridSize - gridCols)) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + 2);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
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
                                    if (activePiecePos[i] >= (gridSize - gridCols)) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + 2);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i]);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] % gridCols == 0) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] + (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 + gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - 2);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i]);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    // out of range exception para a nova pos
                                    if (activePiecePos[i] < gridCols) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - 2);
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] - 1 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i]);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                {
                                    // out of range exception para a nova pos
                                    if ((activePiecePos[i] + 1) % gridCols == 0) { canFlip = false; }
                                    else
                                    {
                                        // formula para rodar para 90 
                                        switch (i)
                                        {
                                            // formula para rodar 90 para o bit 1
                                            case 0:
                                                newActivePiecePos.Add(activePiecePos[i] - (2 * gridCols));
                                                break;
                                            // formula para rodar 90 para o bit 2
                                            case 1:
                                                newActivePiecePos.Add(activePiecePos[i] + 1 - gridCols);
                                                break;
                                            // formula para rodar 90 para o bit 3
                                            case 2:
                                                newActivePiecePos.Add(activePiecePos[i] + 2);
                                                break;
                                            // formula para rodar 90 para o bit 4
                                            case 3:
                                                newActivePiecePos.Add(activePiecePos[i]);
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            if (!canFlip)
            {
                // atualiza a grid com o último sítio da peça (utiliza o X, visto nao poder descer mais)
                restoreActivePiecesFromGrid();
                // resetar o canFlip
                canFlip = true;
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
            else
            {
                // switch atualizar o valor atual da rotação de acordo com a peça
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
                // atualiza a lista de pos de peças ativas, com as novas posições
                activePiecePos = new List<int>(newActivePiecePos);
                // atualiza a grid com o novo sítio da peça (utiliza o +)
                updateGameGridWithNewPiece();
                // atualiza a gamegrid
                refreshGameGrid();
                // listener para input do player
                playerKeyListener(Console.ReadKey().Key);
            }
        }

        private void flipcheck(int i)
        {
            // se passar para fora da grid, ou caso a nova posição esteja ocupada
            if (gameGridValues[i].Contains("X"))
            {
                canFlip = false;
            }
        }

        private void writeRedMargins()
        {
            // muda a cor da consola para vermelho
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("================================== ");
            // muda a cor da consola para branco
            Console.ForegroundColor = ConsoleColor.White;
        }

        private string colorfullLine(string text)
        {
            // substitui os "X" e os "+" pelas respetivas cores
            string newtext = text.Replace(" X ", "{BC=" + placedPieceColor + "}   {/BC}");
            string newtext2 = newtext.Replace(" + ", "{BC=" + activePieceColor + "}   {/BC}");
            return newtext2;
        }

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
                if (i == 0 || i % gridCols == 0) { gameGridRowInString = "{FC=" + borderColor + "}|{/FC} "; }
                // adiciona o valor da pos, com o evido espaçamento
                gameGridRowInString += " " + gridValue + " ";
                // se for a última posição da linha
                if ((i + 1) % gridCols == 0)
                {
                    // fecha o border
                    gameGridRowInString += " {FC=" + borderColor + "}|{/FC}";
                    // para a primeira linha, adicionamos o score
                    if (i == ((1 * gridCols) - 1)) { gameGridRowInString += "  {FC=" + scoreTitleColor + "} Score: {/FC}{FC=" + scoreValueColor + "}" + score + "{/FC}"; }
                    // para a terceira linha, adicionamos a mensagem
                    if (i == ((3 * gridCols) - 1)) { gameGridRowInString += "  {FC=" + messageTextColor + "}" + propz + "{/FC}"; }
                    // para a quinta linha, adicionamos o score
                    if (i == ((5 * gridCols) - 1)) { gameGridRowInString += "  {FC=" + nextPieceTitleColor + "} Próxima peça: {/FC}{FC=" + nextPieceTextColor + "}" + nextPiece + "{/FC}"; }
                    // escreve a linha completa
                    ConsoleWriter.WriteLine(colorfullLine(gameGridRowInString));
                }
                // acrescenta o contador
                i++;
            }
            // introduzir a margem superior
            writeRedMargins();
        }
        /////////////////// class end ////////////////////////
    }
}