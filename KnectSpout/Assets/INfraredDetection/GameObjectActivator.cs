using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActivator : MonoBehaviour
{
    public GameObject[] gameObjects;
    //public DepthSourceManagerMy kinect;
    void Start()
    {
        // Deactivate all gameObjects initially
        DeactivateAllGameObjects();
    }

    void Update()
    {
        // Check for input
        for (int i = 0; i < gameObjects.Length; i++)
        {
            // Use the numeric keypad keys (e.g., "1", "2", "3", etc.) to activate/deactivate the corresponding gameObjects in the array
            if (Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                ToggleGameObject(i);
            }
        }
      
    }

    void ToggleGameObject(int index)
    {
        // Check if the index is within the array bounds
        if (index >= 0 && index < gameObjects.Length)
        {
            // Activate the gameObject if it's currently deactivated, or deactivate it if it's currently activated
            gameObjects[index].SetActive(!gameObjects[index].activeSelf);
        }
    }

    void DeactivateAllGameObjects()
    {
        // Deactivate all gameObjects in the array
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }
}
