using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float spawnDelay = 0.5f;
    private bool movingRight = true;
    public float speed = 5f;
    private float xMax;
    private float xMin;
	// Use this for initialization
	void Start () {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceToCamera));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distanceToCamera));
        xMax = rightEdge.x;
        xMin = leftEdge.x;
        SpawnUntilFull();
	}
  public void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(width,height));
  }
	// Update is called once per frame
	void Update () {
        if (movingRight){
          transform.position += Vector3.right * speed * Time.deltaTime;
        } else {
          transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);
        if (leftEdgeOfFormation < xMin){
            movingRight = true;
        } else if ( rightEdgeOfFormation > xMax) {
            movingRight = false;
        }

        if (AllMembersDead()){
            Debug.Log("Empty Formation");
            SpawnUntilFull();
        }
  }
  void SpawnEnemies(){
      foreach ( Transform child in transform)
      {
          GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
          enemy.transform.parent = child;
      }
  }

  void SpawnUntilFull(){
      Transform freePosition = NextFreePosition();
      if (freePosition){
          GameObject enemy = Instantiate(enemyPrefab,freePosition.position, Quaternion.identity) as GameObject;
          enemy.transform.parent = freePosition;
      }
      if (NextFreePosition()){
          Invoke ("SpawnUntilFull", spawnDelay); //a delay for spawning,if theres a free position.
      }
  }
  Transform NextFreePosition(){
    foreach (Transform childPositionGameObject in transform){
          if (childPositionGameObject.childCount == 0){
              return childPositionGameObject;
          }
      }
    return null;
  }
  bool AllMembersDead(){
    //for loop, looping each childposition.
      foreach (Transform childPositionGameObject in transform){
          if (childPositionGameObject.childCount > 0){
              return false;
          }
      }
      return true;
  }

}
