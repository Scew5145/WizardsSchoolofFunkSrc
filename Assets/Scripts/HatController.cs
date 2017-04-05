﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HatController : MonoBehaviour {

	public float bpm;

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

	private float timePerQ;
	private int qIter = 1;
	private int mIter = 1;
	private int[] spellArray = {0,0,0,0};
	private int spellIter = 0;

	// Use this for initialization
	void Start () {
		timePerQ = 60 / bpm;
		rb = GetComponent<Rigidbody2D>();

		InvokeRepeating ("QueueUp", 0f, timePerQ);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriggerLand"))
        {
            fullVolume1.TransitionTo(1);
        }
    }

    // Update is called once per frame
    void Update () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		if (Input.GetButtonDown("Fire1"))
		{
			if (spellIter < 4) {
				spellArray [spellIter] = 1;
				spellIter++;
			}
			/*
			GameObject newBall = Instantiate(fireball, transform.position, transform.rotation);
			float randomizer = Random.value*1000;
            comboCount += 1;
			newBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, randomizer));
			Destroy(newBall, 1);
			*/
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

	void QueueUp(){
		print ("beep"+ qIter);
		if (qIter % 4 == 1){
			for (int i = 0; i < spellArray.Length; i++) {
				if (spellArray [i] == 1) {
					GameObject newBall = Instantiate(fireball, transform.position, transform.rotation);
					float randomizer = Random.value*1000;
					comboCount += 1;
					newBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, randomizer));
					Destroy (newBall, 1);
					print ("fire!");
				}
				spellArray [i] = 0;
			}
			qIter = 1;
			spellIter = 0;
		}
		qIter++;
	}
}