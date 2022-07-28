using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator anim; // Armazena o controlador da anima��o.
    bool isWalkingFlag;   // Armazena os estado do par�metro "isWalking".
    bool isRunningFlag;   // Armazena os estado do par�metro "isRunning".

    PlayerControls input; // Armazena as informacoes do controlador do player.

    Vector2 movimento = new Vector2(); // Armazena os controles de dire��o.
    bool movimentoPressionado; // armazena o estado de mover.
    bool runPressionado; // armazena os estado de correr.

    /* O m�todo Awake � chamado quando uma inst�ncia com esse script for criada
     * (executada antes do metodo Start.
     */
    private void Awake()
    {
        input = new PlayerControls();

        /* performed � uma fun��o de evento que � disparada toda vez que uma
         * determinado estado � registrado. No caso da perfomed, o evento �
         * disparado sempre que uma action � completada.
         * 
         * Para gerenciar tal evento a classe possui uma vari�vel de evento 
         * (pode ser identificada atrav�s da palavra reservada event) que pode
         * ser atribuida atrav�s da subscrita do operador += (sobrecarga de 
         * operadores da linguaguem C#). Dessa forma, assim que performed for
         * chamada, o m�todo passado pelo operador tamb�m ser� chamado.
         * 
         */
        input.Player.Move.performed += ctx =>
        {

            // Armazena o movimento em um vetor 2D.
            movimento = ctx.ReadValue<Vector2>();
            // Verfica se o movimento � 0 em ambos os eixos.
            movimentoPressionado = movimento.x != 0 || movimento.y != 0; 
        };

        // Mesma descri��o do comando a cima.
        input.Player.Run.performed += ctx =>
        {
            // Verifica se o bot�o est� apertado ou n�o.
            runPressionado = ctx.ReadValueAsButton();
        };
    }

    // M�todo de controle de movimento.
    void Mover()
    {
        /* Coloca o par�metro isWalking em true caso o movimento aconte�a e
         * o isWalkingFlag esteja em false.
         */
        if (movimentoPressionado && !isWalkingFlag)
        {
            isWalkingFlag = true;
            anim.SetBool("isWalking", isWalkingFlag);
        }

        /* Coloca o par�metro isWalking em false caso o movimento n�o aconte�a 
         * e o isWalkingFlag esteja em true.
         */
        if (!movimentoPressionado && isWalkingFlag)
        {
            isWalkingFlag = false;
            anim.SetBool("isWalking", isWalkingFlag);
        }


        /* Coloca o par�metro isRunning em true caso o movimento aconte�a e o
         * bot�o de correr esteja pressionado aconte�a e isRunningFlag seja 
         * false.
        */
        if ((movimentoPressionado && runPressionado) && !isRunningFlag)
        {
            isRunningFlag = true;
            anim.SetBool("isRunning", isRunningFlag);
        }

        /* O n�o haja movimento ou a tecla de correr n�o esteja pressionada e
         * isRunningFlag � true, coloca o par�metro isRunningFlag em false.
         */
        if ((!movimentoPressionado || !runPressionado) && isRunningFlag)
        {
            isRunningFlag = false;
            anim.SetBool("isRunning", isRunningFlag);
        }
    }

    // Controle de rota��o da inst�ncia.
    void Rotacionar()
    {
        // Armazena a posi��o atual.
        Vector3 atualPosicao = transform.position;

        // Vetor de mudan�a de posi��o.
        Vector3 novaPosicao = new Vector3(movimento.x, 0f, movimento.y);

        // Atualiza��o da posi��o.
        Vector3 positionToLookAt = atualPosicao + novaPosicao;

        // Tranforma��o de posi��o.
        transform.LookAt(positionToLookAt);
    }

    // Start is called before the first frame update
    void Start()
    {

        // Pega o componente de anima��o da inst�ncia
        anim = GetComponent<Animator>();

        // Inicializa par�metros
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
