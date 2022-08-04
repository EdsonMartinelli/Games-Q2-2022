using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Animator anim; // Armazena o controlador da anima��o.
    bool isWalkingFlag;   // Armazena os estado do par�metro "isWalking".
    bool isRunningFlag;   // Armazena os estado do par�metro "isRunning".
    bool isAttackingFlag;  // Armazena os estado do par�metro "isAttacking".

    PlayerControls input; // Armazena as informacoes do controlador do player.

    Vector2 movimento = new Vector2(); // Armazena os controles de dire��o.
    bool movimentoPressionado; // armazena o estado de mover.
    bool runPressionado; // armazena os estado de correr.

    public float suavizarRotacao = 0; // Armazena um valor para suavizar a rota��o.

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
            /* Verfica se o movimento � 0 em ambos os eixos. A linha foi comentada
             * pois alterei algumas coisas que deixaram a movimenta��o mais fluida.
             */
            // movimentoPressionado = movimento.x != 0 || movimento.y != 0; 

            /* Coloca o movimentoPressionado como true para que Move funcione
             * corretamente.
             */
            movimentoPressionado = true;
        };


        /* canceled � um evento similar ao performed, mas � disparado quando a action
         * � cancelada. Foi adicionado por mim para gerar mais fluidez.
         */
        input.Player.Move.canceled += ctx =>
        {
            // Quando a a��o � cancelada, o movimento � setado para false.
            movimentoPressionado = false;
        };

        // Mesma descri��o do comando a cima.
        input.Player.Run.performed += ctx =>
        {
            // Verifica se o bot�o est� apertado ou n�o.
            runPressionado = ctx.ReadValueAsButton();
        };

        // Mesma descri��o do comando a cima.
        input.Player.Attack.performed += ctx =>
        {
            // Verifica se o bot�o est� apertado ou n�o.
            isAttackingFlag = true;
            anim.SetBool("isAttacking", isAttackingFlag);
        };

        // Mesma descri��o do comando a cima.
        input.Player.Attack.canceled += ctx =>
        {
            // Termina a execu��o do ataque.
            isAttackingFlag = false;
            anim.SetBool("isAttacking", isAttackingFlag);
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
        // Pega a rota��o atual.
        Quaternion atualPosicao = transform.rotation;

        // Pega o angulo dado pelo input em x e y.
        float angulo= Mathf.Atan2(movimento.x, movimento.y) * Mathf.Rad2Deg;

        /* Usa a fun��o euler para criar uma rota��o no eixo Y com o angulo dado
         * por x e y do input.
         */
        Quaternion novaPosicao = Quaternion.Euler(0f, Mathf.Round(angulo), 0f);

        /* Usa Lerp para criar uma rota��o interpolada. Com isso, apenas parte do
         * movimento � feito, assim permitindo uma suavi��o da rota��o (E for�ando
         * o jogador a apertar por mais tempo o bot�o desejado para virar).
         */
        Quaternion x = Quaternion.Lerp(atualPosicao, novaPosicao, Time.deltaTime * suavizarRotacao);

        // Coloca a rota��o obtida como rota��o do objeto.
        transform.rotation = x;

    }

    // Start is called before the first frame update
    void Start()
    {
        // Pega o componente de anima��o da inst�ncia.
        anim = GetComponent<Animator>();

        // Inicializa par�metros.
        anim.SetBool("isWalking", isWalkingFlag);
        anim.SetBool("isRunning", isRunningFlag);
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Rotacionar();
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
