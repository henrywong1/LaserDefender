using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
			public float health = 200;
			public GameObject projectile;
			public float projectileSpeed = 10f;
			public float shotsPerSecond = 0.5f;
			public int scoreValue = 150;
			public AudioClip fireSound;
			public AudioClip deathSound;
			private ScoreKeeper scoreKeeper;
			void Start() {
					scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
			}
			void Update(){
				  float probability = shotsPerSecond * Time.deltaTime;
					if (Random.value < probability) {
							Fire();
						}
			}
			void Fire(){
				Vector3 startPosition = transform.position + new Vector3(0, -1, 0);
				GameObject missle = Instantiate(projectile,startPosition, Quaternion.identity) as GameObject;
				missle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
				AudioSource.PlayClipAtPoint(fireSound, transform.position);
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
			void Die() {
					AudioSource.PlayClipAtPoint(deathSound, transform.position);
					scoreKeeper.Score(scoreValue);
					Destroy(gameObject);
			}

}
