using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator anim; // Armazena o controlador da animação.
    bool isWalkingFlag;   // Armazena os estado do parâmetro "isWalking".
    bool isRunningFlag;   // Armazena os estado do parâmetro "isRunning".

    PlayerControls input; // Armazena as informacoes do controlador do player.

    Vector2 movimento = new Vector2(); // Armazena os controles de direção.
    bool movimentoPressionado; // armazena o estado de mover.
    bool runPressionado; // armazena os estado de correr.

    /* O método Awake é chamado quando uma instância com esse script for criada
     * (executada antes do metodo Start.
     */
    private void Awake()
    {
        input = new PlayerControls();

        /* performed é uma função de evento que é disparada toda vez que uma
         * determinado estado é registrado. No caso da perfomed, o evento é
         * disparado sempre que uma action é completada.
         * 
         * Para gerenciar tal evento a classe possui uma variável de evento 
         * (pode ser identificada através da palavra reservada event) que pode
         * ser atribuida através da subscrita do operador += (sobrecarga de 
         * operadores da linguaguem C#). Dessa forma, assim que performed for
         * chamada, o método passado pelo operador também será chamado.
         * 
         */
        input.Player.Move.performed += ctx =>
        {

            // Armazena o movimento em um vetor 2D.
            movimento = ctx.ReadValue<Vector2>();
            // Verfica se o movimento é 0 em ambos os eixos.
            movimentoPressionado = movimento.x != 0 || movimento.y != 0; 
        };

        // Mesma descrição do comando a cima.
        input.Player.Run.performed += ctx =>
        {
            // Verifica se o botão está apertado ou não.
            runPressionado = ctx.ReadValueAsButton();
        };
    }

    // Método de controle de movimento.
    void Mover()
    {
        /* Coloca o parâmetro isWalking em true caso o movimento aconteça e
         * o isWalkingFlag esteja em false.
         */
        if (movimentoPressionado && !isWalkingFlag)
        {
            isWalkingFlag = true;
            anim.SetBool("isWalking", isWalkingFlag);
        }

        /* Coloca o parâmetro isWalking em false caso o movimento não aconteça 
         * e o isWalkingFlag esteja em true.
         */
        if (!movimentoPressionado && isWalkingFlag)
        {
            isWalkingFlag = false;
            anim.SetBool("isWalking", isWalkingFlag);
        }


        /* Coloca o parâmetro isRunning em true caso o movimento aconteça e o
         * botão de correr esteja pressionado aconteça e isRunningFlag seja 
         * false.
        */
        if ((movimentoPressionado && runPressionado) && !isRunningFlag)
        {
            isRunningFlag = true;
            anim.SetBool("isRunning", isRunningFlag);
        }

        /* O não haja movimento ou a tecla de correr não esteja pressionada e
         * isRunningFlag é true, coloca o parâmetro isRunningFlag em false.
         */
        if ((!movimentoPressionado || !runPressionado) && isRunningFlag)
        {
            isRunningFlag = false;
            anim.SetBool("isRunning", isRunningFlag);
        }
    }

    // Controle de rotação da instância.
    void Rotacionar()
    {
        // Armazena a posição atual.
        Vector3 atualPosicao = transform.position;

        // Vetor de mudança de posição.
        Vector3 novaPosicao = new Vector3(movimento.x, 0f, movimento.y);

        // Atualização da posição.
        Vector3 positionToLookAt = atualPosicao + novaPosicao;

        // Tranformação de posição.
        transform.LookAt(positionToLookAt);
    }

    // Start is called before the first frame update
    void Start()
    {

        // Pega o componente de animação da instância
        anim = GetComponent<Animator>();

        // Inicializa parâmetros
        anim.SetBool("isWalking", isWalkingFlag);
        anim.SetBool("isRunning", isRunningFlag);
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Rotacionar();

        //print("Movimento X: " + movimento.x + "        Movimento Y: " + movimento.y);
    }


    // Habilita input.
    public void OnEnable()
    {
        input.Player.Enable();
    }


    // Desabilita input.
    public void OnDisable()
    {
        input.Player.Disable();
    }
}
