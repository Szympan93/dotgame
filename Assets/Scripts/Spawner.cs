using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int waveCount;
    public int waveInc;
    public float spawnDelay;
    public float spawnAcc;
    public float waveDelay;

	IEnumerator Start ()
    {
        int i;
        yield return new WaitForSeconds(waveDelay);
        while(true)
        {
            i = waveCount;
            while(i-- > 0)
            {
                GameObject go = Instantiate(prefab);
                go.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(waveDelay);
            GameController.instance.AddColor();
            waveCount += waveInc;
            spawnDelay -= spawnAcc;
        }
    }
}
