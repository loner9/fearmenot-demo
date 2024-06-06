using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private GameObject NotesPanel;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private Button[] buttons;
    GameController gameController;
    PlayerHealth playerHealth;
    [SerializeField] private Sprite[] imgs;
    [SerializeField] private string[] names;
    [SerializeField] private GameObject btnDrawerPanel;
    // [SerializeField] private GameObject btnBook;
    // [SerializeField] private GameObject btnCase;
    [SerializeField] private Sprite[] bookSprite;
    [SerializeField] private InputField[] inputs;
    int count = 1;

    public static PauseMenu Instance { get; private set; }

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerHealth = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();

    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Debug.Log(InventoryPanel.transform.childCount);
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
        buttons[2].GetComponent<Button>().Select();
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
        Option();
        Time.timeScale = 1f;
    }

    public void resumeDrawer()
    {
        btnDrawerPanel.SetActive(false);
        Time.timeScale = 1f;
        Invoke("resetCounter", 0.4f);
    }

    void resetCounter()
    {
        Drawer drawer = GameObject.Find("DrawerQ").GetComponent<Drawer>();
        drawer.trigger = 1;
    }

    public void GoToMainMenu()
    {
        PlayerPrefs.SetString("save", "yes");
        SceneManager.LoadSceneAsync(0);
    }

    public void Option()
    {
        OptionPanel.SetActive(true);
        NotesPanel.SetActive(false);
        InventoryPanel.SetActive(false);
    }

    public void Notes()
    {
        OptionPanel.SetActive(false);
        NotesPanel.SetActive(true);
        InventoryPanel.SetActive(false);
    }

    public void Inventory()
    {
        GameObject inventoryList = InventoryPanel.transform.GetChild(1).gameObject;
        OptionPanel.SetActive(false);
        NotesPanel.SetActive(false);
        InventoryPanel.SetActive(true);
        Debug.Log(inventoryList.transform.childCount);
        for (int i = 0; i < inventoryList.transform.childCount; i++)
        {
            GameObject child = inventoryList.transform.GetChild(i).gameObject;
            // Image img = child.transform.GetChild(2).GetComponent<Image>();
            // Debug.Log("nama : " + img.name);
            if (i == 0)
            {
                if (gameController.apelAmount > 0)
                {
                    setItems(child, imgs[i], names[i], gameController.apelAmount.ToString(), false);
                    Button btn = child.GetComponent<Button>();
                    btn.onClick.AddListener(delegate { btnApel(); });
                }
                else
                {
                    setItems(child, imgs[i], names[i], gameController.apelAmount.ToString(), true);
                }
            }
            else if (i == 1)
            {
                if (gameController.drinkAmount > 0)
                {
                    setItems(child, imgs[i], names[i], gameController.drinkAmount.ToString(), false);
                    Button btn = child.GetComponent<Button>();
                    btn.onClick.AddListener(delegate { btnDrink(); });
                }
                else
                {
                    setItems(child, imgs[i], names[i], gameController.drinkAmount.ToString(), true);
                }
            }
            else if (i == 2)
            {
                if (gameController.isLanternCollected)
                {
                    setItems(child, imgs[i], names[i], "", false);
                }
                else
                {
                    setItems(child, imgs[i], names[i], "", true);
                }
            }
            else if (i == 3)
            {
                if (gameController.isSunShardCollected)
                {
                    setItems(child, imgs[i], names[i], "", false);
                }
                else
                {
                    setItems(child, imgs[i], names[i], "", true);
                }
            }
            else if (i == 4)
            {
                if (gameController.isCompassCollected)
                {
                    setItems(child, imgs[i], names[i], "", false);
                }
                else
                {
                    setItems(child, imgs[i], names[i], "", true);
                }
            }
            else if (i == 5)
            {
                if (gameController.isLanternCollected)
                {
                    setItems(child, imgs[i], names[i], "", false);
                }
                else
                {
                    setItems(child, imgs[i], names[i], "", true);
                }
            }

        }

    }

    void setItems(GameObject items, Sprite image, string name, string count, bool isEmpty)
    {
        Debug.Log("is setItems");
        TextMeshProUGUI textCount = items.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textName = items.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        GameObject imgCon = items.transform.GetChild(2).gameObject;
        Image img = imgCon.GetComponent<Image>();

        if (!isEmpty)
        {
            Debug.Log("is setItems.!isEmpty");
            img.sprite = image;
            imgCon.SetActive(true);
            textCount.text = count;
            textName.text = name;
        }
        else
        {
            Debug.Log("is setItems.isEmpty");
            imgCon.SetActive(false);
            textCount.text = "";
            textName.text = "";
        }
    }

    public void btnApel()
    {
        playerHealth.heal(3);
        gameController.apelAmount--;
        Resume();
    }

    public void btnDrink()
    {
        playerHealth.heal(1);
        gameController.drinkAmount--;
        Resume();
    }

    public void showBtnDrawer()
    {
        btnDrawerPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void showCase()
    {
        GameObject bookBtn = btnDrawerPanel.transform.GetChild(1).gameObject;
        GameObject koperBtn = btnDrawerPanel.transform.GetChild(2).gameObject;
        GameObject koper = btnDrawerPanel.transform.GetChild(5).gameObject;
        bookBtn.SetActive(false);
        koperBtn.SetActive(false);
        koper.SetActive(true);
    }

    public void CaseAction()
    {
        GameObject koper = btnDrawerPanel.transform.GetChild(5).gameObject;
        Image light = koper.GetComponent<Image>();
        string one, two, three, four;
        one = inputs[0].text;
        two = inputs[1].text;
        three = inputs[2].text;
        four = inputs[3].text;

        if (one.Equals("3") && two.Equals("1") && three.Equals("2") && four.Equals("4"))
        {
            light.color = Color.green;
        }
        else
        {
            light.color = Color.red;
            foreach (InputField s in inputs)
            {
                s.text = "";
            }
        }
    }

    public void showBook()
    {
        GameObject bookBtn = btnDrawerPanel.transform.GetChild(1).gameObject;
        GameObject koperBtn = btnDrawerPanel.transform.GetChild(2).gameObject;
        GameObject buku = btnDrawerPanel.transform.GetChild(4).gameObject;
        bookBtn.SetActive(false);
        koperBtn.SetActive(false);
        buku.SetActive(true);
    }

    public void bookAction()
    {
        GameObject buku = btnDrawerPanel.transform.GetChild(4).gameObject;
        Image img = buku.GetComponent<Image>();
        if (bookSprite.Length == count)
        {
            count = 0;
            img.sprite = bookSprite[count];
        }
        else
        {
            img.sprite = bookSprite[count];
        }
        count++;
    }

    void saveData()
    {

    }
}
