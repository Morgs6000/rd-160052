using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    // Referência para o transform do personagem
    [SerializeField] private Transform player;

    // Rotação atual da câmera em relação ao eixo x
    private float xRotation = 0;
    
    private void Start() {
        // Tranca o cursor do mouse na tela do jogo
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        // Atualiza a câmera de acordo com os inputs do mouse
        CameraUpdates();

        // Tranca o cursor do mouse na tela do jogo
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Atualiza a câmera de acordo com os inputs do mouse
    private void CameraUpdates() {
        // Lê os inputs do mouse para os eixos horizontal e vertical
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotaciona o personagem em relação ao eixo y usando o método Rotate da classe Transform
        player.Rotate(Vector3.up * mouseX);

        // Atualiza a rotação da câmera em relação ao eixo x
        xRotation -= mouseY;
        // Limita a rotação da câmera entre -90 e 90 graus
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Aplica a rotação atual da câmera ao transform da câmera usando o método Euler da classe Quaternion
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
