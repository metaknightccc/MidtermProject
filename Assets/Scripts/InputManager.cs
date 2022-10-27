using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public GameObject[] playerPrefabs;

   public InputActionAsset[] keyActions;

   void Start()
   {
        int playerCount = Mathf.Clamp(Gamepad.all.Count, 0, 4);
        Debug.Log(playerCount);

        if(playerCount < 1) {
            PlayerInput.Instantiate(playerPrefabs[0], pairWithDevice: Keyboard.current, controlScheme: "Keyboard&Mouse");
            PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Keyboard.current, controlScheme: "RightKeyboard");
        } else if (playerCount < 2) {
            PlayerInput.Instantiate(playerPrefabs[0], pairWithDevice: Keyboard.current, controlScheme: "Keyboard&Mouse");
            PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Keyboard.current, controlScheme: "RightKeyboard");
            PlayerInput.Instantiate(playerPrefabs[2], pairWithDevice: Gamepad.all[0]);
        } else if (playerCount < 3) {
            PlayerInput.Instantiate(playerPrefabs[0], pairWithDevice: Keyboard.current, controlScheme: "Keyboard&Mouse");
            PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Keyboard.current, controlScheme: "RightKeyboard");
            PlayerInput.Instantiate(playerPrefabs[2], pairWithDevice: Gamepad.all[0]);
            PlayerInput.Instantiate(playerPrefabs[3], pairWithDevice: Gamepad.all[1]);
        } else if (playerCount < 4) {
            PlayerInput.Instantiate(playerPrefabs[0]);
            PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Gamepad.all[0]);
            PlayerInput.Instantiate(playerPrefabs[2], pairWithDevice: Gamepad.all[1]);
            PlayerInput.Instantiate(playerPrefabs[3], pairWithDevice: Gamepad.all[2]);
        } else {
            PlayerInput.Instantiate(playerPrefabs[0], pairWithDevice: Gamepad.all[0]);
            PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Gamepad.all[1]);
            PlayerInput.Instantiate(playerPrefabs[2], pairWithDevice: Gamepad.all[2]);
            PlayerInput.Instantiate(playerPrefabs[3], pairWithDevice: Gamepad.all[3]);
        }
       //PlayerInput.Instantiate(playerPrefabs[0], pairWithDevice: Gamepad.all[0]);
       //PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Gamepad.all[1]);
       //PlayerInput.Instantiate(playerPrefabs[2], pairWithDevice: Gamepad.all[2]);
       //PlayerInput.Instantiate(playerPrefabs[3], pairWithDevice: Gamepad.all[3]);
   }
}