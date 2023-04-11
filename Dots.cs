using DotsGame.Extensions;
using static Constantes;

namespace DotsGame.Core
{
    public class Dots
    {
        private char[,] TabuleiroPrincipal = new char[DimensaoDoTabuleiro, DimensaoDoTabuleiro];
        private No noRaiz = new();

        public void Inicializar()
        {
            ExibirMensagemInicial();

            int numeroDaOpcao;

            do
            {
                ExibirOpcoes();

                string opcaoDigitada = Console.ReadLine() ?? "";
                if (int.TryParse(opcaoDigitada, out numeroDaOpcao)) 
                {
                    switch(numeroDaOpcao)
                    {
                        case 1: ExibirTutorial(); break;
                        case 2: IniciarJogo(); break;
                        case 3: SairDoJogo(); break;
                        default: ExibirMensagemDeOpcaoInvalida(); break;
                    }
                }
                else
                {
                    ExibirMensagemDeOpcaoInvalida();
                }
            }while(numeroDaOpcao != 3);
        }

        private void ExibirMensagemInicial()
        {
            Console.Clear();
            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("===================================");
            Console.WriteLine("Bem vindo ao meu DOTS C#");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nEsse jogo foi feito utilizando uma \narvore de possibilidades e um \nalgoritmo de MinMax.\nO seu objetivo é ganhar da IA*.\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("===================================");
            Console.WriteLine("\nFeito por Adrian Damião");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Pressione enter para iniciar");
            Console.ReadKey();
            Console.Clear();
        }

        private void ExibirOpcoes()
        {
            // Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Selecione uma opção:");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("1 - Tutorial");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("2 - Iniciar Jogo");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("3 - Sair");
            Console.ResetColor();
        }

        private void ExibirTutorial()
        {
            Console.WriteLine("===================================");
            Console.WriteLine("Tutorial");
            Console.WriteLine("\nO jogo Dots é composto por 9 pontos....\n");
            
            var noExemplo = new No();
            noExemplo.ExibirTabuleiroTutorial();
        }

        private void IniciarJogo()
        {
            //Verificar a possibilidade de exibir as jogadas possíveis conforme o andamento do jogo
            ExibirTabuleiroComJogadasPossiveis();

            // Força primeira jogada da IA na posição 1
            Console.WriteLine("IA começa jogando...");
            var coordenada = noRaiz.MapearPosicao(1);
            bool jogador = false;
            JogadasPossiveis[0] = false;
            noRaiz.Tabuleiro[coordenada.linha, coordenada.coluna] = MarcadorDeJogada;
            TabuleiroPrincipal[coordenada.linha, coordenada.coluna] = MarcadorDeJogada;
            

            // Espera o jogador fazer a segunda jogada
            Console.WriteLine("Onde você quer jogar(número):");
            string numero = Console.ReadLine() ?? "";
            
            coordenada = noRaiz.MapearPosicao(int.Parse(numero));
            noRaiz.Tabuleiro[coordenada.linha, coordenada.coluna] = MarcadorDeJogada;
            JogadasPossiveis[int.Parse(numero) - 1] = false;
            
            // Após jogar duas vezes, começa a preencher a árvore
            if(JogadasPossiveis.Count(jogada => jogada == false) >= 2)
            {
                PreencherArvoreDePossibilidades(noRaiz, JogadasPossiveis, jogador);
            }

            // Avalia o min e max
            AvaliarMiniMax(noRaiz, jogador);

            // realiza as jogadas
            var resultado = Jogar(noRaiz, jogador);
            if(resultado == 1 || resultado == -1 || resultado == 0)
                return;
        }

        private void PreencherArvoreDePossibilidades(No noPai, bool[] jogadasPossiveis, bool jogador)
        {
            for(int i = 0; i < jogadasPossiveis.Length; i++)
            {
                // Jogada é possivel?
                if(jogadasPossiveis[i] == true)
                {
                    No filho = new();

                    // Copia o tabuleiro do Pai
                    filho.CopiaMatrizDoPai(noPai);

                    // Realiza Jogada do filho
                    var coordenada = filho.MapearPosicao(i + 1);
                    filho.Tabuleiro[coordenada.linha, coordenada.coluna] = MarcadorDeJogada;

                    jogadasPossiveis[i] = false;
                    noPai.Filhos.Add(filho);

                    if(jogador) 
                        filho.ValorMinMax = Int32.MinValue;
                    else
                        filho.ValorMinMax = Int32.MaxValue;

                    if(filho.VerificarSeFechouQuadrado(coordenada, jogador) == true)
                    {
                        PreencherArvoreDePossibilidades(filho, jogadasPossiveis, jogador);
                    }
                    else
                    {
                        PreencherArvoreDePossibilidades(filho, jogadasPossiveis, !jogador);
                    }
                    jogadasPossiveis[i] = true;
                }
            }
        }

