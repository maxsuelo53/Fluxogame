using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTela : MonoBehaviour
{
    float destroyTime = 1.33f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
