using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    // Referência para o componente CharacterController do personagem
    [SerializeField] private CharacterController characterController;

    //private Vector3 moveDirection;

    // Velocidade atual do personagem
    private float speed;

    // Velocidade de movimento do personagem ao andar
    private float walkingSpeed = 4.317f;
    
    // Vetor de velocidade atual do personagem
    private Vector3 velocity;
    
    // Aceleração da gravidade
    private float fallSpeed = -78.4f;

    // Flag que indica se o personagem está no chão
    private bool isGrounded;

    // Transform usado para verificar se o personagem está no chão
    [SerializeField] private Transform groundCheck;
    // Distância mínima para considerar que o personagem está no chão
    private float groundDistance = 0.1f;
    // Máscara de camada usada para verificar se o personagem está no chão
    [SerializeField] private LayerMask groundMask;
    
    // Altura máxima do pulo do personagem
    private float jumpHeight = 1.2522f;

    // Offset para ajustar a posição do personagem em relação ao chão
    private float stepOffset = 1.0f;

    private void Start() {
        // Inicializa a velocidade do personagem como a velocidade de movimento ao andar
        speed = walkingSpeed;
    }

    private void Update() {
        // Atualiza o movimento do personagem
        MovementUpdate();
        // Atualiza a queda do personagem
        FallUpdate();
        // Atualiza o pulo do personagem
        JumpUpdate();
        
        // Respawna o personagem em uma posição aleatória quando o jogador pressiona R
        Respawn();

        // Atualiza o offset da posição do personagem em relação ao chão
        //StepOffsetUpdate();
    }

    // Atualiza o movimento do personagem de acordo com os inputs do jogador
    private void MovementUpdate() {
        // Lê o input do jogador para o eixo horizontal
        float x = Input.GetAxis("HorizontalAD");
        // Lê o input do jogador para o eixo vertical
        float z = Input.GetAxis("VerticalWS");

        // Calcula a direção de movimento do personagem com base nos inputs do jogador
        Vector3 moveDirection = transform.TransformDirection(new Vector3(x, 0.0f, z));

        // Aplica a velocidade atual do personagem à direção de movimento
        moveDirection *= speed;

        // Aplica o movimento do personagem usando o método Move da classe CharacterController
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Atualiza a queda do personagem
    private void FallUpdate() {
        // Verifica se o personagem está no chão usando o método CheckSphere da classe Physics
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Aplica a gravidade ao vetor de velocidade do personagem
        velocity.y += fallSpeed * Time.deltaTime;

        // Aplica a velocidade atual do personagem ao personagem usando o método Move da classe CharacterController
        characterController.Move(velocity * Time.deltaTime);

        // Se o personagem estiver no chão e o vetor de velocidade tiver componente y negativa, reseta o componente y do vetor de velocidade
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2.0f;
        }
    }

    // Atualiza o pulo do personagem
    private void JumpUpdate() {
        // Se o personagem estiver no chão e o jogador pressionar o botão de pulo...
        if(isGrounded && Input.GetButton("Space")) {
            // ...marca o personagem como não estando mais no chão...
            isGrounded = false;

            // ...e aplica uma força de pulo ao personagem usando o método Move da classe CharacterController
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * fallSpeed);
        }
    }

    // Respawna o personagem em uma posição aleatória
    private void Respawn() {
        /*
        float x = Random.Range(-((World.WorldSize.x * Chunk.ChunkSizeInVoxels.x) / 2), ((World.WorldSize.x * Chunk.ChunkSizeInVoxels.x) / 2));
        float z = Random.Range(-((World.WorldSize.z * Chunk.ChunkSizeInVoxels.z) / 2), ((World.WorldSize.z * Chunk.ChunkSizeInVoxels.z) / 2));
        */
        // Calcula uma posição aleatória para o respawn baseada na distância de visualização do mundo e no tamanho dos chunks
        float x = Random.Range(-((World.viewDistance * Chunk.ChunkSizeInVoxels.x) / 2), ((World.viewDistance * Chunk.ChunkSizeInVoxels.x) / 2));
        float z = Random.Range(-((World.viewDistance * Chunk.ChunkSizeInVoxels.z) / 2), ((World.viewDistance * Chunk.ChunkSizeInVoxels.z) / 2));
        
        // Cria um vetor de posição com a posição aleatória e uma altura de 64 unidades
        Vector3 respawn = new Vector3(x, 64.0f, z);

        // Se o jogador pressionar R...
        if(Input.GetKeyDown(KeyCode.R)) {
            // ...move o personagem para a posição de respawn usando o método Move da classe CharacterController
            characterController.Move(respawn);
        }
    }

    // Atualiza o offset da posição do personagem em relação ao chão
    private void StepOffsetUpdate() {
        // Dispara um raio para baixo a partir da posição atual do personagem
        RaycastHit hit;

        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, stepOffset)) {
            // Se o raio colidir com algum objeto, move o personagem para a posição de colisão mais o offset definido
            Vector3 newPosition = hit.point + Vector3.up * stepOffset;
            characterController.Move(newPosition - transform.position);
        }
        else {
            // Se o raio não colidir com nenhum objeto, mantém o movimento do personagem como está
            //characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
