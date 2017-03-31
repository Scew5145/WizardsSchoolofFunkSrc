using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HatController : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public GameObject fireball;
    public AudioMixerSnapshot fullVolume1;
	private Rigidbody2D rb;
    private int comboCount = 0;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriggerLand"))
        {
            fullVolume1.TransitionTo(1);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		if (Input.GetAxis("Fire1") == 1)
		{
			GameObject newBall = Instantiate(fireball, transform.position, transform.rotation);
			float randomizer = Random.value*1000;
            comboCount += 1;
			newBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, randomizer));
			Destroy(newBall, 1);
		}
        if (comboCount >= 100)
        {

        }

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		if (grounded && Input.GetAxis ("Jump") == 1)
		{
			rb.AddForce (new Vector2(0, jumpForce));
		}
		rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
	}
}