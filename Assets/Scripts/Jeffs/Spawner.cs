using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int spawnrate = 3;
    public GameObject projectile;
    
    // Start is called before the first frame update
    void Awake()
    {
        //get the Animator component from the trap;
        //start opening and closing the trap for demo purposes;
        StartCoroutine(Spawn());
    }


    public void SpawnProjectile()
    {
        GameObject b = Instantiate(projectile, transform.position, transform.rotation);
    }
    IEnumerator Spawn(){
        SpawnProjectile();

        yield return new WaitForSeconds(Random.Range(5,30));
        Awake();
    }
}
