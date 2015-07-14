using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public Vector2 speed;

	void Update ()
    {
        transform.Translate(speed * Time.deltaTime);
	}
}
