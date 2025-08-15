namespace RubyDung;

public class Chunk {
    // Referência ao nível (Level) ao qual este chunk pertence
    public readonly Level level;

    // Coordenadas mínimas do chunk (início do bloco)
    public readonly int x0;
    public readonly int y0;
    public readonly int z0;

    // Coordenadas máximas do chunk (fim do bloco)
    public readonly int x1;
    public readonly int y1;
    public readonly int z1;

    // Instância da classe Tesselator para gerenciar a renderização de geometria
    private Tesselator t;

    // Construtor da classe Chunk
    public Chunk(Shader shader, Level level, int x0, int y0, int z0, int x1, int y1, int z1) {
        // Inicializa o Tesselator com o shader fornecido
        t = new Tesselator(shader);

        // Armazena a referência ao nível
        this.level = level;

        // Define as coordenadas mínimas e máximas do chunk
        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;

        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
    }

    // Método chamado para carregar os dados do chunk
    public void OnLoad() {
        t.Init();

        // Itera sobre todas as coordenadas (x, y, z) dentro do chunk
        for(int x = x0; x < x1; x++) {
            for(int y = y0; y < y1; y++) {
                for(int z = z0; z < z1; z++) {
                    int tileId = level.GetTile(x, y, z);

                    if(tileId > 0) {
                        Tile.tiles[tileId].OnLoad(t, level, x, y, z);
                    }
                }
            }
        }

        // Configura os buffers e atributos de vértices na GPU
        t.OnLoad();
    }

    // Método chamado para renderizar o chunk
    public void OnRenderFrame() {
        // Renderiza a geometria do chunk usando o Tesselator
        t.OnRenderFrame();
    }
}
