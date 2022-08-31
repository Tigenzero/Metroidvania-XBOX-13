using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    public GameObject heroine;
    public GameObject boss;
    public GameObject oldMan;

    public GameObject acc;

    public float accCounter;
    public float auraCounter;
    public float flashCounter;

    bool auraNotSet = true;
    bool flash = true;

    bool accdone = true;

    // Update is called once per frame
    void Start()
    {
        // boss.GetComponent<Animator>().SetTrigger("attack");
        
        attackHero();
        

    }

    private void Update() {
        
        
        startAura();
        startAcc();
        flashCount();
    }

    void attackHero() {
        boss.GetComponent<Animator>().SetTrigger("attack");
        heroine.GetComponent<Animator>().SetTrigger("attackUp");
    }

    void startAura(){
        if (auraCounter > 0 && auraNotSet) {
            auraCounter -= Time.deltaTime;
        } else {
            Debug.Log("Aura");
            heroine.GetComponent<Animator>().SetTrigger("aura");
            auraNotSet = false;
        }
    }

    void startAcc(){
        if (accCounter > 0) {
            accCounter -= Time.deltaTime;
        } else {
            acc.SetActive(true);
            accdone = false;
        }
    }

    

    void flashCount() {
        if (flashCounter > 0) {
            flashCounter -= Time.deltaTime;
        } else {
            heroine.SetActive(false);
            boss.SetActive(false);
            flash = false;

        }

    }



}
