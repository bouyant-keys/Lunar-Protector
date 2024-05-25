using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    Transform _planet;
    Rigidbody2D _rB;

    float _gravityForce;
    [SerializeField] float _gravityDist = 15f;

    // Start is called before the first frame update
    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        _planet = GameObject.FindGameObjectWithTag("Planet").transform;

        _gravityForce = Physics.gravity.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 distance = _planet.transform.position - transform.position;
        if (distance.magnitude < _gravityDist) _rB.AddForce(distance.normalized * _gravityForce * _rB.mass); //Applies equal force evewrywhere within gravity range
        //_rB.AddForce(distance.normalized * (1.0f - distance.magnitude / _gravityDist) * _gravityForce); //Applies less force further away from planet you are

        var down = (_planet.transform.position - transform.position).normalized;
        var forward = Vector3.Cross(transform.right, down);
        transform.rotation = Quaternion.LookRotation(-forward, -down);
    }
}
