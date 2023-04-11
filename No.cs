using static Constantes;

namespace DotsGame.Core
{
    public class No
    {
        public char[,] Tabuleiro { get; private set; }
        public List<No> Filhos { get; set; }
        public int ValorMinMax { get; set; }

        public No()
        {
            Filhos = new List<No>();
            Tabuleiro = new char[DimensaoDoTabuleiro, DimensaoDoTabuleiro];
            ResetaTabuleiro();
        }

        private void ResetaTabuleiro()
        {
            LimparTabuleiro();
            PreencherPontosLimitadores();
            LimparJogadas();
            LimparPontuacao();
        }

        public void ExibirTabuleiroTutorial()
        {
            PreencherPontosLimitadores();
            PreencherJogadasPossiveis();
            PreencherExemploDePontuacao();
            ExibirTabuleiro();
        }

        public void LimparTabuleiro()
        {
            for (int i = 0; i < DimensaoDoTabuleiro; i++)
                for (int j = 0; j < DimensaoDoTabuleiro; j++)
                    Tabuleiro[i, j] = ' ';
        }

        private void LimparJogadas()
        {
            for(int i = 0; i < DimensaoDoTabuleiro; i++)
            {
                if(i % 2 == 0){
                    for(int j = 0; j < DimensaoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = ' ';
                } else {
                    for(int k = 0; k < DimensaoDoTabuleiro; k++)
                        if(k % 2 == 0)
                            Tabuleiro[i, k] = ' ';
                }
            }
        }

        private void LimparPontuacao()
        {
            for(int i = 0; i < DimensaoDoTabuleiro; i++)
                if(i % 2 == 1)
                    for(int j = 0; j < DimensaoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = ' ';
        }

        public void PreencherPontosLimitadores()
        {
            for(int i = 0; i < DimensaoDoTabuleiro; i++)
                if(i % 2 == 0)
                    for(int j = 0; j < DimensaoDoTabuleiro; j++)
                        if(j % 2 == 0)
                            Tabuleiro[i, j] = '*';
        }

        public void PreencherJogadasPossiveis()
        {
            for(int i = 0; i < DimensaoDoTabuleiro; i++)
            {
                if(i % 2 == 0){
                    for(int j = 0; j < DimensaoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = '-';
                } else {
                    for(int k = 0; k < DimensaoDoTabuleiro; k++)
                        if(k % 2 == 0)
                            Tabuleiro[i, k] = '|';
                }
            }
        }

        private void PreencherExemploDePontuacao()
        {
            for(int i = 0; i < DimensaoDoTabuleiro; i++)
                if(i % 2 == 1)
                    for(int j = 0; j < DimensaoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = '2';
        }

        private void ExibirTabuleiro()
        {
            for (int i = 0; i < DimensaoDoTabuleiro; i++)
            {
                for (int j = 0; j < DimensaoDoTabuleiro; j++)
                {
                    Console.Write(" " + Tabuleiro[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void ExibirTabuleiroComJogadasPossiveis()
        {
            Console.WriteLine("Exibindo tabuleiro com jogadas possíveis...\n");
            
            Console.WriteLine("*   1  *   2   *\n");
            Console.WriteLine("3   *  4   *   5\n");
            Console.WriteLine("*   6  *   7   *\n");
            Console.WriteLine("8   *  9   *   10\n");
            Console.WriteLine("*  11  *   12  *\n");
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

        public void CopiaMatrizDoPai(No noPai)
        {
            for (int i = 0; i < DimensaoDoTabuleiro; i++)
            {
                for (int j = 0; j < DimensaoDoTabuleiro; j++)
                {
                    Tabuleiro[i, j] = noPai.Tabuleiro[i, j];
                }
            }
        }

        public bool VerificarSeFechouQuadrado((int linha, int coluna) coordenada, bool jogador)
        {
            var contadorDePontuacoes = 0;
            bool jogadorPontuou = false;

            // Verifica pra esquerda, se puder
            if(coordenada.coluna - 2 >= 0 && (coordenada.linha - 1 >= 0 && coordenada.linha + 1 < DimensaoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoAEsquerda(coordenada.linha, coordenada.coluna, jogador);
                if(jogadorPontuou)
                    contadorDePontuacoes++;

            // Verifica pra direita, se puder
            if(coordenada.coluna + 2 < DimensaoDoTabuleiro && (coordenada.linha - 1 >= 0 && coordenada.linha + 1 < DimensaoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoADireita(coordenada.linha, coordenada.coluna, jogador);
                if(jogadorPontuou)
                    contadorDePontuacoes++;

            // //Verifica pra cima, se puder
            if(coordenada.linha - 2 >= 0 && (coordenada.coluna - 1 >= 0 && coordenada.coluna + 1 < DimensaoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoAcima(coordenada.linha, coordenada.coluna, jogador);
                if(jogadorPontuou)
                    contadorDePontuacoes++;

            //Verifica pra baixo, se puder
            if(coordenada.linha + 2 < DimensaoDoTabuleiro && (coordenada.coluna - 1 >= 0 && coordenada.coluna + 1 < DimensaoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoAbaixo(coordenada.linha, coordenada.coluna, jogador);
                if(jogadorPontuou)
                    contadorDePontuacoes++;
            
            return contadorDePontuacoes > 0 ? true : false;
        }

        private bool VerificaQuadradoAEsquerda(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha, coluna - 2] != EspacoVazio
                && Tabuleiro[linha + 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha, coluna - 1] != CaracterDePonto)
            {
                Tabuleiro[linha, coluna - 1] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private bool VerificaQuadradoADireita(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha, coluna + 2] != EspacoVazio
                && Tabuleiro[linha + 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha, coluna + 1] != CaracterDePonto)
            {
                Tabuleiro[linha, coluna + 1] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private bool VerificaQuadradoAcima(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha - 2, coluna] != EspacoVazio
                && Tabuleiro[linha - 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna] != CaracterDePonto)
            {
                Tabuleiro[linha - 1, coluna] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private bool VerificaQuadradoAbaixo(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha + 2, coluna] != EspacoVazio
                && Tabuleiro[linha + 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha + 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha + 1, coluna] != CaracterDePonto)
            {
                Tabuleiro[linha + 1, coluna] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private char SimboloDoJogador(bool jogador)
            => jogador ? Jogador1 : Jogador2;
        
        public bool JogadasFinalizadas()
            => (Tabuleiro[1, 1] != EspacoVazio 
                && Tabuleiro[1, 3] != EspacoVazio 
                && Tabuleiro[3, 1] != EspacoVazio 
                && Tabuleiro[3, 3] != EspacoVazio);
    }
}