using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInstance : MonoBehaviour
{

    public static BossInstance instance;

    // when first activated, or reactivated
    // occurs before start function
    // Keeps gameObject consistent between scenes.
    private void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
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
}
