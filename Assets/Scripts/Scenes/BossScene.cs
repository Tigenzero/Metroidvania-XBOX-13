using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    public GameObject heroine;
    public GameObject boss;
    public GameObject oldMan;
    // Update is called once per frame
    void Start()
    {
        // boss.GetComponent<Animator>().SetTrigger("attack");
        attackHero();
    }

    void attackHero() {
        boss.GetComponent<Animator>().SetTrigger("attack");
        heroine.GetComponent<Animator>().SetTrigger("attackUp");
    }
}
