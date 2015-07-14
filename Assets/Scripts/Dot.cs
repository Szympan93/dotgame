using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    [HideInInspector]
    public int color;

	void Start ()
    {
        color = Random.Range(0, GameController.instance.colors);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameController.instance.color[color];
	}
}
