using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScene : MonoBehaviour
{
    public GameObject robot;
    public int increment;
    private int brightness = 1;
    private bool triggered = false;

    private SpriteRenderer sprite;

    bool colored = false;

    float timeLeft = 0f;
    Color targetColor = new Color(0, 0 ,0 , 250);
    Color colorTargetColor = new Color(250, 250 ,250 , 250);

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("triggered");
        triggered = true;

        sprite = robot.GetComponent<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        Debug.Log(sprite.color);
        timeLeft = 1.0f;

    }

    private void Update()
    {

        // if (timeLeft <= Time.deltaTime)
        // {
        //     // transition complete
        //     // assign the target color
        //     renderer.material.color = targetColor;

        //     // start a new transition
        //     targetColor = new Color(Random.value, Random.value, Random.value);
        //     timeLeft = 1.0f;
        // }
        // else
        // {
        //     // transition in progress
        //     // calculate interpolated color
        //     renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime / timeLeft);

        //     // update the timer
        //     timeLeft -= Time.deltaTime;
        // }
        if (triggered && brightness < 255 && timeLeft > 0)
        {
            Debug.Log(sprite.color);
            // robot.SetActive(true);
            sprite.color = Color.Lerp(sprite.color, targetColor, Time.deltaTime / timeLeft);

            // update the timer
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                timeLeft = 0;
            }
        }

        if (triggered && timeLeft == 0) { 
            sprite.color = Color.Lerp(sprite.color, colorTargetColor, Time.deltaTime / timeLeft);

            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }
}
