using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObstacleManager : MonoBehaviour
{
    [SerializeField]
    private List<FallObstacle> fallObj;

    public float delayInSeconds = 1.0f;

    private void Awake()
    {
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer== LayerMask.GetMask("Player"))
        {
            StartCoroutine(SetPlayerInTrue());
        }
    }

    IEnumerator SetPlayerInTrue()
    {
        foreach (var obj in fallObj)
        {
            obj.IsPlayerIn = true;
            yield return new WaitForSeconds(delayInSeconds);
        }
    }
}
