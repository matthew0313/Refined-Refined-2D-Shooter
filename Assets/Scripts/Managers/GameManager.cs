using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        moneyText.text = money + "$";
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().gameover += GameOver;
    }
    [SerializeField] int m_money;
    [SerializeField] Text moneyText, moneyAddText;
    [SerializeField] float blinkTime = 2.0f;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Animator gameOverAnim;
    public int money { get { return m_money; } }
    IEnumerator blinking = null;
    float blinkCounter = 0.0f;
    public void EarnMoney(int amount)
    {
        if (amount <= 0) return; 
        m_money += amount;
        moneyText.text = money + "$";
        moneyAddText.color = Color.green;
        moneyAddText.text = "+" + amount + "$";
        blinkCounter = blinkTime;
        if(blinking == null)
        {
            blinking = Blinking();
            StartCoroutine(blinking);
        }
    }
    public void UseMoney(int amount)
    {
        if (amount <= 0) return;
        m_money -= amount;
        moneyText.text = money + "$";
        moneyAddText.color = Color.red;
        moneyAddText.text = "-" + amount + "$";
        blinkCounter = blinkTime;
        if (blinking == null)
        {
            blinking = Blinking();
            StartCoroutine(blinking);
        }
    }
    IEnumerator Blinking()
    {
        moneyAddText.gameObject.SetActive(true);
        while(blinkCounter > 0.0f)
        {
            blinkCounter -= Time.deltaTime;
            yield return null;
        }
        moneyAddText.gameObject.SetActive(false);
        blinking = null;
    }
    public bool paused { get; private set; } = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }
    public void PauseToggle()
    {
        if (!paused)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
            paused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            paused = false;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToLobby()
    {
        Time.timeScale = 1.0f;
        SceneChanger.Instance.SwitchScene("Lobby");
    }
    public void GameOver()
    {
        gameOverAnim.Play("GameOver");
        Invoke("BackToLobby", 3.0f);
    }
}
