using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


class SongChunk {
	private int bpm;
	private float length;
	private AudioClip[] files;
	private float[] beatmap;
	private float startTime;
	private float musicHitRadius = .0675f;
	private float musicNearHitRadius = .125f;

	public SongChunk(int _bpm, AudioClip[] _files, float[] _beatmap, AudioSource src) {
		//???
		files = _files;
		beatmap = _beatmap;
		bpm = _bpm;
		length = files[0].length;

		src.clip = files[0];
		startTime = Time.time;
		src.Play();
		}
	//For musichitradius stuff
	/*public SongChunk(int _bpm, string[] _files, float[] _beatmap, float _musicHitRadius, float _musicNearHitRadius) {
		//???
	}*/

	public float calcWeight(float currentTime){
		float offset = currentTime - startTime;
		int prevBeat = 0;
		for (int i = 0; i < beatmap.Length-1; i++) {
			if (offset <= beatmap[i]) {
				break;
			} else {
				prevBeat = i;
			}
		}
		float timeSincePrev = offset - beatmap [prevBeat];
		float timeTillNext = beatmap [prevBeat + 1] - offset;
		float theError;
		if (timeSincePrev <= timeTillNext) {
			theError = timeSincePrev;
		} else {
			theError = timeTillNext;
		}
		if (theError <= musicHitRadius) {
			return 1.0f;
		}
		else if (theError <= musicNearHitRadius){
			return (float)(1.0f - (theError - musicHitRadius)*(0.9/(musicNearHitRadius - musicHitRadius)));
		}
		else {
			return 0.1f;
		}
	}
	public float getLength() {
		return length;
	}
	public float getStartTime() {
		return startTime;
	}
		
}

public class HatController : MonoBehaviour {

	public float bpm;

	public float speed;
	public float jumpForce;
	public GameObject fireball;
    public AudioMixerSnapshot fullVolume1;

	//Song Chunk Stuff
	public AudioClip tier1TesterClip;
	public AudioClip tier2TesterClip;
	public AudioClip tier3TesterClip;
	public AudioSource tier1Source;

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
	private SongChunk songcTest;


	//Convention for beatmap 'notation'
	//Measure per line!
	private float[] beatmap = {
		0.0f,0.5f,1.0f,1.5f, 
		2.0f,2.5f,3.0f,3.5f,
		4.0f,4.5f,5.0f,5.5f,
		6.0f,6.5f,7.0f,7.5f,

		8.0f,8.5f,9.0f,9.5f, 
		10.0f,10.5f,11.0f,11.5f,
		12.0f,12.5f,13.0f,13.5f,
		14.0f,14.5f,15.0f,15.5f,

		16.0f,16.5f,17.0f,17.5f, 
		18.0f,18.5f,19.0f,19.5f,
		20.0f,20.5f,21.0f,21.5f,
		22.0f,22.5f,23.0f,23.5f,

		24.0f,24.5f,25.0f,25.5f, 
		26.0f,26.5f,27.0f,27.5f,
		28.0f,28.5f,29.0f,29.5f,
		30.0f,30.5f,31.0f,31.5f,
	};

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
			AudioClip[] fileArray = {tier1TesterClip, tier2TesterClip, tier3TesterClip};
			songcTest = new SongChunk (120, fileArray, beatmap, tier1Source);
        }
    }

    // Update is called once per frame
    void Update () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		if (Input.GetButtonDown("Fire1"))
		{
			if (songcTest != null) {
				print (songcTest.calcWeight (Time.time));
			} else {
				print ("songcTest is null.");
			}
			/*if (spellIter < 4) {
				spellArray [spellIter] = 1;
				spellIter++;
			}*/
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