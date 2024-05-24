using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private Button[] buttons;

    public static PauseMenu Instance { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        // Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        // if (!isFirst)
        // {
        //     // buttons[0].interactable = true;
        //     buttons[1].interactable = true;
        // }
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        // if (!isFirst)
        // {
        //     buttons[0].interactable = true;
        // }
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu () 
    {
        SceneManager.LoadSceneAsync(0);
    }
}
