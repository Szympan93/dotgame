using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Dot prefab;
    public int waveCount;
    public int waveInc;
    public float spawnDelay;
    public float spawnAcc;
    public float waveDelay;

	IEnumerator Start ()
    {
        int i;
        yield return new WaitForSeconds(waveDelay / 2);
        while(true)
        {
            i = waveCount;
            while(i-- > 0)
            {
                int form = Random.Range(0, 4);
                switch(form)
                {
                    case 0:
                        yield return StartCoroutine(spawnAllColors());
                        break;
                    case 1:
                        yield return StartCoroutine(spawnCollumn());
                        break;
                    case 2:
                        yield return StartCoroutine(spawnCollision());//yield return StartCoroutine(spawnOrbits());
                        break;
                    case 3:
                        yield return StartCoroutine(spawnOrbits());
                        break;
                }
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(waveDelay);
            GameController.instance.AddColor();
            waveCount += waveInc;
            spawnDelay -= spawnAcc;
        }
    }

//układy
    IEnumerator spawnAllColors()
    {
        float angle = Random.Range(0, 360);
        int color = Random.Range(0, GameController.instance.activeColors);
        for (int j = 0; j < GameController.instance.activeColors; j++)
        {
            Dot dot = Instantiate(prefab);
            dot.transform.rotation = Quaternion.Euler(0, 0, angle + 360 / GameController.instance.activeColors * j);
            dot.color = color + j >= GameController.instance.activeColors ? color + j - GameController.instance.activeColors : color + j;
        }
        yield break;
    }

    IEnumerator spawnCollumn()
    {
        float angle = Random.Range(0, 360);
        int color = Random.Range(0, GameController.instance.activeColors);
        for (int j = 0; j < waveCount; j++)
        {
            Dot dot = Instantiate(prefab);
            dot.transform.rotation = Quaternion.Euler(0, 0, angle - 15 * j);
            dot.color = color;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator spawnOrbits()
    {
        float angle = Random.Range(0, 360);
        int color = Random.Range(0, GameController.instance.activeColors);

        Dot dot = Instantiate(prefab);
        dot.transform.rotation = Quaternion.Euler(0, 0, angle);
        dot.color = color;
        dot.gameObject.AddComponent<Rotate>().speed = 55;

        dot = Instantiate(prefab);
        dot.transform.rotation = Quaternion.Euler(0, 0, angle);
        dot.color = color;
        dot.gameObject.AddComponent<Rotate>().speed = -55;

        yield break;
    }

    IEnumerator spawnCollision()
    {
        float angle = Random.Range(0, 360);
        int color = Random.Range(0, GameController.instance.activeColors);

        Dot dot = Instantiate(prefab);
        dot.transform.rotation = Quaternion.Euler(0, 0, angle - 40);
        dot.color = color;
        dot.gameObject.AddComponent<Rotate>().speed = 10;

        dot = Instantiate(prefab);
        dot.transform.rotation = Quaternion.Euler(0, 0, angle + 40);
        dot.color = color;
        dot.gameObject.AddComponent<Rotate>().speed = -10;

        yield break;
    }
}
