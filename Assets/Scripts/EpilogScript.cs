using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class EpilogScript : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    public Image backgroundComponent;
    public Sprite[] background;
    public Image textBox;
    public TextMeshProUGUI textComponent;
    public String[] lines;
    public float textSpeed;
    private int index;
    public AudioSource[] audioSources;


    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
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
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        audioSources[0].Play();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            FadeOut();
            index++;

            if (lines[index] == "")
            {
                textBox.gameObject.SetActive(false);
            }
            else
            {
                textBox.gameObject.SetActive(true);
            }

            backgroundComponent.sprite = background[index];
            FadeIn();

            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());

        }
        else
        {
            audioSources[0].Stop();
            gameObject.SetActive(false);
            Invoke("toNextScene", 0.2f);
        }
    }

    void toNextScene()
    {
        PlayerPrefs.DeleteAll();
        SceneController.instance.toLevel(0);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeImage(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeImage(1f, 0f));
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color imageColor = backgroundComponent.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(endAlpha); // Ensure the final alpha is set
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = backgroundComponent.color;
        color.a = alpha;
        backgroundComponent.color = color;
    }
}
