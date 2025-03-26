using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRightWall : MonoBehaviour
{
    public Transform player;
    public float offset = 5f;

    void LateUpdate()
    {

        if (player.position.x + offset > transform.position.x)
        {
            transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        }
        Debug.Log("Posición del jugador: " + player.position.x);
    }
}
