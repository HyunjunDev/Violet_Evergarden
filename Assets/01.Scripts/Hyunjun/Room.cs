using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCam;

    public Vector3 spawnPosition;

    public float timeDelay = 0.25f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
            if (this.gameObject.activeSelf==true)
                StartCoroutine(TimeDelay(timeDelay));
        }
    }


    IEnumerator TimeDelay(float time)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1.0f;
    }
}
