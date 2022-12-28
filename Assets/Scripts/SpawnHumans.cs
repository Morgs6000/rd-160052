using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHumans : MonoBehaviour {
    [SerializeField] GameObject humanPrefab;
    private GameObject human;
    
    // Velocidade de movimento do personagem ao andar
    private float walkingSpeed = 4.317f;
    
    private void Start() {
        Spawn();
    }

    private void Update() {
        //Walk();
    }

    private void Spawn() {
        // Gera 10 prefabs em posições aleatórias
        for (int i = 0; i < 10; i++)  {
            // Gera um valor aleatório para x e z entre -128 e 128
            int x = Random.Range(-128, 128);
            int z = Random.Range(-128, 128);

            // Cria o prefab na posição (x, 0, z)
            human = Instantiate(humanPrefab);
            human.transform.position = new Vector3(x, Noise.Perlin(x, z), z);
            human.transform.SetParent(transform);
            human.name = "Human: " + i;

            // Move o prefab aleatoriamente
            StartCoroutine(MoveRandomly(human.transform));
        }
    }

    /*
    private void Walk() {
        // Adiciona um componente Rigidbody ao prefab
        Rigidbody rb = human.AddComponent<Rigidbody>();

        // Aplica uma força aleatória ao prefab para movê-lo
        rb.AddForce(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f), ForceMode.Impulse);
    }
    */

    /*
    private void Walk() {
        // Adiciona um componente Character Controller ao prefab
        CharacterController cc = human.AddComponent<CharacterController>();

        // Define a velocidade de movimento do prefab
        //cc.movement.maxForwardSpeed = 5f;
        //cc.movement.maxSidewaysSpeed = 5f;
        //cc.movement.maxBackwardsSpeed = 5f;

        // Aplica uma força aleatória ao prefab para movê-lo
        cc.Move(new Vector3(Random.Range(-walkingSpeed, walkingSpeed), 0, Random.Range(-walkingSpeed, walkingSpeed)));
    }
    */

    IEnumerator MoveRandomly(Transform transform) {
        // Gera um vetor de movimento aleatório
        Vector3 moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        // Repete o movimento enquanto o prefab estiver ativo
        while (transform.gameObject.activeInHierarchy) {
            // Move o prefab na direção e velocidade especificadas
            transform.position += moveDirection * walkingSpeed * Time.deltaTime;

            // Espera um frame antes de continuar o loop
            yield return null;
        }
    }
}
