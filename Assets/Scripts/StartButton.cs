using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class StartButton : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void loadLevel()
    {
        animator.SetTrigger("pressed");
        StartCoroutine(_loadLevel());
    }

    private IEnumerator _loadLevel()
    {
        yield return new WaitForSeconds(0.5f);
        Application.LoadLevel("game");
    }
}
