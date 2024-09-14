using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public  float timeDelay;
    private void Start()
    {
        StartCoroutine(CountDownDestroy(timeDelay));
    }


    private IEnumerator CountDownDestroy(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        Destroy(gameObject);
    } 
}
