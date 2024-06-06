using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private GameObject NotesPanel;
    [SerializeField] private GameObject InventoryPanel;
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
        buttons[2].GetComponent<Button>().Select();
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void InventoryShortcut()
    {
        buttons[4].GetComponent<Button>().Select();
        Inventory();
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu () 
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Option () 
    {
        OptionPanel.SetActive(true);
        NotesPanel.SetActive(false);
        InventoryPanel.SetActive(false);
    }

    public void Notes () 
    {
        OptionPanel.SetActive(false);
        NotesPanel.SetActive(true);
        InventoryPanel.SetActive(false);
    }

    public void Inventory () 
    {
        OptionPanel.SetActive(false);
        NotesPanel.SetActive(false);
        InventoryPanel.SetActive(true);
    }
}
