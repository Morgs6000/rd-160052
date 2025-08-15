using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class DrawGUI {
    private int width;
    private int height;

    private Shader shader;
    private Texture texture;
    private Level level;

    private Tesselator t;
    public int paintTexture = 1;

    public DrawGUI(int width, int height, Level level) {
        this.width = width;
        this.height = height;

        shader = new Shader("src/shaders/texture_vertex.glsl", "src/shaders/texture_fragment.glsl");

        texture = new Texture("src/textures/terrain.png");

        this.level = level;
    }

    public void OnLoad() {
        shader.OnRenderFrame();

        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateTranslation(1.5f, -0.5f, -0.5f);
        view *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45.0f));
        view *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(30.0f));
        view *= Matrix4.CreateScale(48.0f, 48.0f, 48.0f);
        view *= Matrix4.CreateTranslation((float)(width - 48.0f), (float)(height - 48.0f), 0.0f);
        view *= Matrix4.CreateTranslation(0.0f, 0.0f, -200.0f);
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter();
        shader.SetMatrix4("projection", projection);

        t = new Tesselator(shader);
        t.Init();
        Tile.tiles[paintTexture].OnLoad(t, level, -2, 0, 0);
        t.OnLoad();
    }

    public void OnUpdateFrame(GameWindow window) {
        KeyboardState keyboardState = window.KeyboardState;

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

    public void OnRenderFrame() {
        shader.OnRenderFrame();
        texture.OnRenderFrame();
        t.OnRenderFrame();
    }

    private void UpdateGUI() {
        t.Init();
        Tile.tiles[paintTexture].OnLoad(t, level, -2, 0, 0);
        t.OnLoad();

        shader.OnRenderFrame();
        texture.OnRenderFrame();
        t.OnRenderFrame();
    }

    private Matrix4 CreateOrthographicOffCenter() {
        float left = 0.0f;
        float right = (float)width;
        float bottom = 0.0f;
        float top = (float)height;
        float depthNear = 100.0f;
        float depthFar  = 300.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    }
}
