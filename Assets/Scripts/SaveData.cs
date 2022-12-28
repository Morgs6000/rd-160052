using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour {
    private void Start() {
        Load();
    }

    private void Update() {
        if(Input.GetButtonDown("Enter")) {
            Save();

            Debug.Log("Pressionou Enter");
        }

        Save();
    }

    private void Save() {
        PlayerPrefs.SetFloat("playerPostionX", transform.position.x);
        PlayerPrefs.SetFloat("playerPostionY", transform.position.y);
        PlayerPrefs.SetFloat("playerPostionZ", transform.position.z);

        float playerRotation = transform.rotation.eulerAngles.y;
        PlayerPrefs.SetFloat("playerRotationY", playerRotation);
    }

    private void Load() {
        Vector3 position = new Vector3(
            PlayerPrefs.GetFloat("playerPostionX"),
            PlayerPrefs.GetFloat("playerPostionY"),
            PlayerPrefs.GetFloat("playerPostionZ")
        );

        transform.position = position;

        float playerRotation = PlayerPrefs.GetFloat("playerRotationY");
        transform.rotation = Quaternion.Euler(0.0f, playerRotation, 0.0f);
    }
}
