namespace RubyDung;

public class Tile {
    public static readonly Tile[] tiles = new Tile[256];
    
    // Instancia estatica da classe Tile para ser acessada globalmente
    public static readonly Tile rock = new Tile(1, 1);
    public static readonly Tile grass = new GrassTile(2);
    public static readonly Tile dirt = new Tile(3, 2);
    public static readonly Tile stoneBrick = new Tile(4, 16);
    public static readonly Tile wood = new Tile(5, 4);

    // Índice da textura do bloco no atlas de texturas
    public int tex;
    public readonly int id;

    protected Tile(int id) {
        tiles[id] = this;
        this.id = id;
    }

    protected Tile(int id, int tex) : this(id) {
        this.tex = tex;
    }

    // Metodo chamado para carregar os dados do tile (bloco) no Tesselator
    public void OnLoad(Tesselator t, Level level, int x, int y, int z) {
        // Fatores de brilho para cada eixo (x, y, z)
        float cx = 0.6f; // Fator de brilho para o eixo X
        float cy = 1.0f; // Fator de brilho para o eixo Y
        float cz = 0.8f; // Fator de brilho para o eixo Z

        // Face x0 (lado esquerdo do cubo)
        if(!level.IsSolidTile(x - 1, y, z)) {
            t.Color(cx, cx, cx);
            RenderFace(t, x, y, z, 0);
        }

        // Face x1 (lado direito do cubo)
        if(!level.IsSolidTile(x + 1, y, z)) {
            t.Color(cx, cx, cx);
            RenderFace(t, x, y, z, 1);
        }

        // Face y0 (base do cubo)
        if(!level.IsSolidTile(x, y - 1, z)) {
            t.Color(cy, cy, cy);
            RenderFace(t, x, y, z, 2);
        }

        // Face y1 (topo do cubo)
        if(!level.IsSolidTile(x, y + 1, z)) {
            t.Color(cy, cy, cy);
            RenderFace(t, x, y, z, 3);
        }

        // Face z0 (frente do cubo)
        if(!level.IsSolidTile(x, y, z - 1)) {
            t.Color(cz, cz, cz);
            RenderFace(t, x, y, z, 4);
        }

        // Face z1 (tras do cubo)
        if(!level.IsSolidTile(x, y, z + 1)) {
            t.Color(cz, cz, cz);
            RenderFace(t, x, y, z, 5);
        }
    }

    protected virtual int GetTexture(int face) {
        return tex;
    }

    public void RenderFace(Tesselator t, int x, int y, int z, int face) {
        // Define as coordenadas minimas e maximas do cubo (bloco)
        float x0 = (float)x + 0.0f; // Coordenada x minima
        float y0 = (float)y + 0.0f; // Coordenada y minima
        float z0 = (float)z + 0.0f; // Coordenada z minima

        float x1 = (float)x + 1.0f; // Coordenada x maxima
        float y1 = (float)y + 1.0f; // Coordenada y maxima
        float z1 = (float)z + 1.0f; // Coordenada z maxima

        int tex = GetTexture(face);

        // Define as coordenadas de textura iniciais (u0, v0) e finais (u1, v1)
        // u0 e v0 representam o canto inferior esquerdo da textura no atlas
        float u0 = (float)(tex % 16) / 16.0f; // Coordenada u inicial
        float v0 = (16.0f - 1.0f - tex / 16) / 16.0f; // Coordenada v inicial
        
        // u1 e v1 representam o canto superior direito da textura no atlas
        float u1 = u0 + (1.0f / 16.0f); // Coordenada u final
        float v1 = v0 + (1.0f / 16.0f); // Coordenada v final


        float br; // Variável para armazenar o brilho calculado

        // Face x0 (lado esquerdo do cubo)
        if(face == 0) {
            // Define as coordenadas de textura e vértices para a face esquerda
            t.Tex(u0, v0); // Define a coordenada de textura
            t.Vertex(x0, y0, z0); // Define o vertice 0
            t.Tex(u1, v0);
            t.Vertex(x0, y0, z1); // Define o vertice 1
            t.Tex(u1, v1);
            t.Vertex(x0, y1, z1); // Define o vertice 2
            t.Tex(u0, v1);
            t.Vertex(x0, y1, z0); // Define o vertice 3            
        }

        // Face x1 (lado direito do cubo)
        if(face == 1) {
            // Define as coordenadas de textura e vértices para a face direita
            t.Tex(u0, v0);
            t.Vertex(x1, y0, z1); // Define o vertice 4
            t.Tex(u1, v0);
            t.Vertex(x1, y0, z0); // Define o vertice 5
            t.Tex(u1, v1);
            t.Vertex(x1, y1, z0); // Define o vertice 6
            t.Tex(u0, v1);
            t.Vertex(x1, y1, z1); // Define o vertice 7
        }

        // Face y0 (base do cubo)
        if(face == 2) {
            // Define as coordenadas de textura e vértices para a face inferior
            t.Tex(u0, v0);
            t.Vertex(x0, y0, z0); // Define o vertice 8
            t.Tex(u1, v0);
            t.Vertex(x1, y0, z0); // Define o vertice 9
            t.Tex(u1, v1);
            t.Vertex(x1, y0, z1); // Define o vertice 10
            t.Tex(u0, v1);
            t.Vertex(x0, y0, z1); // Define o vertice 11
        }

        // Face y1 (topo do cubo)
        if(face == 3) {
            // Define as coordenadas de textura e vértices para a face superior
            t.Tex(u0, v0);
            t.Vertex(x0, y1, z1); // Define o vertice 12
            t.Tex(u1, v0);
            t.Vertex(x1, y1, z1); // Define o vertice 13
            t.Tex(u1, v1);
            t.Vertex(x1, y1, z0); // Define o vertice 14
            t.Tex(u0, v1);
            t.Vertex(x0, y1, z0); // Define o vertice 15
        }

        // Face z0 (frente do cubo)
        if(face == 4) {
            // Define as coordenadas de textura e vértices para a face frontal
            t.Tex(u0, v0);
            t.Vertex(x1, y0, z0); // Define o vertice 16
            t.Tex(u1, v0);
            t.Vertex(x0, y0, z0); // Define o vertice 17
            t.Tex(u1, v1);
            t.Vertex(x0, y1, z0); // Define o vertice 18
            t.Tex(u0, v1);
            t.Vertex(x1, y1, z0); // Define o vertice 19
        }

        // Face z1 (tras do cubo)
        if(face == 5) {
            // Define as coordenadas de textura e vértices para a face traseira
            t.Tex(u0, v0);
            t.Vertex(x0, y0, z1); // Define o vertice 20
            t.Tex(u1, v0);
            t.Vertex(x1, y0, z1); // Define o vertice 21
            t.Tex(u1, v1);
            t.Vertex(x1, y1, z1); // Define o vertice 22
            t.Tex(u0, v1);
            t.Vertex(x0, y1, z1); // Define o vertice 23
        }
    }

