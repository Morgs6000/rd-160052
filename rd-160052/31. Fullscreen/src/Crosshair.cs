using OpenTK.Mathematics;

namespace RubyDung;

public class Crosshair {
    private Shader shader;
    private Tesselator t;

    public Crosshair() {        
        // Cria uma instância do shader, carregando os arquivos de vertex e fragment shader
        shader = new Shader("src/shaders/texture_vertex.glsl", "src/shaders/texture_fragment.glsl");

        t = new Tesselator(shader);
        UpdateGUI();
    }

    public void OnRenderFrame(int width, int height) {
        // Ativa o shader para uso na renderização
        shader.OnRenderFrame();
        shader.SetColor("color0", 1.0f, 1.0f, 1.0f);

        t.OnRenderFrame();

        // Cria a matriz de visualização (view) a partir da posição e orientação do jogador
        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateTranslation((float)(width / 2), (float)(height / 2), 0.0f);
        view *= Matrix4.CreateTranslation(0.0f, 0.0f, -200.0f);
        shader.SetMatrix4("view", view); // Passa a matriz de visualização para o shader

        // Cria a matriz de projeção em perspectiva a partir do tamanho da janela
        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter(width, height);
        shader.SetMatrix4("projection", projection); // Passa a matriz de projeção para o shader
    }

    private void UpdateGUI() {
        t.Init();

        // Linha horizontal
        t.Vertex((float)(- 8), (float)(- 0), 0.0f);
        t.Vertex((float)(+ 9), (float)(- 0), 0.0f);
        t.Vertex((float)(+ 9), (float)(+ 1), 0.0f);
        t.Vertex((float)(- 8), (float)(+ 1), 0.0f);

        // Linha vertical
        t.Vertex((float)(- 0), (float)(- 8), 0.0f);
        t.Vertex((float)(+ 1), (float)(- 8), 0.0f);
        t.Vertex((float)(+ 1), (float)(+ 9), 0.0f);
        t.Vertex((float)(- 0), (float)(+ 9), 0.0f);

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