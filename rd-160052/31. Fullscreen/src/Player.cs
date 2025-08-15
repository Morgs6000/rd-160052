using OpenTK.Mathematics; // Fornece funcionalidades matemáticas, como vetores e matrizes
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop; // Fornece funcionalidades para criar e gerenciar janelas
using OpenTK.Windowing.GraphicsLibraryFramework; // Fornece acesso ao GLFW para manipulação de entrada

namespace RubyDung;

// Classe que representa o jogador e suas propriedades relacionadas à câmera
public class Player {
    private Level level; // Referência ao nível (Level) ao qual o jogador pertence

    // Posição do jogador no espaço 3D
    public Vector3 position = Vector3.Zero; // Posição inicial no centro (0, 0, 0)

    private Vector3 position_d = Vector3.Zero;

    public AABB aabb;

    public bool onGround = false;

    // Vetores que definem a orientação da câmera
    // private Vector3 horizontal = Vector3.UnitX; // Vetor horizontal (eixo X)
    private Vector3 vertical = Vector3.UnitY;   // Vetor vertical (eixo Y)
    public Vector3 direction = Vector3.UnitZ;  // Vetor de direção (eixo Z)

    // Variáveis para controle de tempo e movimento suave
    private float deltaTime = 0.0f; // Tempo decorrido desde o último frame
    private float lastFrame = 0.0f; // Tempo do último frame

    private float walking = 4.317f; // Velocidade de movimento do jogador
    
    private Vector2 lastPos; // Última posição do mouse

    private float pitch; // Rotação vertical da câmera (para cima/baixo)
    private float yaw = -90.0f; // Rotação horizontal da câmera (para esquerda/direita)
    // private float roll; // Rotação de rotação (não utilizada neste exemplo)

    private bool firstMouse = true; // Indica se é o primeiro movimento do mouse

    private float sensitivity = 0.2f; // Sensibilidade do movimento do mouse

    private bool isGameReady = false; // Indica se o jogo está pronto para processar movimentos do mouse

    // Construtor da classe Player
    public Player(Level level) {
        this.level = level;
        ResetPos(); // Define a posição inicial do jogador
    }

    // Método chamado quando o jogo é carregado
    public void OnLoad(GameWindow gameWindow) {
        gameWindow.CursorState = CursorState.Grabbed; // Captura o cursor do mouse para dentro da janela
        
        // Marca o jogo como pronto para processar movimentos do mouse
        isGameReady = true;
    }

    // Método chamado a cada frame para atualizar a lógica do jogador
    public void OnUpdateFrame(GameWindow gameWindow) {
        Time(); // Atualiza o tempo decorrido
        ProcessInput(gameWindow.KeyboardState); // Processa a entrada do teclado

        MouseCallBack(gameWindow.MouseState); // Processa o movimento do mouse
    }

    // Método para redefinir a posição do jogador para uma posição aleatória no nível
    private void ResetPos() {
        Random random = new Random();
        float x = (float)random.NextDouble() * (float)level.width;
        float y = (float)(level.height + 10);
        float z = (float)random.NextDouble() * (float)level.depth;

        SetPos(x, y, z); // Define a nova posição do jogador
    }

    // Método para definir a posição do jogador
    private void SetPos(float x, float y, float z) {
        position.X = x;
        position.Y = y;
        position.Z = z;

        float w = 0.3f;
        float h = 0.9f;

        aabb = new AABB(x - w, y - h, z - w, x + w, y + h, z + w);
    }

