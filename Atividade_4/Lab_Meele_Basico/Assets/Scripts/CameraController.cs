using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.rotation = player.transform.rotation;
        Vector3 novaPosicao = player.transform.rotation * originalPosition;
        transform.position = player.transform.position + novaPosicao;*/

        transform.position = player.transform.position + originalPosition;

    }
}
