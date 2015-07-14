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

	public bool isRotating
    {
        set
        {
            if(value)
            {
                if(rotateCoroutine == null)
                {
                    rotateCoroutine = StartCoroutine(rotate());
                }
            }else
            {
                if(rotateCoroutine != null)
                {
                    StopCoroutine(rotateCoroutine);
                    rotateCoroutine = null;
                }
            }
        }
    }

    private IEnumerator rotate()
    {
        while(true)
        {
            angle += Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
    }

    public void Start()
    {
        for (int i = 0; i < GameController.instance.colors; i++ )
        {
            Image img = Instantiate(ringPrefab).GetComponent<Image>();
            img.transform.SetParent(transform, false);

            img.color = GameController.instance.color[i];
            img.fillAmount = 1f / GameController.instance.colors;
            img.transform.rotation = Quaternion.Euler(Vector3.forward * 360f / GameController.instance.colors * i);

            ringParts.Add(img);
        }
        GameController.instance.OnAddColor.AddListener(addColor);
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
        float min = 360f / GameController.instance.colors * color;
        min -= 180;
        float max = 360f / GameController.instance.colors * (color + 1);
        max -= 180;
        return (angle >= min && angle < max);
    }

    public void addColor()
    {
        Quaternion old = transform.rotation;
        transform.rotation = Quaternion.identity;
        Image img;
        int i;
        for (i = 0; i < GameController.instance.colors - 1; i++)
        {
            ringParts[i].fillAmount = 1f / GameController.instance.colors;
            ringParts[i].transform.rotation = Quaternion.Euler(Vector3.forward * 360f / GameController.instance.colors * i);
        }

        img = Instantiate(ringPrefab).GetComponent<Image>();
        img.transform.SetParent(transform, false);

        img.color = GameController.instance.color[i];
        img.fillAmount = 1f / GameController.instance.colors;
        img.transform.rotation = Quaternion.Euler(Vector3.forward * 360f / GameController.instance.colors * i);

        ringParts.Add(img);

        transform.rotation = old;
    }
}
