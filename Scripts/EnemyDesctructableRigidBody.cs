using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDesctructableRigidBody : MonoBehaviour
{
    [SerializeField]
    Vector2 forceDirection;

    [SerializeField]
    int torque;

    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(forceDirection);
        rigidbody2D.AddTorque(torque);
    }
}
