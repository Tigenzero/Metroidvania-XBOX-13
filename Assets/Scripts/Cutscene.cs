using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Let's see this");
            //collision.GetComponent<PlayerHealthController>().GetComponent<HeroinePlayerController>().Canmove(false);
            PlayerHealthController.instance.GetComponent<HeroinePlayerController>().Canmove(false);
            Dialouge.instance.gameObject.SetActive(true);
            Dialouge.instance.StartDialog();



                /* var A = 
                var H = gameObject.GetComponent<HeroinePlayerController>();
                H.GetComponent<HeroinePlayerController>();
                H.canMove = false;
                */

                Debug.Log("cutscene activated");
            PlayerHealthController.instance.GetComponent<HeroinePlayerController>().Canmove(true);
        }
    }
}
