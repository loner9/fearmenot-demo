using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool isCutScene;
    [SerializeField] bool isMainMenu;
    Animator animator;
    [SerializeField] Canvas canvas;
    void Start()
    {
        animator = GetComponent<Animator>();
        // if (isMainMenu)
        // {
        //     animator.SetBool("mainMenu", true);
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void nextLevTransition()
    {
        animator.Play("fadeout");
    }

    public void initOder()
    {
        canvas.sortingOrder = 1;
    }

    public void afterOrder()
    {
        canvas.sortingOrder = -1;
    }

    public void resumeThings()
    {
        Time.timeScale = 1f;
    }

    public void pauseThings()
    {
        Time.timeScale = 0f;
    }
}