    public void RenderFaceNoTexture(Tesselator t, int x, int y, int z, int face) {
        // Define as coordenadas minimas e maximas do cubo (bloco)
        float x0 = (float)x + 0.0f; // Coordenada x minima
        float y0 = (float)y + 0.0f; // Coordenada y minima
        float z0 = (float)z + 0.0f; // Coordenada z minima

        float x1 = (float)x + 1.0f; // Coordenada x maxima
        float y1 = (float)y + 1.0f; // Coordenada y maxima
        float z1 = (float)z + 1.0f; // Coordenada z maxima

        // Face x0 (lado esquerdo do cubo)
        if(face == 0) {
            t.Vertex(x0, y0, z0); // Define o vertice 0
            t.Vertex(x0, y0, z1); // Define o vertice 1
            t.Vertex(x0, y1, z1); // Define o vertice 2
            t.Vertex(x0, y1, z0); // Define o vertice 3
        }

        // Face x1 (lado direito do cubo)
        if(face == 1) {
            t.Vertex(x1, y0, z1); // Define o vertice 4
            t.Vertex(x1, y0, z0); // Define o vertice 5
            t.Vertex(x1, y1, z0); // Define o vertice 6
            t.Vertex(x1, y1, z1); // Define o vertice 7
        }

        // Face y0 (base do cubo)
        if(face == 2) {
            t.Vertex(x0, y0, z0); // Define o vertice 8
            t.Vertex(x1, y0, z0); // Define o vertice 9
            t.Vertex(x1, y0, z1); // Define o vertice 10
            t.Vertex(x0, y0, z1); // Define o vertice 11
        }

        // Face y1 (topo do cubo)
        if(face == 3) {
            t.Vertex(x0, y1, z1); // Define o vertice 12
            t.Vertex(x1, y1, z1); // Define o vertice 13
            t.Vertex(x1, y1, z0); // Define o vertice 14
            t.Vertex(x0, y1, z0); // Define o vertice 15
        }

        // Face z0 (frente do cubo)
        if(face == 4) {
            t.Vertex(x1, y0, z0); // Define o vertice 16
            t.Vertex(x0, y0, z0); // Define o vertice 17
            t.Vertex(x0, y1, z0); // Define o vertice 18
            t.Vertex(x1, y1, z0); // Define o vertice 19
        }

        // Face z1 (tras do cubo)
        if(face == 5) {
            t.Vertex(x0, y0, z1); // Define o vertice 20
            t.Vertex(x1, y0, z1); // Define o vertice 21
            t.Vertex(x1, y1, z1); // Define o vertice 22
            t.Vertex(x0, y1, z1); // Define o vertice 23
        }
    }

    public bool IsSolid() {
        return true;
    }
}
