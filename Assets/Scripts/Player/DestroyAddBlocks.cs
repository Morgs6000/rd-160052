using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe responsável por adicionar e remover blocos em um mundo de jogo baseado em chunk
public class DestroyAddBlocks : MonoBehaviour {
    // Referência à câmera do jogo
    [SerializeField] private Transform cam;

    // Alcance máximo em que o raio lançado pelo mouse pode colidir com um objeto
    private float rangeHit = 5.0f;

    // Máscara de camada que define quais objetos o raio pode colidir
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private SelectedBlock selectedBlock;
    
    private void Start() {
        
    }

    private void Update() {
        // Chamada das funções que adicionam ou removem blocos
        DestroyBlocks();
        AddBlocks();
    }

    // Função responsável por remover um bloco quando o botão direito do mouse é pressionado
    private void DestroyBlocks() {
        // Verifica se o botão direito do mouse foi pressionado
        if(Input.GetMouseButtonDown(1)) {
            // Armazena informações sobre a colisão do raio
            RaycastHit hit;

            // Lança um raio a partir da câmera na direção em frente com o alcance especificad
            if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
                // Calcula a posição do ponto de colisão, levando em conta a normal da superfície colidida
                Vector3 pointPos = hit.point - hit.normal / 2;

                // Obtém o chunk onde a colisão ocorreu
                Chunk c = Chunk.GetChunk(new Vector3(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                ));

                // Altera o tipo de bloco no chunk para "air" (ar)
                c.SetBlock(pointPos, VoxelType.air);
            }
        }
    }

    // Função responsável por adicionar um bloco quando o botão esquerdo do mouse é pressionado
    private void AddBlocks() {
        // Verifica se o botão esquerdo do mouse foi pressionado
        if(Input.GetMouseButtonDown(0)) {
            // Armazena informações sobre a colisão do raio
            RaycastHit hit;

            // Lança um raio a partir da câmera na direção em frente com o alcance especificado
            if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
                // Calcula a posição do ponto de colisão, levando em conta a normal da superfície colidida
                Vector3 pointPos = hit.point + hit.normal / 2;

                // Verifica se a posição do novo bloco é válida, ou seja, se ele não está muito próximo do jogador ou da câmera
                bool isValidPosition = 
                    Vector3.Distance(this.transform.position, pointPos) > 1.0f &&
                    Vector3.Distance(cam.position, pointPos) > 1.0f;

                // Se a altura do novo bloco for maior que o tamanho do chunk, não adiciona o bloco
                if(this.transform.position.y - 1 > Chunk.ChunkSizeInVoxels.y) {
                    return;
                }
                // Caso contrário, adiciona o bloco se a posição for válida
                else if(isValidPosition) {
                    // Obtém o chunk onde a colisão ocorreu
                    Chunk c = Chunk.GetChunk(new Vector3(
                        Mathf.FloorToInt(pointPos.x),
                        Mathf.FloorToInt(pointPos.y),
                        Mathf.FloorToInt(pointPos.z)
                    ));

                    // Altera o tipo de bloco no chunk para "stone" (pedra)
                    c.SetBlock(pointPos, selectedBlock.GetCurrentItem());
                }                
            }
        }
    }
}
