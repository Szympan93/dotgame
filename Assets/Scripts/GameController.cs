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
    public int colors = 2;

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
        colors = Mathf.Min(colors + 1, color.Length);
        if(OnAddColor != null)
        {
            OnAddColor.Invoke();
        }
    }

    void Start()
    {
        OnDie.AddListener(() =>
            {
                Camera.main.cullingMask ^= 1<<LayerMask.NameToLayer("Dots");
                spawner.StopAllCoroutines();
                Invoke("toMenu", 2.5f);
            });
    }

    void toMenu()
    {
        Application.LoadLevel("menu");
    }
}
