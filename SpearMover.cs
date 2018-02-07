using UnityEngine;
using System.Collections;

public class SpearMover : MonoBehaviour {

    public int speed;
    private GameObject enemy;
    private Enemy enemyController;

	// Use this for initialization
	void Start () {

        enemy = GameObject.FindWithTag("Enemy");
        enemyController = enemy.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {

        if(transform.position.x > 250)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else if(transform.position.x < 250)
        {
            enemyController.CallDamageAnimation();
            Destroy(this.gameObject);
        }

        
    }
}
