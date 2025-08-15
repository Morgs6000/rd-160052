namespace RubyDung;

public class LevelRenderer {
    // Referência ao nível (Level) que será renderizado
    private Level level;

    // Array de chunks que compõem o nível
    private Chunk[] chunks;

    // Número de chunks em cada eixo (x, y, z)
    private int xChunks;
    private int yChunks;
    private int zChunks;

    // Construtor da classe LevelRenderer
    public LevelRenderer(Shader shader, Level level) {
        this.level = level;

        // Calcula o número de chunks em cada eixo com base nas dimensões do nível
        // Cada chunk tem tamanho fixo de 16x16x16 blocos
        xChunks = level.width / 16;
        yChunks = level.height / 16;
        zChunks = level.depth / 16;

        // Inicializa o array de chunks com o tamanho apropriado
        chunks = new Chunk[xChunks * yChunks * zChunks];

        // Itera sobre todas as coordenadas de chunks (x, y, z)
        for(int x = 0; x < xChunks; x++) {
            for(int y = 0; y < yChunks; y++) {
                for(int z = 0; z < zChunks; z++) {
                    // Calcula as coordenadas mínimas e máximas do chunk
                    int x0 = x * 16;
                    int y0 = y * 16;
                    int z0 = z * 16;
                    
                    int x1 = (x + 1) * 16;
                    int y1 = (y + 1) * 16;
                    int z1 = (z + 1) * 16;

                    // Cria um novo chunk e o armazena no array de chunks
                    chunks[(x + y * xChunks) * zChunks + z] = new Chunk(shader, level, x0, y0, z0, x1, y1, z1);   
                }
            }
        }   
    }

    // Método chamado para carregar todos os chunks
    public void OnLoad() {
        // Itera sobre todos os chunks e chama o método OnLoad de cada um
        for(int i = 0; i < chunks.Length; i++) {
            chunks[i].OnLoad();
        }
    }

    // Método chamado para renderizar todos os chunks
    public void OnRenderFrame() {
        // Itera sobre todos os chunks e chama o método OnRenderFrame de cada um
        for(int i = 0; i < chunks.Length; i++) {
            chunks[i].OnRenderFrame();
        }
    }

    public void SetChunk(int x0, int y0, int z0, int x1, int y1, int z1) {
        x0 /= 16;
        y0 /= 16;
        z0 /= 16;

        x1 /= 16;
        y1 /= 16;
        z1 /= 16;

        if(x0 < 0) {
            x0 = 0;
        }
        if(y0 < 0) {
            y0 = 0;
        }
        if(z0 < 0) {
            z0 = 0;
        }

        if(x1 >= xChunks) {
            x1 = xChunks - 1;
        }
        if(y1 >= yChunks) {
            y1 = yChunks - 1;
        }
        if(z1 >= zChunks) {
            z1 = zChunks - 1;
        }

        for(int x = x0; x <= x1; x++) {
            for(int y = y0; y <= y1; y++) {
                for(int z = z0; z <= z1; z++) {
                    chunks[(x + y * xChunks) * zChunks + z].OnLoad();
                }
            }
        }
    }
}