    // Método para processar o movimento do mouse e atualizar a direção da câmera
    public void MouseCallBack(MouseState mouseState) {
        if(!isGameReady) {
            return; // Ignora o movimento do mouse até que o jogo esteja pronto
        }

        if(firstMouse) {
            // Se for o primeiro movimento do mouse, armazena a posição inicial
            lastPos = new Vector2(mouseState.X, mouseState.Y);
            firstMouse = false;
        }
        else {
            // Calcula a diferença entre a posição atual e a última posição do mouse
            float deltaX = mouseState.X - lastPos.X;
            float deltaY = mouseState.Y - lastPos.Y;
            lastPos = new Vector2(mouseState.X, mouseState.Y);

            // Atualiza o pitch (rotação vertical) e o yaw (rotação horizontal) com base no movimento do mouse
            pitch -= deltaY * sensitivity;
            yaw   += deltaX * sensitivity;

            // Limita o pitch para evitar rotações extremas
            if(pitch < -89.0f) {
                pitch = -89.0f;
            }
            if(pitch > 89.0f) {
                pitch = 89.0f;
            }
        }

        // Atualiza a direção da câmera com base no pitch e yaw
        direction.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
        direction.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
        direction.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
        direction = Vector3.Normalize(direction); // Normaliza o vetor de direção
    }

    // Método para calcular o tempo decorrido desde o último frame
    private void Time() {
        float currentFrame = (float)GLFW.GetTime(); // Obtém o tempo atual
        deltaTime = currentFrame - lastFrame; // Calcula o tempo decorrido
        lastFrame = currentFrame; // Atualiza o tempo do último frame
    }

    // Método para processar a entrada do teclado e mover o jogador
    private void ProcessInput(KeyboardState keyboardState) {
        float speed = walking * deltaTime; // Calcula a velocidade de movimento com base no tempo decorrido

        float xa = 0.0f; // Movimento no eixo X
        float ya = 0.0f; // Movimento no eixo Y
        float za = 0.0f; // Movimento no eixo Z

        // Verifica se a tecla R foi pressionada para redefinir a posição do jogador
        if(keyboardState.IsKeyDown(Keys.R)) {
            ResetPos();
        }

        // Verifica as teclas pressionadas e define a direção do movimento
        if(keyboardState.IsKeyDown(Keys.W)) {
            za++; // Move para frente
        }
        if(keyboardState.IsKeyDown(Keys.S)) {
            za--; // Move para trás
        }
        if(keyboardState.IsKeyDown(Keys.A)) {
            xa--; // Move para a esquerda
        }
        if(keyboardState.IsKeyDown(Keys.D)) {
            xa++; // Move para a direita
        }
        
        if(keyboardState.IsKeyDown(Keys.Space)) {
            ya++; // Move para cima
        }
        if(keyboardState.IsKeyDown(Keys.LeftShift)) {
            ya--; // Move para baixo
        }
        // if(keyboardState.IsKeyDown(Keys.Space) && onGround) {
        //     pos_d.Y =  0.12f;
        // }

        // Atualiza a posição do jogador com base na direção do movimento
        // position += xa * speed * Vector3.Normalize(Vector3.Cross(direction, vertical)); // Movimento horizontal
        // position += ya * speed * vertical; // Movimento vertical
        // position += za * speed * Vector3.Normalize(new Vector3(direction.X, 0.0f, direction.Z)); // Movimento para frente/trás

        // MoveRelative(xa, za, onGround ? 0.02f : 0.005f);
        MoveRelative(xa, ya, za, speed);

        //pos_d.Y = (float)((double)pos_d.Y - 0.005f);

        Move(position_d.X, position_d.Y, position_d.Z);

        // this.pos_d.X *= 0.91f;
        // this.pos_d.Y *= 0.98f;
        // this.pos_d.X *= 0.91f;

        // if (this.onGround) {
        //     this.pos_d.X *= 0.8F;
        //     this.pos_d.Z *= 0.8F;
        // }
    }

