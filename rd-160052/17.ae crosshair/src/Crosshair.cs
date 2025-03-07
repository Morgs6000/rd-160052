using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace RubyDung;

public class Crosshair {
    private int width;
    private int height;

    private Shader shader;
    
    private int vao;
    private int vbo;

    public Crosshair(int width, int height) {
        this.width = width;
        this.height = height;

        shader = new Shader("src/shaders/crosshair_vertex.glsl", "src/shaders/crosshair_fragment.glsl");
    }

    public void OnLoad() {
        float[] vertices = {
            // Linha horizontal
            -0.5f,  0.0f,
             0.5f,  0.0f,
            // Linha vertical
             0.0f, -0.5f,
             0.0f,  0.5f
        };

        // Gerar e configurar o VAO e VBO
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void OnRenderFrame() {
        shader.OnRenderFrame();
        shader.SetColor("color0", 1.0f, 1.0f, 1.0f); // Cor branca

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Lines, 0, 4);
        GL.BindVertexArray(0);

        Matrix4 view = Matrix4.Identity;
        // view *= Matrix4.CreateScale(48.0f, 48.0f, 48.0f);
        view *= Matrix4.CreateScale(17.0f);
        view *= Matrix4.CreateTranslation((float)(width / 2), (float)(height / 2), 0.0f);
        view *= Matrix4.CreateTranslation(0.0f, 0.0f, -200.0f);
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter();
        shader.SetMatrix4("projection", projection);
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
