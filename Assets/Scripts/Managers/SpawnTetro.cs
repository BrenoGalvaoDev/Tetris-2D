using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetro : MonoBehaviour
{
    public static SpawnTetro instance;
    public int nextPiece;

    public Transform[] tetroPieces;

    public List<GameObject> showTetroPieces;

    bool canSpawn = true; // use IEnumerator for don't spawn more pieces in same time
    bool spawn; // use to check is spawn don't work and reset the spawns
    float timer; // use together with bool spawn

    public bool Spawn { get => spawn; set => spawn = value; }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        nextPiece = Random.Range(0, tetroPieces.Length - 1);
        SpawnNewPiece();
    }
    public void SpawnNewPiece()
    {
        if (canSpawn)
        {
            if(Spawn)
            {
                Spawn = false;
            }
            Instantiate(tetroPieces[nextPiece], transform.position, Quaternion.identity);
            nextPiece = Random.Range(0, tetroPieces.Length - 1);
            for (int i = 0; i < tetroPieces.Length; i++)
            {
                showTetroPieces[i].SetActive(false);
            }
            showTetroPieces[nextPiece].SetActive(true);
            StartCoroutine(SpawnCooldown());
        }
    }

    IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(2);
        canSpawn = true;
    }

    private void Update()
    {
        if (Spawn)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                timer = 0;
                Spawn = false;
                SpawnNewPiece();
            }
        }
        else
        {
            timer = 0;
        }
    }
}
