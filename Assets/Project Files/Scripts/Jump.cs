using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpHeight = 30;

    Vector3[] StartPoint = new Vector3[3];
    void Update()
    {
        void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Mainplayercontroller>();
            if (player != null)
            {
                player.Jump(jumpHeight);
            }
        }
    }
}
