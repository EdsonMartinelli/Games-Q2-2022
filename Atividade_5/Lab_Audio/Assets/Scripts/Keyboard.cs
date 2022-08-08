using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{

    private AudioSource audio; // Armazenará componente de AudioSource
    public int transpose = 0; // Variável para ajuste de potência da frequência

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var nota = -1;
        if (Input.GetKeyDown("1")) nota = 0;  // C
        if (Input.GetKeyDown("2")) nota = 2;  // D
        if (Input.GetKeyDown("3")) nota = 4;  // E
        if (Input.GetKeyDown("4")) nota = 5;  // F
        if (Input.GetKeyDown("5")) nota = 7;  // G
        if (Input.GetKeyDown("6")) nota = 9;  // A
        if (Input.GetKeyDown("7")) nota = 11; // B
        if (Input.GetKeyDown("8")) nota = 12; // C
        if (Input.GetKeyDown("9")) nota = 14; // D
        if (nota >= 0 && audio != null)
        {
            audio.pitch = Mathf.Pow(2, (nota + transpose) / 12.0f);
            audio.Play();
        }
    }
}
