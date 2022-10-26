using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public GameObject[] playerPrefabs;
   void Start()
   {
       PlayerInput.Instantiate(playerPrefabs[0], pairWithDevice: Gamepad.all[0]);
       PlayerInput.Instantiate(playerPrefabs[1], pairWithDevice: Gamepad.all[1]);
       PlayerInput.Instantiate(playerPrefabs[2], pairWithDevice: Gamepad.all[2]);
       PlayerInput.Instantiate(playerPrefabs[3], pairWithDevice: Gamepad.all[3]);
   }
}