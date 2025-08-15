using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class DrawGUI {
    private Level level;

    private Shader shader;
    private Texture texture;

    private Tesselator t;
    public int paintTexture = 1;

    public DrawGUI(Level level) {  
        this.level = level;
              
        // Cria uma instância do shader, carregando os arquivos de vertex e fragment shader
        shader = new Shader("src/shaders/texture_vertex.glsl", "src/shaders/texture_fragment.glsl");
        
        // Cria uma instância da textura, carregando a imagem do arquivo especificado
        texture = new Texture("src/textures/terrain.png");

        t = new Tesselator(shader);
        UpdateGUI();
    }

    public void OnUpdateFrame(GameWindow gameWindow) {
        KeyboardState keyboardState = gameWindow.KeyboardState;

        if(keyboardState.IsKeyPressed(Keys.D1)) {
            paintTexture = 1;
            UpdateGUI();
        }
        if(keyboardState.IsKeyPressed(Keys.D2)) {
            paintTexture = 3;
            UpdateGUI();
        }
        if(keyboardState.IsKeyPressed(Keys.D3)) {
            paintTexture = 4;
            UpdateGUI();
        }
        if(keyboardState.IsKeyPressed(Keys.D4)) {
            paintTexture = 5;
            UpdateGUI();
        }
    }

    public void OnRenderFrame(int width, int height) {        
        // Ativa o shader para uso na renderização
        shader.OnRenderFrame();

        // Vincula a textura para uso na renderização
        texture.OnRenderFrame();

        t.OnRenderFrame();

        // Cria a matriz de visualização (view) a partir da posição e orientação do jogador
        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateTranslation(1.5f, -0.5f, -0.5f);
        view *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45.0f));
        view *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(30.0f));
        view *= Matrix4.CreateScale(48.0f);
        view *= Matrix4.CreateTranslation((float)(width - 48.0f), (float)(height - 48.0f), 0.0f);
        view *= Matrix4.CreateTranslation(0.0f, 0.0f, -200.0f);
        shader.SetMatrix4("view", view); // Passa a matriz de visualização para o shader

        // Cria a matriz de projeção em perspectiva a partir do tamanho da janela
        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter(width, height);
        shader.SetMatrix4("projection", projection); // Passa a matriz de projeção para o shader
    }

    private void UpdateGUI() {
        t.Init();
        Tile.tiles[paintTexture].OnLoad(t, level, -2, 0, 0);
        t.OnLoad();
    }

    // Método para criar uma matriz de projeção em perspectiva
    public Matrix4 CreateOrthographicOffCenter(int width, int height) {
        float left = 0.0f;
        float right = (float)width;
        float bottom = 0.0f;
        float top = (float)height;

        // Define a distância do plano de corte próximo (depthNear)
        float depthNear = 100.0f;

        // Define a distância do plano de corte distante (depthFar)
        float depthFar = 300.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    }
}
