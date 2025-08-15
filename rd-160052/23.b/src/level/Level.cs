using System.IO.Compression;

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

        // Preenche o array de blocos com o valor 1 (representando blocos sólidos)
        for(int x = 0; x < w; x++) {
            for(int y = 0; y < h; y++) {
                for(int z = 0; z < d; z++) {
                    // Calcula o índice no array para a posição (x, y, z)
                    int i = (x + y * width) * depth + z;
                    // Define o bloco como sólido (valor 1) se estiver abaixo de 2/3 da altura do nível
                    blocks[i] = (byte)(y <= h * 2 / 3 ? 1 : 0);
                }
            }
        }

        Load();
    }

    public void Load() {
        try {
            BinaryReader dis = new BinaryReader(new GZipStream(new FileStream("level.dat", FileMode.Open), CompressionMode.Decompress));
            dis.Read(blocks, 0, blocks.Length);
            dis.Close();
        }
        catch(Exception e) {
            Console.WriteLine(e.StackTrace);
        }
    }

    public void Save() {
        try {
            BinaryWriter dos = new BinaryWriter(new GZipStream(new FileStream("level.dat", FileMode.Create), CompressionMode.Compress));
            dos.Write(blocks);
            dos.Close();
        }
        catch(Exception e) {
            Console.WriteLine(e.StackTrace);
        }
    }

    // Método para verificar se há um bloco na posição (x, y, z)
    public bool IsTile(int x, int y, int z) {
        // Verifica se as coordenadas estão dentro dos limites do nível
        if(x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth) {
            // Calcula o índice no array e verifica se o bloco existe (valor 1)
            return blocks[(x + y * width) * depth + z] == 1;
        }
        else {
            // Se as coordenadas estiverem fora dos limites, retorna falso
            return false;
        }
    }

    // Método para verificar se o bloco na posição (x, y, z) é sólido
    public bool IsSolidTile(int x, int y, int z) {
        // Por enquanto, este método é idêntico ao IsTile, mas pode ser expandido no futuro
        return IsTile(x, y, z);
    }

    public List<AABB> GetCubes(AABB aabb) {
        List<AABB> AABBs = new List<AABB>();

        int x0 = (int)aabb.x0;
        int x1 = (int)(aabb.x1 + 1.0f);
        int y0 = (int)aabb.y0;
        int y1 = (int)(aabb.y1 + 1.0f);
        int z0 = (int)aabb.z0;
        int z1 = (int)(aabb.z1 + 1.0f);

        if (x0 < 0) {
            x0 = 0;
        }

        if (y0 < 0) {
            y0 = 0;
        }

        if (z0 < 0) {
            z0 = 0;
        }

        if (x1 > width) {
            x1 = width;
        }

        if (y1 > height) {
            y1 = height;
        }

        if (z1 > depth) {
            z1 = depth;
        }

        for(int x = x0; x < x1; ++x) {
            for(int y = y0; y < y1; ++y) {
                for(int z = z0; z < z1; ++z) {
                    if (IsSolidTile(x, y, z)) {
                        AABBs.Add(new AABB((float)x, (float)y, (float)z, (float)(x + 1), (float)(y + 1), (float)(z + 1)));
                    }
                }
            }
        }

        return AABBs;
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

    public void SetTile(int x, int y, int z, int type) {
        if(x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth) {
            blocks[(x + y * width) * depth + z] = (byte)type;            
        }
    }
}
