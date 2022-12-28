using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    // Tamanho do mundo em blocos
    public static Vector3 WorldSizeInVoxels = new Vector3(
        256,
        64,
        256
    );
    
    // Tamanho do mundo em chunks
    public static Vector3 WorldSizeInChunks = new Vector3(
        WorldSizeInVoxels.x / Chunk.ChunkSizeInVoxels.x,
        WorldSizeInVoxels.y / Chunk.ChunkSizeInVoxels.y,
        WorldSizeInVoxels.z / Chunk.ChunkSizeInVoxels.z
    );
    
    // Dicionário de chunks
    private Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();

    // Transform do jogador
    [SerializeField] private Transform player;

    // Distância de visualização
    public static int viewDistance = 5;

    // Prefab de chunk
    [SerializeField] private GameObject chunkPrefab;

    // Lista de todos os chunks do mundo
    public static List<Chunk> chunkList = new List<Chunk>();

    // Inicializa a geração do mundo
    private void Start() {
        WorldGen();
    }

    // Atualiza a renderização do mundo
    private void Update() {
        StartCoroutine(WorldRenderer());
    }

    // Gera o mundo
    private void WorldGen() {
        // Itera pelas coordenadas de chunk
        for(int x = -((int)WorldSizeInChunks.x / 2); x < ((int)WorldSizeInChunks.x / 2); x++) {
            for(int y = 0; y < WorldSizeInChunks.y; y++) {
                for(int z = -((int)WorldSizeInChunks.z / 2); z < ((int)WorldSizeInChunks.z / 2); z++) {
                    // Calcula o deslocamento do chunk
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSizeInVoxels.x,
                        y * Chunk.ChunkSizeInVoxels.y,
                        z * Chunk.ChunkSizeInVoxels.z
                    );

                    // Adiciona o chunk ao dicionário
                    chunks.Add(chunkOffset, new Chunk());
                }
            }
        }
    }

    // Renderiza os chunks do mundo
    private IEnumerator WorldRenderer() {
        // Calcula a posição do jogador em chunks
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSizeInVoxels.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSizeInVoxels.z);

        // Itera pelas coordenadas de chunk dentro da distância de visualização
        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < WorldSizeInChunks.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    // Calcula o deslocamento do chunk
                    Vector3 chunkOffset = new Vector3(
                        (x + posX) * Chunk.ChunkSizeInVoxels.x,
                        y * Chunk.ChunkSizeInVoxels.y,
                        (z + posZ) * Chunk.ChunkSizeInVoxels.z
                    );

                    // Obtém o chunk no deslocamento especificado
                    Chunk c = Chunk.GetChunk(new Vector3(
                        Mathf.FloorToInt(chunkOffset.x),
                        Mathf.FloorToInt(chunkOffset.y),
                        Mathf.FloorToInt(chunkOffset.z)
                    ));
                    
                    // Verifica se o chunk já existe no dicionário
                    if(chunks.ContainsKey(chunkOffset)) {
                        // Verifica se o chunk ainda não foi instanciado no mundo
                        if(c == null) {
                            GameObject chunk = Instantiate(chunkPrefab);
                            chunk.transform.position = chunkOffset;
                            chunk.transform.SetParent(transform);
                            chunk.name = "Chunk: " + (x + posX) + ", " + (z + posZ);

                            //chunkList.Add(chunk.GetComponent<Chunk>());
                        }
                    }

                    // Pausa a rotina por um frame
                    yield return null;
                }
            }
        }
    }    

    /*
    // Retorna o chunk que contém a posição dada, se houver
    public static Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < chunkList.Count; i++) {            
            // Calcula a posição do chunk atual
            Vector3 chunkPos = chunkList[i].transform.position;

            // Verifica se a posição dada está dentro dos limites do chunk
            if(
                pos.x < chunkPos.x || pos.x >= chunkPos.x + Chunk.ChunkSizeInVoxels.x || 
                pos.y < chunkPos.y || pos.y >= chunkPos.y + Chunk.ChunkSizeInVoxels.y || 
                pos.z < chunkPos.z || pos.z >= chunkPos.z + Chunk.ChunkSizeInVoxels.z
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
    */
}
