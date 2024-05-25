using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningRocks : MonoBehaviour
{
    [SerializeField] float _spinPerSec = 0.75f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward * _spinPerSec);
    }
}
