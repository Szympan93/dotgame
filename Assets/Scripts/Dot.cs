using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    [HideInInspector]
    public int color;

	void Start ()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameController.instance.color[color];
	}
}
