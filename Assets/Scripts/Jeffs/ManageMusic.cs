using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMusic : MonoBehaviour
{
    // Start is called before the first frame update
private void Awake() 
    {
        if (FindObjectsOfType<ManageMusic>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
