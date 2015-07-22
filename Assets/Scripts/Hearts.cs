using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class Hearts : MonoBehaviour
{
    public GameObject heartPrefab;
    public float heartSize;
    public int maxHearts;

    public UnityEvent OnLoseAllLife;

    [HideInInspector]
    public List<GameObject> heart = new List<GameObject>();

    private RectTransform rectTransform;
    private int _life;

    public int life
    {
        set
        {
            _life = Mathf.Clamp(value, 0, maxHearts);
            updateHeartsState();
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

    private void updateHeartsState()
    {
        for(int i = 0; i < heart.Count; i++)
        {
            heart[i].GetComponent<Animator>().SetBool("active", i < life);
        }
    }

    //zbędne przy budowaniu
#if UNITY_EDITOR
	void Update ()
    {
        // nie twórz na nowo serc w czasie gry
        if(Application.isPlaying)
        {
            return;
        }
        rectTransform.offsetMin = new Vector2(-maxHearts * (1 - rectTransform.pivot.x), rectTransform.pivot.y - 1) * heartSize;
        rectTransform.offsetMax = new Vector2(maxHearts * rectTransform.pivot.x, rectTransform.pivot.y) * heartSize;

        if(heartPrefab != null)
        {
            foreach(GameObject go in heart)
            {
                DestroyImmediate(go);
            }
            heart.Clear();

            for(int i = 0; i < maxHearts; i++)
            {
                RectTransform rt = (Instantiate(heartPrefab) as GameObject).GetComponent<RectTransform>();
                rt.SetParent(transform, false);

                if(rectTransform.pivot.x < 0.5f)
                {
                    rt.anchorMin = new Vector2(1f - (float)(i + 1) / maxHearts, 0);
                    rt.anchorMax = new Vector2(1f - (float)i / maxHearts, 1);
                }else
                {
                    rt.anchorMin = new Vector2((float)i / maxHearts, 0);
                    rt.anchorMax = new Vector2((float)(i + 1) / maxHearts, 1);
                }

                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;


                heart.Add(rt.gameObject);
            }
        }
	}
#endif
}
