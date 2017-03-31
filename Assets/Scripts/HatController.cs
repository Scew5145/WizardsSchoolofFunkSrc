using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public GameObject fireball;
	private Rigidbody2D rb;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		if (Input.GetAxis("Fire1") == 1)
		{
			GameObject newBall = Instantiate(fireball, transform.position, transform.rotation);
			float randomizer = Random.value*1000;
			newBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, randomizer));
			Destroy(newBall, 1);
		}

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		if (grounded && Input.GetAxis ("Jump") == 1)
		{
			rb.AddForce (new Vector2(0, jumpForce));
		}
		rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
	}
}
