using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get => _instance; 
    }

    private GameManager()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // reference to Player 
    private Player _player;
    public Player Player
    {
        get => _player;
        set => _player = value;
    }

    // lives of Player
    [SerializeField] private int lives = 3;
    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
            if (lives > 0) return;
            if (_hudManager != null)
            {
                _hudManager.ShowGameOver();
            }
            Time.timeScale = 0f;
        }
    }

    // Points of Player
    private int _score;

    private int Score
    {
        get => _score;
        set
        {
            _score = value; 
            if (_hudManager != null)
            {
                _hudManager.Score = _score;
            }
        }
    }
    
    public void AddPoints(int points)
    {
        Score += points;
    }
    
    // Health of Player
    private int _playerHealth;

    public int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = value; 
            if (_hudManager != null) 
            {
                _hudManager.PlayerHealth = _playerHealth;
            }
        }
    }

    public void UpdateHealth(int health)
    {
        PlayerHealth = health;
    }
    
    // reference to HUDManager
    private HUDManager _hudManager;
    public HUDManager HUDManager
    {
        set => _hudManager = value;
    }
}
