using OpenTK.Mathematics; // Fornece funcionalidades matemáticas, como vetores e matrizes
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input; // Fornece funcionalidades para manipulação de entrada, como ícones de janela
using OpenTK.Windowing.Desktop; // Fornece funcionalidades para criar e gerenciar janelas
using StbImageSharp; // Biblioteca para carregar imagens (usada para carregar o ícone da janela)

namespace RubyDung;

// Classe principal do programa
public class Program {
    // Método de entrada do programa
    private static void Main(string[] args) {
        try {
            Console.WriteLine("Hello, World!");

            // Configurações padrão para a janela do jogo
            GameWindowSettings gws = GameWindowSettings.Default;

            // Configurações personalizadas para a janela nativa
            NativeWindowSettings nws = NativeWindowSettings.Default;
            nws.ClientSize = new Vector2i(1024, 768); // Define o tamanho da janela como 1024x768 pixels
            nws.Title = "Game"; // Define o título da janela como "Game"

            // Carrega a imagem do ícone da janela
            var strem = File.OpenRead("src/textures/openTK_logo.png");
            var image = ImageResult.FromStream(strem, ColorComponents.RedGreenBlueAlpha); // Carrega a imagem usando StbImageSharp
            var icon = new WindowIcon(new Image(image.Width, image.Height, image.Data)); // Cria um ícone de janela a partir da imagem
            nws.Icon = icon; // Define o ícone da janela

            if(RubyDung.FULLSCREEN_MODE) {
                nws.WindowState = WindowState.Fullscreen;
            }

            // Cria uma instância da classe RubyDung (que herda de GameWindow) e inicia o loop principal do jogo
            new RubyDung(gws, nws).Run();
        }
        catch(Exception e) {
            Console.WriteLine("Erro:" + e.Message);
            Console.WriteLine("Pressione Enter para sair...");
            Console.ReadLine(); // Mantém o terminal aberto
        }
    }
}
