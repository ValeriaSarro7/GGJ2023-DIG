using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{

   public GameObject credits;
   public void Update()

   {
      if (Input.GetKeyDown(KeyCode.Space))
      {  
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }

      if(!credits.activeSelf && Input.GetKeyDown(KeyCode.C))
      {
         Debug.Log("gerew");
         credits.SetActive(true);
      }
  
      else if(credits.activeInHierarchy && Input.GetKeyDown(KeyCode.C))
      {
         credits.SetActive(false);
      }
   }

}
