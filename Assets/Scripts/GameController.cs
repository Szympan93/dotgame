using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance;

	void Awake ()
    {
        instance = this;
	}

    public Color[] color;
    public Spawner spawner;
    public Text scoreText;
    public Hearts hearts;
    public int activeColors = 2;

    public UnityEvent OnAddColor;
    public UnityEvent OnDie;

    private int _score;

    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            scoreText.text = value.ToString();
        }
    }

    public void AddColor()
    {
        activeColors = Mathf.Min(activeColors + 1, color.Length);
        if(OnAddColor != null)
        {
            OnAddColor.Invoke();
        }
    }

    void Start()
    {
        OnDie.AddListener(() =>
            {
                Time.timeScale = 0;
            });
    }

    public void replay()
    {
        Time.timeScale = 1;
        Application.LoadLevel("game");
    }
}
