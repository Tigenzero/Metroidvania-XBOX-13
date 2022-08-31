using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RushUncleConvo : MonoBehaviour

{
    [SerializeField]
    public GameObject Prime;

    [SerializeField]
    public GameObject Text;

    [SerializeField]
    public GameObject EvilText;

    //[SerializeField]
    //public GameObject Image;

   // [SerializeField]
    //public GameObject EImage;

    public static Dialouge instance;

    public TextMeshProUGUI textComponent;
    public string[] lines;

    public float textspeed;

    private int index;



    // Start is called before the first frame update
    public void StartDialog()
    {
        textComponent.text = string.Empty;
        Prime.SetActive(true);

        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }

    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            gameObject.SetActive(true);
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            Debug.Log(+index);

            if (index == 3|| index == 4 || index == 5|| index == 11)
            {
                Debug.Log(+index);
                Text.SetActive(true);
                EvilText.SetActive(false);
                //Image.SetActive(true);
                //EPanel.SetActive(false);
                //EImage.SetActive(false);
            }
            else if (index == 0|| index == 1 || index == 2 || index == 6 || index == 7|| index == 8 || index == 9 || index ==10 ||index ==12 || index == 13  ||index == 14 ||index == 15 || index == 16  )
            {
                Debug.Log(+index);
                Text.SetActive(false);
                EvilText.SetActive(true);
                //Image.SetActive(false);
                //EPanel.SetActive(true);
                //EImage.SetActive(true);

            }
        }


        else
        {
            gameObject.SetActive(false);
        }
    }
}

