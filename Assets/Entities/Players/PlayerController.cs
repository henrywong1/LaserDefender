using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;
    public float padding = 1f;
    public float projectilleSpeed;
    public float firingRate;
    public GameObject projectile;
    public float health = 2050f;
    float xMin;
    float xMax;

    public AudioClip fireSound;
	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftmost.x + padding;
        xMax = rightmost.x - padding;
    }
  void Fire(){
    //LASERS
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject beam = Instantiate(projectile,transform.position + offset, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectilleSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
  }
	// Update is called once per frame
	void Update () {
        // Controllers using up,down,left,right keys.
        // delta time use for speed based on frame rate.
    if (Input.GetKeyDown(KeyCode.Space)){
          InvokeRepeating("Fire", 0.0000001f, firingRate);
       }
    if (Input.GetKeyUp(KeyCode.Space)){
          CancelInvoke("Fire");
        }
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        //restricts player to game space.
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    }
    void Die() {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);

    }
    void OnTriggerEnter2D(Collider2D collider) {
        Projectile missle = collider.gameObject.GetComponent<Projectile>();
        if (missle){
          health -= missle.GetDamage();
          missle.Hit();
          if (health <= 0){
            Die();
          }
        }
    }

}
