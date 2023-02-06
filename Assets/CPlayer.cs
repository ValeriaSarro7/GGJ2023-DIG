using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CPlayer : MonoBehaviour
{
    private Rigidbody2D _rigi;
    private BoxCollider2D _box;
    private Animator _anim;
    public LayerMask groundLayer;
    public float _speed;
    public float _gravity;
    public bool _isDigging;
    public bool _isGrounded;
    public bool _inputEnabled;
    public bool _canDigDown;
    public GameObject _particlePosition;
    public GameObject _exitingParticlePosition;
    public ParticleSystem _groundParticle;
    public ParticleSystem _hitParticle;
    public ParticleSystem _secondGroundParticle;
    public HealthBar healthBar;
    public static CPlayer inst;
    public int maxHealth;
    private int currentHealth;
    public AudioSource _playerAudio;
    public AudioClip _diggingSound;
    public AudioClip _hitSound;
    public AudioClip _attackSound;
    public Canvas _canvas;
    public bool _isDead;
#region AttackParametres

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;

#endregion
    void Awake()
    {
        inst = this;
        _rigi = GetComponent<Rigidbody2D>();
        _box = GetComponentInChildren<BoxCollider2D>();
        _anim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _inputEnabled = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _isDead = IsDead();
    }

    // Update is called once per frame
    void Update()
    {

        if(IsDead())
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        _isGrounded = _rigi.IsTouchingLayers(groundLayer);

        if(Input.GetKeyDown(KeyCode.DownArrow) && _canDigDown == true && !_isDigging)
        {
            StartCoroutine(Dig());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack(); 
        }

        if(_isDigging != true && _isGrounded != true)
        {
            _rigi.AddForce(new Vector2(0, -_gravity), ForceMode2D.Force);
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (!_inputEnabled)
        return;

        float inputX = Input.GetAxis ("Horizontal");
        _anim.SetBool("IsRunning", inputX != 0.0f);

        if(inputX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _anim.SetBool("IsRunning", true);
        }
        else if (inputX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _anim.SetBool("IsRunning", true);

        }
        _rigi.MovePosition(transform.position + (new Vector3(inputX, 0, 0)) * _speed * Time.fixedDeltaTime); 
    }
    public IEnumerator Dig()
    {
        _playerAudio.clip = _diggingSound;
        _playerAudio.Play();
        _box.isTrigger = true;
        _isDigging = true;
        _canDigDown = false;
        _anim.Play("Digging", 0, 0);
        Instantiate(_groundParticle, _particlePosition.transform.position, Quaternion.identity); 
        _inputEnabled = false;
        _rigi.gravityScale = 0;
        
        yield return new WaitForSeconds(1f);

        transform.position = transform.position + new Vector3(0, -1.4f, 0);
        Instantiate(_secondGroundParticle, _exitingParticlePosition.transform.position, Quaternion.identity);

        _rigi.gravityScale = 1;
        _isDigging = false;
        _inputEnabled = true;
        _box.isTrigger = false;
        _anim.Play("Idle", 0, 0);
    }
    void Attack()
    {
        if(!_inputEnabled)
        return;
        // Play attack animation
        _anim.Play("Attack", 0, 0);
        _playerAudio.clip = _attackSound;
        _playerAudio.Play();
        
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage then 
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyChaser>().ReceiveDamage(attackDamage);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        CGround ground = col.gameObject.GetComponent<CGround>();

        if(!ground)
        return;

        _canDigDown = true;
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        CGround ground = col.gameObject.GetComponent<CGround>();

        if(!ground)
        return;

        _canDigDown = false;
    }

    public bool IsDead()
    {
        if(currentHealth <= 0)
        {
            return true;
        }
        return false;
    }
    public void ReceiveDamage(int damage)
    {
        if(_isDigging)
        return;
        if(IsDead())
        {
            _inputEnabled = false;
            _canvas.enabled = true;
        }
        currentHealth -= damage;
        _playerAudio.clip = _hitSound;
        _playerAudio.Play();

    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
