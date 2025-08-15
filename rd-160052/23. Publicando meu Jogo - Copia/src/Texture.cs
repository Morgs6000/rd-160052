using OpenTK.Graphics.OpenGL4; // Fornece acesso às funções do OpenGL 4
using StbImageSharp; // Biblioteca para carregar imagens (texturas)

namespace RubyDung;

// Classe responsável por carregar, configurar e gerenciar texturas
public class Texture {
    private int texture; // Identificador da textura na GPU

    // Construtor da classe Texture
    public Texture(string path) {
        // Gera um identificador de textura
        texture = GL.GenTexture();
        // Vincula a textura para configurá-la
        GL.BindTexture(TextureTarget.Texture2D, texture);

        // Inverte a imagem verticalmente (OpenGL espera que a origem esteja no canto inferior esquerdo)
        StbImage.stbi_set_flip_vertically_on_load(1);

        // Carrega a imagem do arquivo especificado
        ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

        // Envia os dados da imagem para a GPU
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

        // Configura o comportamento de wrapping (repetição) da textura
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat); // Repete a textura no eixo S (horizontal)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat); // Repete a textura no eixo T (vertical)

        // Configura o filtro de minificação e magnificação da textura
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); // Filtro de minificação (quando a textura é reduzida)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); // Filtro de magnificação (quando a textura é ampliada)
    }

    // Método chamado antes de renderizar para vincular a textura
    public void OnRenderFrame() {
        GL.BindTexture(TextureTarget.Texture2D, texture);
    }
}
