# DotsGame
Jogo de Dots desenvolvido durante a disciplina de Inteligência Artificial.

#### 🕹️ Como rodar o jogo

Para rodar o jogo, é necessário ter o [Runtime do .NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-7.0.3-windows-x64-installer) instalado. Após fazer a instalação, basta digitar o seguinte comando no terminal do vscode:

```csharp
dotnet run
```

#### 💡 Tecnologias e Estruturas Utilizadas

O projeto do jogo foi desenvolvido utilizando a linguagem C#, mais especificamente usando o framework .NET. Para que fosse possível fazer o jogo, era necessário que houvesse uma árvore de possibilidades, que armazenaria todo o conhecimento da "IA", para isso, foi utilizado o método `PreencherArvoreDePossibilidades()`, que percorre uma arvore com 10 de profundidade que guarda cada jogada possível em um nó, em uma cópia do tabuleiro naquele momento. O tabuleiro é armazenado em uma matriz de caracteres que é inicialmente formatada para exibir os pontos e depois as jogadas possíveis numeradas de 1 a 12. Ao escolher uma jogada, esse numero é mapeado para uma coordenada **x** e **y** e um simbolo é marcado no tabuleiro para contabilizar a jogada. Para que a "IA" saiba qual a melhor jogada possivel, foi utilizado um algoritmo de MinMax para pontuar quais nós levariam a uma vitória da máquina. Ao finalizar as 12 jogadas, o jogador que tiver a maior quantidade de quadrados fechados ganha.

#### 🚧 Dificuldades encontradas

Durante o desenvolvimento desse jogo, foram encontrados alguns problemas em relação ao uso do MinMax, pois muitas vezes, embora a implementação do algoritmo estivesse correta, aconteciam jogadas erradas por parte da "IA", como se ela não tivesse aprendido. Outros problemas foram em relação aos métodos recursivos que dificultavam o uso de ferramentas como o modo debug para identificar o problema.