    /*
    public void Move(float xa, float ya, float za) {
        float xaOrg = xa;
        float yaOrg = ya;
        float zaOrg = za;

        List<AABB> AABBs = level.GetCubes(aabb.Expand(xa, ya, za));

        for(int i = 0; i < AABBs.Count; i++) {
            ya = AABBs[i].ClipYCollide(aabb, ya);
        }

        aabb.Move(0.0f, ya, 0.0f);

        for (int i = 0; i < AABBs.Count; i++) {
            xa = AABBs[i].ClipXCollide(aabb, xa);
        }

        aabb.Move(xa, 0.0f, 0.0f);

        for (int i = 0; i < AABBs.Count; ++i) {
            za = AABBs[i].ClipZCollide(aabb, za);
        }

        aabb.Move(0.0f, 0.0f, za);

        this.onGround = yaOrg != ya && yaOrg < 0.0F;

        if (xaOrg != xa) {
            this.pos_d.X = 0.0F;
        }

        if (yaOrg != ya) {
            this.pos_d.Y = 0.0F;
        }

        if (zaOrg != za) {
            this.pos_d.Z = 0.0F;
        }

        this.position.X = (aabb.x0 + aabb.x1) / 2.0F;
        this.position.Y = aabb.y0 + 1.62F;
        this.position.Z = (aabb.z0 + aabb.z1) / 2.0F;
    }
    */
    public void Move(float xa, float ya, float za) {
        position.X += xa;
        position.Y += ya;
        position.Z += za;

        List<AABB> AABBs = level.GetCubes(aabb.Expand(xa, ya, za));

        for(int i = 0; i < AABBs.Count; i++) {
            ya = AABBs[i].ClipYCollide(aabb, ya);
        }

        aabb.Move(0.0f, ya, 0.0f);

        for (int i = 0; i < AABBs.Count; i++) {
            xa = AABBs[i].ClipXCollide(aabb, xa);
        }

        aabb.Move(xa, 0.0f, 0.0f);

        for (int i = 0; i < AABBs.Count; ++i) {
            za = AABBs[i].ClipZCollide(aabb, za);
        }

        aabb.Move(0.0f, 0.0f, za);

        this.position.X = (aabb.x0 + aabb.x1) / 2.0F;
        this.position.Y = aabb.y0 + 1.62F;
        this.position.Z = (aabb.z0 + aabb.z1) / 2.0F;
    }

    /*
    public void MoveRelative(float xa, float za, float speed) {
        float dist = xa * xa + za * za;

        if (!(dist < 0.01F)) {
            dist = speed / (float)Math.Sqrt((double)dist);

            xa *= dist;
            za *= dist;

            float sin = (float)Math.Sin((double)this.yaw * Math.PI / 180.0);
            float cos = (float)Math.Cos((double)this.yaw * Math.PI / 180.0);

            this.pos_d.X += xa * cos - za * sin;
            this.pos_d.Z += za * cos + xa * sin;
        }
    }
    */
    public void MoveRelative(float xa, float ya, float za, float speed) {
        position_d = Vector3.Zero;

        position_d += xa * speed * Vector3.Normalize(Vector3.Cross(direction, vertical));
        position_d += ya * speed * vertical;
        position_d += za * speed * Vector3.Normalize(new Vector3(direction.X, 0.0f, direction.Z));

        position += position_d;
    }

    // Método para criar uma matriz de visualização (LookAt)
    public Matrix4 LookAt() {
        // Define o ponto de observação (olho) como a posição do jogador
        Vector3 eye = position;

        // Define o ponto de destino (alvo) como a posição do jogador mais a direção
        Vector3 target = position + direction;

        // Define o vetor "up" como o vetor vertical
        Vector3 up = vertical;

        // Retorna a matriz de visualização (LookAt) que define como a cena é vista
        return Matrix4.LookAt(eye, target, up);
    }

    // Método para criar uma matriz de projeção em perspectiva
    public Matrix4 CreatePerspectiveFieldOfView(int width, int height) {
        // Define o campo de visão vertical (fovy) em radianos
        float fovy = MathHelper.DegreesToRadians(70.0f);

        // Define a proporção da tela (aspect ratio) como largura dividida pela altura
        float aspect = (float)width / (float)height;

        // Define a distância do plano de corte próximo (depthNear)
        float depthNear = 0.05f;

        // Define a distância do plano de corte distante (depthFar)
        float depthFar = 1000.0f;

        // Retorna a matriz de projeção em perspectiva
        return Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar);
    }
}
