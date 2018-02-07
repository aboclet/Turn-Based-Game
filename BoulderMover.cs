using UnityEngine;
using System.Collections;

public class BoulderMover : MonoBehaviour {

    private float speed = 400.0f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((Vector2.right + Vector2.down) * speed * Time.deltaTime);

        if (transform.position.x > 660.0f)
            Destroy(this.gameObject);
    }
}
