using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform player;
    private Animator anim;

    float randomRotacao = 0;
    float tempoMudarRotacao = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direcao = player.position - transform.position;
        float angulo = Vector3.Angle(player.position, transform.position);
        float distancia = Vector3.Distance(player.position, transform.position);
        if (distancia < 8 )
        {
            direcao.y = 0;
            tempoMudarRotacao = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direcao),
                                                  Time.deltaTime * 10);

            if(direcao.magnitude > 2)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
            } else
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
            }
        } else if (distancia >= 8 && distancia < 20)
        {
            tempoMudarRotacao += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.Euler(0f, randomRotacao, 0f),
                                                  Time.deltaTime * 10);
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        } else
        {
            tempoMudarRotacao = 0;
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
        }

        if (tempoMudarRotacao > 2)
        {
            randomRotacao = Mathf.Lerp(Random.Range(0f, 360f), angulo, 0.5f);
            tempoMudarRotacao = 0;
        }
    }
}
