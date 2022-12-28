using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    // Malha do chunk usada para renderizar os blocos
    private Mesh voxelMesh;

    // Listas de vértices, triângulos e coordenadas de textura da malha
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    // Enum com as faces de um bloco
    private enum VoxelSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }

    // Índice do próximo vértice a ser adicionado à malha
    private int vertexIndex;

    // Tamanho do chunk em unidades de bloco
    public static Vector3 ChunkSizeInVoxels = new Vector3(16, 64, 16);

    // Dicionário que armazena as posições e os tipos de bloco presentes no chunk
    private VoxelType[,,] voxelMap = new VoxelType[(int)ChunkSizeInVoxels.x, (int)ChunkSizeInVoxels.y, (int)ChunkSizeInVoxels.z];

    // Tipo de bloco atual
    private VoxelType voxelType;

    // Lista de todos os chunks do mundo
    public static List<Chunk> chunkList = new List<Chunk>();

    private void Start() {        
        // Adiciona este chunk à lista de chunks
        chunkList.Add(this);

        // Gera os blocos do chunk
        ChunkGen();
    }

    private void Update() {
        // Atualizações do chunk podem ser feitas aqui (por exemplo, mudar a posição de um bloco)        
    }

    // Adiciona um bloco à voxel map e atualiza a malha do chunk
    public void SetBlock(Vector3 worldPos, VoxelType voxel) {
        // Calcula a posição local do bloco em relação ao chunk
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        // Adiciona o bloco à voxel map
        voxelMap[x, y, z] = voxel;

        // Atualiza a malha do chunk para refletir o novo bloco
        ChunkRenderer();
    }

    // Retorna o chunk que contém a posição dada, se houver
    public static Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < chunkList.Count; i++) {            
            // Calcula a posição do chunk atual
            Vector3 chunkPos = chunkList[i].transform.position;

            // Verifica se a posição dada está dentro dos limites do chunk
            if(
                pos.x < chunkPos.x || pos.x >= chunkPos.x + ChunkSizeInVoxels.x || 
                pos.y < chunkPos.y || pos.y >= chunkPos.y + ChunkSizeInVoxels.y || 
                pos.z < chunkPos.z || pos.z >= chunkPos.z + ChunkSizeInVoxels.z
            ) {
                // A posição não está neste chunk, passa para o próximo
                continue;
            }

            // A posição está neste chunk, retorna o chunk
            return chunkList[i];
        }

        // Nenhum chunk contém a posição dada
        return null;
    }
    
    // Gera as camadas de blocos do chunk de acordo com sua posição
    private VoxelType VoxelLayers(Vector3 offset) {
        // Adiciona um bloco de pedra abaixo da superfície
        if(offset.y < Noise.Perlin(offset.x, offset.z) - 4) {
            return VoxelType.stone;
        }
        // Adiciona a camada de terra
        else if(offset.y < Noise.Perlin(offset.x, offset.z)) {
            return VoxelType.dirt;
        }
        // Adiciona um bloco de grama na superfície
        else if(offset.y == Noise.Perlin(offset.x, offset.z)) {
            return VoxelType.grass_block;
        }
        // Adiciona um bloco de ar acima da superfície
        else {
            return VoxelType.air;
        }
    }

    // Gera todos os blocos do chunk
    private void ChunkGen() {
        for(int x = 0; x < ChunkSizeInVoxels.x; x++) {
            for(int y = 0; y < ChunkSizeInVoxels.y; y++) {
                for(int z = 0; z < ChunkSizeInVoxels.z; z++) {
                    voxelMap[x, y, z] = VoxelLayers(new Vector3(x, y, z) + transform.position);
                }
            }
        }

        // Atualiza a malha do chunk para refletir todos os blocos
        ChunkRenderer();
    }

    // Atualiza a malha do chunk para refletir a voxel map atual
    // É chamada sempre que um bloco é adicionado ou removido do chunk.
    private void ChunkRenderer() {
        // Cria uma nova malha
        voxelMesh = new Mesh();
        voxelMesh.name = "Chunk";

        // Limpa as listas de vértices, triângulos e coordenadas de textura
        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        // Reseta o índice de vértices
        vertexIndex = 0;

        // Percorre os voxels do chunk
        for(int x = 0; x < ChunkSizeInVoxels.x; x++) {
            for(int y = 0; y < ChunkSizeInVoxels.y; y++) {
                for(int z = 0; z < ChunkSizeInVoxels.z; z++) {
                    // Se o voxel atual não for ar, adiciona suas faces à malha
                    if(voxelMap[x, y, z] != VoxelType.air) {
                        BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        MeshRenderer();
    }

    private void MeshRenderer() {
        // Atribui as listas de vértices, triângulos e coordenadas de textura à malha
        voxelMesh.vertices = vertices.ToArray();
        voxelMesh.triangles = triangles.ToArray();
        voxelMesh.uv = uv.ToArray();

        voxelMesh.RecalculateNormals();
        voxelMesh.Optimize();

        GetComponent<MeshCollider>().sharedMesh = voxelMesh;
        GetComponent<MeshFilter>().mesh = voxelMesh;
    }

    private bool HasSolidNeighbor(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;
        
        if(
            x < 0 || x > ChunkSizeInVoxels.x - 1 ||
            y < 0 || y > ChunkSizeInVoxels.y - 1 ||
            z < 0 || z > ChunkSizeInVoxels.z - 1
        ) {
            return false;
        }
        else if(voxelMap[x, y, z] == VoxelType.air) {
            return false;
        }
        else {
            return true;
        }        
    }

    private void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;
        
        voxelType = voxelMap[x, y, z];

        if(!HasSolidNeighbor(new Vector3(1, 0, 0) + offset)) {
            VerticesAdd(VoxelSide.RIGHT, offset);
        }
        if(!HasSolidNeighbor(new Vector3(-1, 0, 0) + offset)) {
            VerticesAdd(VoxelSide.LEFT, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 1, 0) + offset)) {
            VerticesAdd(VoxelSide.TOP, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, -1, 0) + offset)) {
            VerticesAdd(VoxelSide.BOTTOM, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 0, 1) + offset)) {
            VerticesAdd(VoxelSide.FRONT, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 0, -1) + offset)) {
            VerticesAdd(VoxelSide.BACK, offset);
        }
    }

    private void VerticesAdd(VoxelSide side, Vector3 offset) {
        switch(side) {
            case VoxelSide.RIGHT: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);

                break;
            }
            case VoxelSide.LEFT: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case VoxelSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);

                break;
            }
            case VoxelSide.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case VoxelSide.FRONT: {
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);

                break;
            }
            case VoxelSide.BACK: {
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);

                break;
            }
        }

        TrianglesAdd();

        UVsPos(side);
    }

    private void TrianglesAdd() {
        // Primeiro Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(1 + vertexIndex);
        triangles.Add(2 + vertexIndex);

        // Segundo Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(2 + vertexIndex);
        triangles.Add(3 + vertexIndex);

        vertexIndex += 4;
    }

    private void UVsAdd(Vector2 textureCoordinate) {
        Vector2 offset = new Vector2(
            0, 
            0
        );

        Vector2 textureSizeInTiles = new Vector2(
            16 + offset.x,
            16 + offset.y
        );
        
        float x = textureCoordinate.x + offset.x;
        float y = textureCoordinate.y + offset.y;

        float _x = 1.0f / textureSizeInTiles.x;
        float _y = 1.0f / textureSizeInTiles.y;

        y = (textureSizeInTiles.y - 1) - y;

        x *= _x;
        y *= _y;

        uv.Add(new Vector2(x, y));
        uv.Add(new Vector2(x, y + _y));
        uv.Add(new Vector2(x + _x, y + _y));
        uv.Add(new Vector2(x + _x, y));
    }

    private void UVsPos(VoxelSide side) {
        // Pre-Classic | rd-132211
        
        // STONE
        if(voxelType == VoxelType.stone) {
            UVsAdd(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(voxelType == VoxelType.grass_block) {
            if(side == VoxelSide.TOP) {
                UVsAdd(new Vector2(0, 0));
                return;
            }
            if(side == VoxelSide.TOP) {
                UVsAdd(new Vector2(2, 0));
                return;
            }            
            UVsAdd(new Vector2(3, 0));
        }
        
        // DIRT
        if(voxelType == VoxelType.dirt) {
            UVsAdd(new Vector2(2, 0));
        }
        
        // COBBLESTONE
        if(voxelType == VoxelType.cobblestone) {
            UVsAdd(new Vector2(0, 1));
        }
        
        // OAK PLANKS
        if(voxelType == VoxelType.oak_planks) {
            UVsAdd(new Vector2(4, 0));
        }
    }
}
