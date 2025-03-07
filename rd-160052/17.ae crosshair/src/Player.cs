using OpenTK.Mathematics; // Fornece funcionalidades matemáticas, como vetores e matrizes
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop; // Fornece funcionalidades para criar e gerenciar janelas
using OpenTK.Windowing.GraphicsLibraryFramework; // Fornece acesso ao GLFW para manipulação de entrada

namespace RubyDung;

// Classe que representa o jogador e suas propriedades relacionadas à câmera
public class Player {
    private Level level; // Referência ao nível (Level) ao qual o jogador pertence

    // Posição do jogador no espaço 3D
    private Vector3 postion = Vector3.Zero; // Posição inicial no centro (0, 0, 0)

    // Vetores que definem a orientação da câmera
    private Vector3 horizontal = Vector3.UnitX; // Vetor horizontal (eixo X)
    private Vector3 vertical = Vector3.UnitY;   // Vetor vertical (eixo Y)
    private Vector3 direction = Vector3.UnitZ;  // Vetor de direção (eixo Z)

    // Variáveis para controle de tempo e movimento suave
    private float deltaTime = 0.0f; // Tempo decorrido desde o último frame
    private float lastFrame = 0.0f; // Tempo do último frame

    private float walking = 4.317f; // Velocidade de movimento do jogador
    
    private Vector2 lastPos; // Última posição do mouse

    private float pitch; // Rotação vertical da câmera (para cima/baixo)
    private float yaw = -90.0f; // Rotação horizontal da câmera (para esquerda/direita)
    private float roll; // Rotação de rotação (não utilizada neste exemplo)

    private bool firstMouse = true; // Indica se é o primeiro movimento do mouse

    private float sensitivity = 0.2f; // Sensibilidade do movimento do mouse

    private bool isGameReady = false; // Indica se o jogo está pronto para processar movimentos do mouse

    // Construtor da classe Player
    public Player(Level level) {
        this.level = level;
        ResetPos(); // Define a posição inicial do jogador
    }

    // Método chamado quando o jogo é carregado
    public void OnLoad(GameWindow window) {
        window.CursorState = CursorState.Grabbed; // Captura o cursor do mouse para dentro da janela
        
        // Marca o jogo como pronto para processar movimentos do mouse
        isGameReady = true;
    }

    // Método chamado a cada frame para atualizar a lógica do jogador
    public void OnUpdateFrame(GameWindow window) {
        Time(); // Atualiza o tempo decorrido
        ProcessInput(window.KeyboardState); // Processa a entrada do teclado
        MouseCallBack(window.MouseState); // Processa o movimento do mouse
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
        postion.X = x;
        postion.Y = y;
        postion.Z = z;
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

        float x = 0.0f; // Movimento no eixo X
        float y = 0.0f; // Movimento no eixo Y
        float z = 0.0f; // Movimento no eixo Z

        // Verifica se a tecla R foi pressionada para redefinir a posição do jogador
        if(keyboardState.IsKeyDown(Keys.R)) {
            ResetPos();
        }

        // Verifica as teclas pressionadas e define a direção do movimento
        if(keyboardState.IsKeyDown(Keys.W)) {
            z++; // Move para frente
        }
        if(keyboardState.IsKeyDown(Keys.S)) {
            z--; // Move para trás
        }
        if(keyboardState.IsKeyDown(Keys.A)) {
            x--; // Move para a esquerda
        }
        if(keyboardState.IsKeyDown(Keys.D)) {
            x++; // Move para a direita
        }
        
        if(keyboardState.IsKeyDown(Keys.Space)) {
            y++; // Move para cima
        }
        if(keyboardState.IsKeyDown(Keys.LeftShift)) {
            y--; // Move para baixo
        }

        // Atualiza a posição do jogador com base na direção do movimento
        postion += x * speed * Vector3.Normalize(Vector3.Cross(direction, vertical)); // Movimento horizontal
        postion += y * speed * vertical; // Movimento vertical
        postion += z * speed * Vector3.Normalize(new Vector3(direction.X, 0.0f, direction.Z)); // Movimento para frente/trás
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

    // Método para criar uma matriz de visualização (LookAt)
    public Matrix4 LookAt() {
        // Define o ponto de observação (olho) como a posição do jogador
        Vector3 eye = postion;

        // Define o ponto de destino (alvo) como a posição do jogador mais a direção
        Vector3 target = postion + direction;

        // Define o vetor "up" como o vetor vertical
        Vector3 up = vertical;

        // Retorna a matriz de visualização (LookAt) que define como a cena é vista
        return Matrix4.LookAt(eye, target, up);
    }

    // Método para criar uma matriz de projeção em perspectiva
    public Matrix4 CreatePerspectiveFieldOfView(Vector2i clientSize) {
        // Define o campo de visão vertical (fovy) em radianos
        float fovy = MathHelper.DegreesToRadians(70.0f);

        // Define a proporção da tela (aspect ratio) como largura dividida pela altura
        float aspect = (float)clientSize.X / (float)clientSize.Y;

        // Define a distância do plano de corte próximo (depthNear)
        float depthNear = 0.05f;

        // Define a distância do plano de corte distante (depthFar)
        float depthFar = 1000.0f;

        // Retorna a matriz de projeção em perspectiva
        return Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar);
    }
}
