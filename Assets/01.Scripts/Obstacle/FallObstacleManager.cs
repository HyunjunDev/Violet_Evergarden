using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObstacleManager : MonoBehaviour, IReStartable
{
    [SerializeField]
    private List<FallObstacle> fallObj;

    public float delayInSeconds = 1.0f;

    private Coroutine coroutine;

    private bool isFirst = true;

    private void Awake()
    {
        EventManager.Instance.onFadeIn.AddListener(ReStart);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isFirst)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            coroutine = StartCoroutine(SetPlayerInTrue());
            isFirst = false;
        }
    }

    IEnumerator SetPlayerInTrue()
    {
        foreach (var obj in fallObj)
        {
            obj.IsPlayerIn = true;
            yield return new WaitForSeconds(delayInSeconds);
        }
        coroutine = null;
    }

    public void ReStart()
    {
        isFirst = true;
        if(coroutine!=null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
