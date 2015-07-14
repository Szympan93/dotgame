using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class Hearts : MonoBehaviour
{
    public GameObject heart;
    public float heartSize;
    public int maxHearts;

    public UnityEvent OnLoseAllLife;

    [HideInInspector]
    public List<GameObject> hearts = new List<GameObject>();

    private RectTransform rectTransform;
    private int _life;

    public int life
    {
        set
        {
            _life = Mathf.Clamp(value, 0, maxHearts);
            changeHeartsState();
            if (_life == 0 && OnLoseAllLife != null)
            {
                OnLoseAllLife.Invoke();
            }
        }
        get
        {
            return _life;
        }
    }

    public void Start()
    {
        OnLoseAllLife.AddListener(() => 
        { 
            GameController.instance.OnDie.Invoke(); 
        });
        rectTransform = GetComponent<RectTransform>();
        life = maxHearts;
    }

    private void changeHeartsState()
    {
        for(int i = 0; i < hearts.Count; i++)
        {
            hearts[i].GetComponent<Animator>().SetBool("active", i < life);
        }
    }

#if UNITY_EDITOR
	void Update ()
    {
        if(Application.isPlaying)
        {
            return;
        }
        rectTransform.offsetMin = new Vector2(-maxHearts * heartSize, -heartSize);
        rectTransform.offsetMax = new Vector2(0, 0);

        if(heart != null)
        {
            foreach(GameObject go in hearts)
            {
                DestroyImmediate(go);
            }
            hearts.Clear();

            for(int i = 0; i < maxHearts; i++)
            {
                RectTransform rt = (Instantiate(heart) as GameObject).GetComponent<RectTransform>();
                rt.SetParent(transform, false);

                rt.anchorMin = new Vector2(1f - (float)(i + 1) / maxHearts, 0);
                rt.anchorMax = new Vector2(1f - (float)i / maxHearts, 1);

                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;


                hearts.Add(rt.gameObject);
            }
        }
	}
#endif
}
