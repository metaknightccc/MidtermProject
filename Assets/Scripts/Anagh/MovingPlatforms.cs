using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{

    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;
    private Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = pos2.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position == pos1.position) {
            nextPos = pos2.position;
        }

        if (transform.position == pos2.position) {
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

    
    
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) {
            print("running");
            return;
        }
           
        else
            print("test");
            other.transform.parent = transform;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player")) 
            return;
        other.transform.parent = null;
    }


    private void OnDrawGizmos() {
        Gizmos.DrawLine(pos1.position, pos2.position);
    } 
}
