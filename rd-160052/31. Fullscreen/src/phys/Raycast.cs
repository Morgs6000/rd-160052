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

    private DrawGUI drawGUI;

    public Raycast(Level level, LevelRenderer levelRenderer, Player player, DrawGUI drawGUI) {
        this.level = level;
        this.levelRenderer = levelRenderer;
        this.player = player;
        this.drawGUI = drawGUI;

        // Cria uma instância do shader, carregando os arquivos de vertex e fragment shader
        shader = new Shader("src/shaders/texture_vertex.glsl", "src/shaders/texture_fragment.glsl");

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

                SetBlock(x, y, z, (byte)drawGUI.paintTexture);
            }
        }
        else {
            t.Init();
        }
    }

    public void OnRenderFrame(int width, int height) {
        shader.OnRenderFrame();

        t.OnRenderFrame();

        // Cria a matriz de visualização (view) a partir da posição e orientação do jogador
        Matrix4 view = Matrix4.Identity;
        view *= player.LookAt();
        shader.SetMatrix4("view", view); // Passa a matriz de visualização para o shader

        // Cria a matriz de projeção em perspectiva a partir do tamanho da janela
        Matrix4 projection = Matrix4.Identity;
        projection *= player.CreatePerspectiveFieldOfView(width, height);
        shader.SetMatrix4("projection", projection); // Passa a matriz de projeção para o shader
    }

    private void RenderHit(HitResult h) {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)Math.Sin((double)Environment.TickCount / 100.0f) * 0.2f + 0.4f;
        shader.SetColor("color0", 1.0f, 1.0f, 1.0f, alpha);

        t.Init();
        Tile.rock.RenderFaceNoTexture(t, h.x, h.y, h.z, h.f);
        t.OnLoad();

        //GL.Disable(EnableCap.Blend);
    }

    private bool Target() {
        // Posição e direção da camera
        Vector3 rayOrigin = player.position;
        Vector3 rayDirection = player.direction;

        float maxDistance = 5.0f; // Distancia maxima para verificar colisões

        /* Digital Differential Analyzer */

        // Inicializa o algoritmo DDA
        int mapX = (int)Math.Floor(rayOrigin.X);
        int mapY = (int)Math.Floor(rayOrigin.Y);
        int mapZ = (int)Math.Floor(rayOrigin.Z);

        float deltaDistX = Math.Abs(1 / rayDirection.X);
        float deltaDistY = Math.Abs(1 / rayDirection.Y);
        float deltaDistZ = Math.Abs(1 / rayDirection.Z);

        float sizeDistX;
        float sizeDistY;
        float sizeDistZ;

        int stepX;        
        int stepY;        
        int stepZ;

        // Determina a direção do passo e distancias laterais iniciais
        if(rayDirection.X < 0) {
            stepX = -1;
            sizeDistX = (rayOrigin.X - mapX) * deltaDistX;
        }
        else {
            stepX = 1;
            sizeDistX = (mapX + 1.0f - rayOrigin.X) * deltaDistX;
        }
        
        if(rayDirection.Y < 0) {
            stepY = -1;
            sizeDistY = (rayOrigin.Y - mapY) * deltaDistY;
        }
        else {
            stepY = 1;
            sizeDistY = (mapY + 1.0f - rayOrigin.Y) * deltaDistY;
        }

        if(rayDirection.Z < 0) {
            stepZ = -1;
            sizeDistZ = (rayOrigin.Z - mapZ) * deltaDistZ;
        }
        else {
            stepZ = 1;
            sizeDistZ = (mapZ + 1.0f - rayOrigin.Z) * deltaDistZ;
        }

        // Algoritimo DDA
        int size = 0; // Qual lado foi atingido (0 = x, 1 = y, 2 = z)
        float distance = 0;

        while(distance < maxDistance) {
            // Pula para o proximo quadrado do grid
            if(sizeDistX < sizeDistY && sizeDistX < sizeDistZ) {
                sizeDistX += deltaDistX;
                mapX += stepX;
                size = 0;
            }
            else if(sizeDistY < sizeDistZ) {
                sizeDistY += deltaDistY;
                mapY += stepY;
                size = 1;
            }
            else {
                sizeDistZ += deltaDistZ;
                mapZ += stepZ;
                size = 2;
            }

            distance = Math.Min(sizeDistX, Math.Min(sizeDistY, sizeDistZ));

            // Verifica se este bloco é solido
            if(level.IsSolidTile(mapX, mapY, mapZ)) {
                hitResult.x = mapX;
                hitResult.y = mapY;
                hitResult.z = mapZ;

                // Determina qual face foi atingida baseado no lado e direção do passo
                if(size == 0) {
                    hitResult.f = (stepX > 0) ? 0 : 1; // Oeste ou Leste
                }
                else if(size == 1) {
                    hitResult.f = (stepY > 0) ? 2 : 3; // Baixo ou Topo
                }
                else {
                    hitResult.f = (stepZ > 0) ? 4 : 5; // Norte ou Sul
                }

                return true;
            }        
        }

        hitResult.f = -1;

        return false;
    }

    public void SetBlock(int x, int y, int z, byte id) {
        level.SetTile(x, y, z, id);

        levelRenderer.SetChunk(x - 1, y - 1, z - 1, x + 1, y + 1, z + 1);
    }
}
