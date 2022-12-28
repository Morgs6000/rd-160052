using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
    private void Start() {
        
    }

    private void Update() {
        // Se o jogador apertar Esc, fechar o jogo.
        if(Input.GetButtonDown("Escape")) {
            Application.Quit();
        }
    }
}