        private int AvaliarMiniMax(No no, bool jogador)
        {
            var ganhador = EncontrarGanhador(no);

            if (ganhador != 2) // Se o jogo foi finalizado
            {
                if (ganhador == 1)
                {
                    return 1;
                }
                else if (ganhador == -1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (jogador)
                {
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliarMiniMax(no.Filhos[i], !jogador);
                        if (resultado < no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
                else if (!jogador)
                {
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliarMiniMax(no.Filhos[i], !jogador);
                        if (resultado > no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
            }
            return 0;
        }

        private int Jogar(No no, bool jogador)
        {
            //Atualiza o tabuleiro e imprime
            ExibirTabuleiro();

            var ganhador = EncontrarGanhador(no);
            if (ganhador != 2)
            {
                System.Console.WriteLine("Ganhador: "+ ganhador);                
            } 
            else
            {
                if (!jogador) //Maquina
                {    
                    int melhorPontuacao = Int32.MinValue;
                    int indiceMelhorFilho = 0;
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        if (no.Filhos[i].ValorMinMax > melhorPontuacao)
                        {
                            melhorPontuacao = no.Filhos[i].ValorMinMax;
                            indiceMelhorFilho = i;
                        }
                    }

                    var coordenadaDaJogada = (0, 0);

                    for(int i = 0; i < 5; i++)
                    {
                        for(int j = 0; j < 5; j++)
                        {
                            if(no.Filhos[indiceMelhorFilho].Tabuleiro[i, j] != TabuleiroPrincipal[i, j]
                                && no.Filhos[indiceMelhorFilho].Tabuleiro[i, j] != Jogador2
                                && no.Filhos[indiceMelhorFilho].Tabuleiro[i, j] != Jogador1)
                            {
                                coordenadaDaJogada = (i, j);
                            }
                        }
                    }

                    AtualizarTabuleiroPrincipal(no.Filhos[indiceMelhorFilho].Tabuleiro);

                    //Exibe jogada
                    Thread.Sleep(2000);

                    if(no.Filhos[indiceMelhorFilho].VerificarSeFechouQuadrado(coordenadaDaJogada, false) == true)
                    {
                        System.Console.WriteLine("IA marcou ponto");
                        Jogar(no.Filhos[indiceMelhorFilho], jogador);
                    }
                    else
                    {
                        Jogar(no.Filhos[indiceMelhorFilho], !jogador);
                    }
                }
                else
                {
                    int indiceFilhoCorreto = 0;

                    Console.WriteLine("Onde você quer jogar(número):");
                    string numero = Console.ReadLine() ?? "";

                    // Traduz o numero da tela para posição (x, y) da matriz
                    var coordenada = MapearPosicao(int.Parse(numero));

                    while(no.Tabuleiro[coordenada.linha, coordenada.coluna] != ' ')
                    { 
                        if(no.Tabuleiro[coordenada.linha, coordenada.coluna] == 'x')
                            Console.WriteLine("Outro jogador já fez essa jogada!");

                        Console.WriteLine("Digite um valor válido:");
                        numero = Console.ReadLine() ?? "";
                        coordenada = MapearPosicao(int.Parse(numero));
                    }

                    TabuleiroPrincipal[coordenada.linha, coordenada.coluna] = 'x';

                    foreach (var (filho, index) in no.Filhos.LoopIndex())
                    {
                        if (ComparaTabuleiro(filho.Tabuleiro))
                        {
                            indiceFilhoCorreto = index;
                        }
                    }
                    // no.Filhos[indiceFilhoCorreto].Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';

                    if(no.Filhos[indiceFilhoCorreto].VerificarSeFechouQuadrado(coordenada, true) == true)
                    {
                        System.Console.WriteLine("Player marcou ponto");
                        Jogar(no.Filhos[indiceFilhoCorreto], jogador);
                    }
                    else
                    {
                        Jogar(no.Filhos[indiceFilhoCorreto], !jogador);
                    }
                }
            }
            return 0;
        }

        private void SairDoJogo()
        {
            Console.WriteLine("Saindo...");
        }

        private void ExibirTabuleiroComJogadasPossiveis()
        {
            var noExemplo = new No();
            noExemplo.ExibirTabuleiroComJogadasPossiveis();
        }

        private void ExibirMensagemDeOpcaoInvalida()
        {
            Console.WriteLine("Opção inválida!");
        }

        private int EncontrarGanhador(No no)
        {
            if(!no.JogadasFinalizadas()) {
                return 2; // Jogo não finalizado ainda.
            }
            
            var locaisDePontuacao = new List<(int linha, int coluna)>(){
                (1, 1), (1, 3), (3, 1), (3, 3),
            };

            int pontuacaoJogador = 0;
            int pontuacaoMaquina = 0;

            foreach(var local in locaisDePontuacao)
            {
                if(no.Tabuleiro[local.linha, local.coluna] == Jogador1)
                    pontuacaoJogador++;
                else if(no.Tabuleiro[local.linha, local.coluna] == Jogador2)
                    pontuacaoMaquina++;
            }

            if(pontuacaoJogador == pontuacaoMaquina)
                return 0;

            return pontuacaoJogador > pontuacaoMaquina ? -1 : 1; 
        }

        public void ExibirTabuleiro()
        {
            Console.WriteLine("Exibindo tabuleiro...\n");

            for (int i = 0; i < DimensaoDoTabuleiro; i++)
            {
                for (int j = 0; j < DimensaoDoTabuleiro; j++)
                {
                    Console.Write(" " + TabuleiroPrincipal[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void AtualizarTabuleiroPrincipal(char[,] tabuleiroNovo)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    TabuleiroPrincipal[i, j] = tabuleiroNovo[i,j];
                }
            }
        }

        public bool ComparaTabuleiro(char[,] jogoDoNo)
        {
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(jogoDoNo[i, j] != TabuleiroPrincipal[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public (int linha, int coluna) MapearPosicao(int posicao)
            => posicao switch {
                1 => (0, 1),
                2 => (0, 3),
                3 => (1, 0),
                4 => (1, 2),
                5 => (1, 4),
                6 => (2, 1),
                7 => (2, 3),
                8 => (3, 0),
                9 => (3, 2),
                10 => (3, 4),
                11 => (4, 1),
                12 => (4, 3),
                _ => default,
            };
    }
}