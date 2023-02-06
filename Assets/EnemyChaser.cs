using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public Transform playerTransform;
    public bool isChaseing;
    public float chaseDistance;
    public float moveSpeed;
    public Rigidbody2D rb;
    public BoxCollider2D _box;
    private Animator _anim;
    public int damage;
    public CPlayer playerHP;
    public CSpawnZone _spawnZone;
    public Transform pointToNextLevel;
    public bool _isDigging;
    public ParticleSystem _hitParticle;
    public int maxHealth = 3;
    private int currentHealth;
    public bool isKnockedBack;
    public int knockbackPower = 10;


    void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        playerHP = CPlayer.inst;
        playerTransform = playerHP.transform;
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        CPlayer player = collider.gameObject.GetComponentInParent<CPlayer>();
        if(player != null)
        {
            player.ReceiveDamage(damage);
			Instantiate(_hitParticle, collider.ClosestPoint(transform.position), Quaternion.identity);
        }

        CSpawnZone spawnZone = collider.gameObject.GetComponent<CSpawnZone>();
        {
            if(spawnZone != null)
            {
                _spawnZone = spawnZone;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CharacterScale = transform.localScale;
        
        if(isChaseing)
        {
            if(transform.position.x > playerTransform.position.x) 
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                 CharacterScale.x = 1f;
            }
            if(transform.position.x < playerTransform.position.x) 
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                 CharacterScale.x = -1f;
            }
              transform.localScale = CharacterScale;
        }
        else 
        {
            if(_spawnZone.playerIsInTheNextLvl)
            {
               pointToNextLevel = _spawnZone.holePoint;
             }
           if(transform.position.x > pointToNextLevel.position.x) 
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            if(transform.position.x < pointToNextLevel.position.x) 
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
              transform.localScale = CharacterScale;

              if(Vector2.Distance(transform.position, pointToNextLevel.position) <= 1f && _spawnZone.playerIsInTheNextLvl && !_isDigging)
              {
                StartCoroutine(Dig());
              }
        }
    }
    void FixedUpdate()
    {
        Vector3 direction = playerHP.transform.position - transform.position;

        if (isKnockedBack)
        {
            rb.AddForce(-direction * knockbackPower, ForceMode2D.Impulse);
            isKnockedBack = false;
        }  
    }
     public IEnumerator Dig()
    {
        _isDigging = true;
        //Instantiate(_groundParticle, _particlePosition.transform.position, Quaternion.identity); 
        rb.gravityScale = 0;
        _box.enabled = false;
        
        yield return new WaitForSeconds(1f);

        transform.position = pointToNextLevel.position + new Vector3(0, -1.3f, 0);
        //Instantiate(_secondGroundParticle, _exitingParticlePosition.transform.position, Quaternion.identity);
        yield return new WaitForSeconds (0.4f);
        _box.enabled = true;
        rb.gravityScale = 1;
        _isDigging = false;
        
        yield return null;
    }
    public void ChansingPlayer(bool enemyMustChase)
    {
        isChaseing = enemyMustChase;
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        isKnockedBack = true;
        _anim.SetTrigger("IsHit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool Die()
    {
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
            return true;
        }
        
        return false;
    }
   
}
