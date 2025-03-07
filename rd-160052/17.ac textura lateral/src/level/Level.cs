namespace RubyDung;

public class Level {
    // Dimensões do nível (largura, altura e profundidade)
    public readonly int width;  // Largura do nível
    public readonly int height; // Altura do nível
    public readonly int depth;  // Profundidade do nível

    // Array que armazena os blocos do nível
    private byte[] blocks;

    // Array que armazena as profundidades de luz para cada coluna (x, z)
    private int[] lightDepths;

    // Construtor da classe Level
    public Level(int w, int h, int d) {
        // Inicializa as dimensões do nível
        width = w;
        height = h;
        depth = d;

        // Cria um array para armazenar os blocos, com tamanho igual a width * height * depth
        blocks = new byte[w * h * d];

        // Cria um array para armazenar as profundidades de luz, com tamanho igual a width * depth
        lightDepths = new int[w * d];

        int[] heightmap1 = (new PerlinNoiseFilter(0)).Read(w, d);
        int[] heightmap2 = (new PerlinNoiseFilter(0)).Read(w, d);
        int[] cf = (new PerlinNoiseFilter(1)).Read(w, d);
        int[] rockMap = (new PerlinNoiseFilter(1)).Read(w, d);

        // Preenche o array de blocos com o valor 1 (representando blocos sólidos)
        for(int x = 0; x < w; x++) {
            for(int y = 0; y < h; y++) {
                for(int z = 0; z < d; z++) {
                    int dh1 = heightmap1[x + z * width];
                    int dh2 = heightmap2[x + z * width];
                    int cfh = cf[x + z * width];
                    if(cfh < 128) {
                        dh2 = dh1;
                    }

                    int dh = dh1;
                    if(dh2 > dh) {
                        dh = dh2;
                    }

                    dh = dh / 8 + h / 3;
                    int rh = rockMap[x + z * width] / 8 + h / 3;
                    if(rh > dh - 2) {
                        rh = dh - 2;
                    }

                    // Calcula o índice no array para a posição (x, y, z)
                    int i = (x + y * width) * depth + z;
                    int id = 0;

                    if(y == dh) {
                        id = Tile.grass.id;
                    }
                    if(y < dh) {
                        id = Tile.dirt.id;
                    }
                    if(y <= rh) {
                        id = Tile.rock.id;
                    }

                    blocks[i] = (byte)id;
                }
            }
        }
    }

    public int GetTile(int x, int y, int z) {
        return x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth ? blocks[(y * depth + z) * width + x] : 0;
    }

    // Método para verificar se o bloco na posição (x, y, z) é sólido
    public bool IsSolidTile(int x, int y, int z) {
        Tile tile = Tile.tiles[GetTile(x, y, z)];
        return tile == null ? false : tile.IsSolid();
    }

    // Método para obter o brilho (brightness) em uma posição (x, y, z)
    public float GetBrightness(int x, int y, int z) {
        float dark = 0.8f; // Valor de brilho para áreas escuras
        float light = 1.0f; // Valor de brilho para áreas claras

        // Verifica se as coordenadas estão dentro dos limites do nível
        if(x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth) {
            // Se a posição y estiver abaixo da profundidade de luz, retorna o valor escuro
            // Caso contrário, retorna o valor claro
            return y < lightDepths[x + z * width] ? dark : light;
        }
        else {
            // Se as coordenadas estiverem fora dos limites, retorna o valor claro
            return light;
        }
    }
}
