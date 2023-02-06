using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneVision : MonoBehaviour
{
    public bool enemyMustChase;
    public bool playerIsInTheNextLvl;
    public EnemyChaser StopChansingPlayer;
    public CPlayer playerHP;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        CPlayer player = collider.gameObject.GetComponentInParent<CPlayer>();
        if(player != null)
        {
            enemyMustChase = true;
            StopChansingPlayer.ChansingPlayer(true);
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void isInTheZone()
    {

    }
}
