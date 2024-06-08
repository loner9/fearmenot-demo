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
    [SerializeField] private Sprite koperBuka;
    [SerializeField] private Sprite koperBukaKosong;
    PlayerControl playerControl;
    GameObject canvas1;
    [SerializeField] GameObject notesContainer;
    [SerializeField] GameObject textNotes;
    [SerializeField] GameObject gmover;

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
        buttons[2].GetComponent<Button>().Select();
        Debug.Log("Test value 1 :" + PlayerPrefs.GetInt("test", 0));
        Debug.Log("Test value 2 :" + PlayerPrefs.GetInt("testicle"));

        Debug.Log(PlayerPrefs.GetString("scene", "null"));
        if (PlayerPrefs.GetString("scene", "").Equals("BossFight"))
        {
            canvas1 = GameObject.Find("Canvas1");
            if (canvas1 != null)
            {
                canvas1.SetActive(false);
            }
        }
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
        if (PlayerPrefs.GetString("scene").Equals("BossFight"))
        {
            // GameObject canvas1 = GameObject.Find("Canvas1");
            canvas1.SetActive(true);
        }
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

    public void gameOver()
    {
        gmover.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoToMainMenu()
    {
        PlayerPrefs.SetString("save", "yes");
        SceneManager.LoadScene(0);
    }

    public void deadToMenu()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
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
                    btn.onClick.AddListener(delegate { btnApel(child); });
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
                    btn.onClick.AddListener(delegate { btnDrink(child); });
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
                if (gameController.isMagnetCollected)
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
                if (gameController.isCompassCollected)
                {
                    if (gameController.isCompassNeedleCollected)
                    {
                        setItems(child, imgs[6], names[i], "", false);
                    }
                    else if (gameController.isCompassFixed)
                    {
                        setItems(child, imgs[7], names[i], "", false);
                    }
                    else
                    {
                        setItems(child, imgs[i], names[i], "", false);
                    }

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

    public void btnApel(GameObject items)
    {
        TextMeshProUGUI textCount = items.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textName = items.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        GameObject imgCon = items.transform.GetChild(2).gameObject;
        if (gameController.apelAmount != 0)
        {
            playerHealth.heal(3);
            gameController.apelAmount--;
            textCount.text = gameController.apelAmount + "";
            if (gameController.apelAmount == 0)
            {
                imgCon.SetActive(false);
                textCount.text = "";
                textName.text = "";
            }
        }
        // Resume();
    }

    public void btnMagnetDrawer()
    {
        gameController.isMagnetCollected = true;
        GameObject koper = btnDrawerPanel.transform.GetChild(5).gameObject;
        Image kprOpenBlank = koper.GetComponent<Image>();
        kprOpenBlank.sprite = koperBukaKosong;

        // Resume();
    }

    public void btnMagnet()
    {
        GameObject inventoryList = InventoryPanel.transform.GetChild(1).gameObject;
        if (gameController.isCompassCollected && gameController.isCompassNeedleCollected)
        {
            gameController.isCompassFixed = true;
        }
        else
        {
            GameObject text = InventoryPanel.transform.GetChild(2).gameObject;
            TextMeshProUGUI clue = text.GetComponent<TextMeshProUGUI>();
            clue.text = "Terdapat komponen kompas yang masih hilang";
        }
        // GameObject magnetGobj = inventoryList.transform.GetChild(4).gameObject;
        // Button compass = magnetGobj.GetComponent<Button>();
        // Resume();
    }

    public void btnDrink(GameObject items)
    {
        TextMeshProUGUI textCount = items.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textName = items.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        GameObject imgCon = items.transform.GetChild(2).gameObject;
        if (gameController.drinkAmount != 0)
        {
            playerHealth.heal(1);
            gameController.drinkAmount--;
            textCount.text = gameController.drinkAmount + "";
            if (gameController.drinkAmount == 0)
            {
                imgCon.SetActive(false);
                textCount.text = "";
                textName.text = "";
            }
        }
    }

    public void showBtnDrawer()
    {
        btnDrawerPanel.SetActive(true);
        GameObject bookBtn = btnDrawerPanel.transform.GetChild(1).gameObject;
        GameObject koperBtn = btnDrawerPanel.transform.GetChild(2).gameObject;
        GameObject koper = btnDrawerPanel.transform.GetChild(5).gameObject;
        GameObject buku = btnDrawerPanel.transform.GetChild(4).gameObject;
        bookBtn.SetActive(true);
        koperBtn.SetActive(true);
        koper.SetActive(false);
        buku.SetActive(false);
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
        Image light = koper.transform.GetChild(5).GetComponent<Image>();
        string one, two, three, four;
        one = inputs[0].text;
        two = inputs[1].text;
        three = inputs[2].text;
        four = inputs[3].text;

        if (gameController.isMagnetCollected)
        {
            for (int i = 0; i < koper.transform.childCount; i++)
            {
                GameObject child = koper.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }
            Image kprBlank = koper.GetComponent<Image>();
            kprBlank.sprite = koperBukaKosong;
        }

        if (one.Equals("3") && two.Equals("1") && three.Equals("2") && four.Equals("4"))
        {
            light.color = Color.green;
            for (int i = 0; i < koper.transform.childCount; i++)
            {
                GameObject child = koper.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }

            Image kprOpen = koper.GetComponent<Image>();
            kprOpen.sprite = koperBuka;
            Button btn = koper.GetComponent<Button>();
            btn.onClick.AddListener(delegate { btnMagnetDrawer(); });
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

    public void notesContent()
    {
        notesContainer.SetActive(true);
        textNotes.SetActive(false);
        for (int i = 0; i < notesContainer.transform.childCount; i++)
        {
            GameObject item = notesContainer.transform.GetChild(i).gameObject;
            item.SetActive(false);
        }

        for (int i = 0; i < gameController.notesName.Count; i++)
        {
            GameObject item = notesContainer.transform.GetChild(i).gameObject;
            item.SetActive(true);
            TextMeshProUGUI textName = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            textName.text = gameController.notesName[i];
            Button notes = item.GetComponent<Button>();
            string msg = gameController.notesMsg[i];
            notes.onClick.AddListener(delegate
            {
                btnRevealNotes(msg);
            });
        }
    }

    private void btnRevealNotes(string v)
    {
        notesContainer.SetActive(false);
        textNotes.SetActive(true);
        TextMeshProUGUI msg = textNotes.GetComponent<TextMeshProUGUI>();
        msg.text = v;
    }

    public void infoCompass()
    {
        GameObject text = InventoryPanel.transform.GetChild(2).gameObject;
        TextMeshProUGUI clue = text.GetComponent<TextMeshProUGUI>();
        if (gameController.isCompassCollected)
        {
            clue.text = "Terdapat komponen yang hilang dari kompas ini...";
            if (gameController.isCompassNeedleCollected)
            {
                clue.text = "Sepertinya kompas ini masih perlu diperbaiki";
                if (gameController.isCompassFixed)
                {
                    clue.text = "Kompas ini telah berfungsi!!!";
                }
            }

        }
    }

    public void infoMagnet()
    {
        GameObject text = InventoryPanel.transform.GetChild(2).gameObject;
        TextMeshProUGUI clue = text.GetComponent<TextMeshProUGUI>();
        if (gameController.isMagnetCollected)
        {
            clue.text = "Sepertinya benda ini dapat digunakan untuk memperbaiki kompas [Klik item untuk memperbaiki kompas]";
        }
    }

    public void resetText()
    {
        GameObject text = InventoryPanel.transform.GetChild(2).gameObject;
        TextMeshProUGUI clue = text.GetComponent<TextMeshProUGUI>();
        clue.text = "";
    }

    public void restart1()
    {
        gmover.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        playerControl = GameObject.Find("Char").GetComponent<PlayerControl>();
        playerControl.resetState();
        gameController.initialState();
        Time.timeScale = 1f;
    }

    public void restart2()
    {
        gmover.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        playerControl = GameObject.Find("Char").GetComponent<PlayerControl>();
        // playerControl.resetState();
        // gameController.accessStateB();
        Time.timeScale = 1f;
    }

    public void restart3()
    {
        gmover.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        BattleSystem battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        battleSystem.resetGame();
    }

    void saveData()
    {

    }
}
