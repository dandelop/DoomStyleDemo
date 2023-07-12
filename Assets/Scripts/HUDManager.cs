using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    //[SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI gameOverText;
    [SerializeField] private Slider healthSlider;
    
    // data model for the HUD
    private int _score;
    private int _playerHealth;

    private void Awake()
    {
        // reference in game manager
        GameManager.Instance.HUDManager = this;
    }

    // getters and setters for the data model
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }

    public int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = value;
            healthSlider.value = GameManager.Instance.PlayerHealth;
        }
    }

    void Start()
    {
        healthSlider.value = GameManager.Instance.PlayerHealth;
        // initialize the data model
        Score = 0;
        // hide the game over text
        gameOverText.gameObject.SetActive(false);
    }
    
    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
