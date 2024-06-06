using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueScript : MonoBehaviour
{
    public Image backgroundComponent;
    public Sprite[] background;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private int cue;
    private int cue2;
    [SerializeField] private int sceneNum;
    bool cueReached;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        cueReached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                cue++;
                StopAllCoroutines();
                textComponent.text = lines[index];
                Debug.Log("this reached " + index);
            }
        }
        if (cue >= lines.Length)
        {
            if (!cueReached)
            {
                cueReached = true;
                Invoke("toNextScene", 0.2f);
            }
        }
    }

    void toNextScene()
    {
        SceneController.instance.nextLevel();
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        int step = lines[index].ToCharArray().Length;
        int count = 0;
        foreach (char c in lines[index].ToCharArray())
        {
            count++;
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
            if (step == count)
            {
                cue++;
            }

        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            backgroundComponent.sprite = background[index % 2 == 0 ? 0 : 1];
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
