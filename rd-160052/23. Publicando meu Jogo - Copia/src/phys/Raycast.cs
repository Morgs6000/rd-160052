using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Raycast {
    private Level level;
    private LevelRenderer levelRenderer;
    private Player player;

    private Shader shader;
    private Tesselator t;
    private HitResult hitResult = null;

    public Raycast(Level level, LevelRenderer levelRenderer, Player player) {
        this.level = level;
        this.levelRenderer = levelRenderer;
        this.player = player;

        // Cria uma instância do shader, carregando os arquivos de vertex e fragment shader
        shader = new Shader("src/shaders/highlight_vertex.glsl", "src/shaders/highlight_fragment.glsl");

        t = new Tesselator(shader);

        hitResult = new HitResult(0, 0, 0, -1);
    }

    public void OnUpdateFrame(GameWindow gameWindow) {
        MouseState mouseState = gameWindow.MouseState;

        if(Target()) {
            RenderHit(hitResult);

            if(mouseState.IsButtonPressed(MouseButton.Right)) {
                SetBlock(hitResult.x, hitResult.y, hitResult.z, 0);
            }

            if(mouseState.IsButtonPressed(MouseButton.Left)) {
                int x = hitResult.x;
                int y = hitResult.y;
                int z = hitResult.z;

                if(hitResult.f == 0) {
                    x--;
                }
                if(hitResult.f == 1) {
                    x++;
                }
                if(hitResult.f == 2) {
                    y--;
                }
                if(hitResult.f == 3) {
                    y++;
                }
                if(hitResult.f == 4) {
                    z--;
                }
                if(hitResult.f == 5) {
                    z++;
                }

                SetBlock(x, y, z, 1);
            }
        }
        else {
            t.Init();
        }
    }

    public void OnRenderFrame(Vector2i clientSize) {
        shader.OnRenderFrame();

        t.OnRenderFrame();

        // Cria a matriz de visualização (view) a partir da posição e orientação do jogador
        Matrix4 view = Matrix4.Identity;
        view *= player.LookAt();
        shader.SetMatrix4("view", view); // Passa a matriz de visualização para o shader

        // Cria a matriz de projeção em perspectiva a partir do tamanho da janela
        Matrix4 projection = Matrix4.Identity;
        projection *= player.CreatePerspectiveFieldOfView(clientSize);
        shader.SetMatrix4("projection", projection); // Passa a matriz de projeção para o shader
    }

    private void RenderHit(HitResult h) {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)Math.Sin((double)Environment.TickCount / 100.0f) * 0.2f + 0.4f;
        shader.SetColor("color0", 1.0f, 1.0f, 1.0f, 0.5f);

        t.Init();
        Tile.rock.RenderFace(t, h.x, h.y, h.z, h.f);
        t.OnLoad();

        //GL.Disable(EnableCap.Blend);
    }

    private bool Target() {
        // Posição e direção da camera
        Vector3 rayOrigin = player.position;
        Vector3 rayDirection = player.direction;

        // Tamanho do passo para o raycasting
        float step = 0.1f;
        float maxDistance = 5.0f; // Distancia maxima para verificar colisões

        Vector3 currentPos = rayOrigin;

        for(float distante = 0; distante < maxDistance; distante += step) {
            currentPos += rayDirection * step;

            hitResult.x = (int)Math.Floor(currentPos.X);
            hitResult.y = (int)Math.Floor(currentPos.Y);
            hitResult.z = (int)Math.Floor(currentPos.Z);

            // Verifica se há um bloco solido nessa posição
            if(level.IsSolidTile(hitResult.x, hitResult.y, hitResult.z)) {
                // Calcula a face do bloco que foi atingida
                hitResult.f = CalculateHitFace(rayDirection, currentPos, new Vector3i(hitResult.x, hitResult.y, hitResult.z));

                return true;
            }
        } 

        hitResult.f = -1;

        return false;
    }

    public void SetBlock(int x, int y, int z, byte id) {
        level.SetTile(x, y, z, id);

        int chunkX = x / 16;
        int chunkY = y / 16;
        int chunkZ = z / 16;

        levelRenderer.ChunkReloadNeighbors(chunkX, chunkY, chunkZ);
    }

    private int CalculateHitFace(Vector3 rayDirection, Vector3 hitPos, Vector3i blockPos) {
        // Converte a posição do bloco para um vetor 3D
        Vector3 blockCenter = new Vector3(blockPos.X + 0.5f, blockPos.Y + 0.5f, blockPos.Z + 0.5f);

        // Calcula o vetor do centro do bloco até o ponto de colisão
        Vector3 hitVector = hitPos - blockCenter;

        // Determina qual face foi atingida com base na direçaõ do raycat e no vetor de colisão
        float epsilon = 0.0001f; // Margem de erro para comparações de ponto flutuante

        // Verifica qual eixo tem a maior componente do vetor de colisão
        if(Math.Abs(hitVector.X) > Math.Abs(hitVector.Y) && Math.Abs(hitVector.X) > Math.Abs(hitVector.Z)) {
            // Face no eixo X
            if(hitVector.X > epsilon) {
                return 1; // Face x1 (direito)
            }
            else if(hitVector.X < -epsilon) {
                return 0; // Face x0 (esquerda)
            }
        }
        else if(Math.Abs(hitVector.Y) > Math.Abs(hitVector.X) && Math.Abs(hitVector.Y) > Math.Abs(hitVector.Z)) {
            // Face no eixo Y
            if(hitVector.Y > epsilon) {
                return 3; // Face y1 (topo)
            }
            else if(hitVector.Y < -epsilon) {
                return 2; // Face y0 (base)
            }
        }
        else if(Math.Abs(hitVector.Z) > Math.Abs(hitVector.X) && Math.Abs(hitVector.Z) > Math.Abs(hitVector.Y)) {
            // Face no eixo Z
            if(hitVector.Z > epsilon) {
                return 5; // Face z1 (trás)
            }
            else if(hitVector.Z < -epsilon) {
                return 4; // Face z0 (frente)
            }
        }

        return -1;
    }
}
