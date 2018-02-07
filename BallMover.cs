using UnityEngine;
using System.Collections;

public class BallMover : MonoBehaviour {

    private bool moveRight;
    private float speed = 1000.0f;

	// Use this for initialization
	void Start () {
        moveRight = true;
	}
	
	// Update is called once per frame
	void Update () {

        if(moveRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if(transform.position.x > 866)
        {
            moveRight = false;
        }
        if(transform.position.x < 150)
        {
            moveRight = true;
        }
	    
	}
}
