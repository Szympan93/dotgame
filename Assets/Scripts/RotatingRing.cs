using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class RotatingRing : MonoBehaviour
{
    public GameObject ringPrefab;
    public float speed;

    private List<Image> ringParts = new List<Image>();
    private Coroutine rotateCoroutine;
    private float angle;

    public bool rotating{get;set;}

    private IEnumerator rotate()
    {
        while(true)
        {
            if (rotating)
            {
                angle += Time.deltaTime * speed;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
    }

    public void Start()
    {
        for (int i = 0; i < GameController.instance.activeColors; i++ )
        {
            addFragment();
        }

        StartCoroutine(rotate());
        GameController.instance.OnAddColor.AddListener(addFragment);
    }

    public float getDeltaAngle(Vector2 other)
    {
        Vector2 delta = other - (Vector2)transform.position;
        return Mathf.DeltaAngle( angle - 90, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(inColorRange(getDeltaAngle(other.transform.position), other.transform.parent.GetComponent<Dot>().color))
        {
            GameController.instance.score++;
        }else
        {
            GameController.instance.hearts.life--;
        }
        Destroy(other.transform.parent.gameObject);
    }

    private bool inColorRange(float angle, int color)
    {
        float min = 360f / GameController.instance.activeColors * color - 180;
        float max = 360f / GameController.instance.activeColors * (color + 1) - 180;
        return (angle >= min && angle < max);
    }

    public void addFragment()
    {
        if(GameController.instance.color.Length <= ringParts.Count)
            return;

        Image part = Instantiate(ringPrefab).GetComponent<Image>();
        part.transform.SetParent(transform, false);

        part.color = GameController.instance.color[ringParts.Count];
        ringParts.Add(part);
        for (int i = 0; i < ringParts.Count; i++)
        {
            ringParts[i].fillAmount = 1f / ringParts.Count;
            ringParts[i].transform.rotation = Quaternion.Euler(Vector3.forward * (angle + 360f * ringParts[i].fillAmount * i));
        }
    }
}
