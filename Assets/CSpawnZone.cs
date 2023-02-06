using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpawnZone : MonoBehaviour
{
    public GameObject _enemyToSpawn;
    public ParticleSystem _groundParticle;
    public GameObject _particlePosition; 
    private BoxCollider2D _box;
    public bool enemyMustChase;
    public bool playerIsInTheNextLvl;
    public bool _isSpawningEnemy;
    public EnemyChaser StopChansingPlayer;
    public CPlayer playerHP;
    public Transform holePoint;
    // Start is called before the first frame update
    void Awake()
    {
        _box = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        playerHP = CPlayer.inst;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy());
    }
    public IEnumerator SpawnEnemy() 
    {
        Instantiate(_groundParticle, _particlePosition.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.6f);

        StopChansingPlayer = Instantiate(_enemyToSpawn, transform.position, Quaternion.identity).GetComponent<EnemyChaser>();

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        CPlayer player = col.gameObject.GetComponentInParent<CPlayer>();

        if(player != null)
        {
            if(_isSpawningEnemy)
            {
                StartSpawn();    
            }
            //enemyMustChase = true;
            playerIsInTheNextLvl = false;

            StopChansingPlayer.ChansingPlayer(true);
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        EnemyChaser enemy = col.gameObject.GetComponentInParent<EnemyChaser>();

        if(enemy != null)
        {
            enemy._spawnZone = this.gameObject.GetComponent<CSpawnZone>(); 
            enemy.pointToNextLevel = holePoint;

        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        CPlayer player = col.gameObject.GetComponentInParent<CPlayer>();

        if(player != null)
        {
            //enemyMustChase = false;
            StopChansingPlayer.ChansingPlayer(false);
            playerIsInTheNextLvl = true;
        }
    }
}
