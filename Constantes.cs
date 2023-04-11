public static class Constantes
{
    public const int NumeroDeJogadasPossiveis = 12;
    public static int DimensaoDoTabuleiro = 5;
    public static bool[] JogadasPossiveis { get; set; } = new bool[12]{
        true, true, true, true,
        true, true, true, true,
        true, true, true, true,
    };
    public static char MarcadorDeJogada = 'x';
    public const char Jogador1 = '1'; 
    public const char Jogador2 = '2';
    public const char EspacoVazio = ' ';
    public const char CaracterDePonto = '*';
}
