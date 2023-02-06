using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CLastEvent : MonoBehaviour
{
    public Animator _anim;
    public float _enemesDefeated;
    public float _enemiesToSpawn;
    public GameObject _enemy;
    public EnemyChaser _enemySpawned;
    public List<EnemyChaser> _enemylist;
    public Transform[] _spawnPosition;
    public BoxCollider2D _box;
    public Camera _camera1;
    public Camera _camera2;

    // Start is called before the first frame update
    void Awake()
    {
        _box = GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D (Collider2D col)
    {
        CPlayer player = col.gameObject.GetComponentInParent<CPlayer>();
        if(player != null)
        {
            StartCoroutine(LastEvent());
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _enemylist.Count; i++)
        {
            if(_enemylist.Count >= 1)
            {
                if(_enemylist[i].Die())
                {
                    _enemesDefeated++;
                    _enemylist.Remove(_enemylist[i]);
                }
            }
        }
    }

    public IEnumerator LastEvent()
    {
        while (_enemiesToSpawn <= 9)
        {
            int _spawnPoint = Random.Range(0,2);
            _enemySpawned = Instantiate(_enemy, _spawnPosition[_spawnPoint].position, Quaternion.identity).GetComponent<EnemyChaser>();
            _enemylist.Add(_enemySpawned);
            _enemiesToSpawn++;
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }

        while (_enemesDefeated <= 9)
        yield return null;

        CPlayer.inst._inputEnabled = false;

        _camera1.enabled = false;
        _camera2.enabled = true;


        _anim.Play("WinMovement");
        
        yield return new WaitForSeconds(5f);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
    }
}
