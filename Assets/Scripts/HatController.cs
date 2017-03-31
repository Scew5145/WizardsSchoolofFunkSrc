using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour {

    public float speed;
    public GameObject fireball;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") == 1)
        {
            GameObject newBall = Instantiate(fireball, transform.position, transform.rotation);
            float randomizer = Random.value*1000;
            newBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, randomizer));
            Destroy(newBall, 1);
        }
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
	}
}
