using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffSet = 1f;
    public Transform target;


    // Update is called once per frame
    void Update()
    {
        Vector3 newPost = new Vector3(target.position.x,target.position.y + yOffSet, -10f);
        transform.position = Vector3.Slerp(transform.position,newPost,followSpeed * Time.deltaTime);
    }
}
