using UnityEngine;

public class FirstPersonPlayer : MonoBehaviour
{
    [Space]
    [Header("Audio")]

    public float velocidadeMovimento = 10f;
    public float sensibilidade = 10f;
    public float forcaPulo = 1.5f;

    float gravidade = -20;
    Vector3 velocidade;
    Vector3 movimento;
    bool estaNoChao = false;
    float rotacaoCamera;
    public float AtrasoMovBraco = 20;
    public float AtrasoRotBraco = 1.7f;

    CharacterController characterController;
    Transform pistol;
    Transform ancora;
    Vector3 movimentoFinal;
    public Animator Animador;

    public GameObject GO_Som;
    public AudioClip Passos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pistol = GameObject.Find("Pistol").transform;
        //Animador = GameObject.Find("Pistol").GetComponent<Animator>();
        ancora = transform.Find("Main Camera/PosicaoDaCam");
    }

    // Update is called once per frame
    void Update()
    {
        Movimento();
        OlharCamera();
        MovimentoBraco();
        AnimAndar();
        Agachar();
    }

    void Movimento()
    {

        estaNoChao = characterController.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movimento = transform.right * horizontal + transform.forward * vertical;
        
        if (movimento.magnitude > 1)
        {
            movimento = movimento.normalized;
        }
            //characterController.Move(movimento * Time.deltaTime * velocidadeMovimento);

                if (velocidade.y < 0 && estaNoChao == true)
                        {
                         velocidade.y = -1f;
                         }

        if (Input.GetButtonDown("Jump") && estaNoChao == true)
        {
            velocidade.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
        }

        velocidade.y += gravidade * Time.deltaTime;

        movimentoFinal = (movimento * velocidadeMovimento + velocidade) * Time.deltaTime;


        characterController.Move(movimentoFinal);
    }

    void OlharCamera()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensibilidade;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidade;

        rotacaoCamera -= mouseY;
        rotacaoCamera = Mathf.Clamp(rotacaoCamera, -65f, 50f);
        Camera.main.transform.localRotation = Quaternion.Euler(rotacaoCamera, 0, 0);

        transform.Rotate(0, mouseX, 0);

    }

    void MovimentoBraco()
    {
        pistol.rotation = ancora.rotation;
        pistol.position = ancora.position;

    }

    void AnimAndar()
    {
        Animador.SetFloat("Andando", movimento.magnitude);

        Debug.Log("Movimento " + movimento.magnitude);

        if (GO_Som.transform.GetComponent<AudioSource>().isPlaying) return;
        
        if (movimentoFinal != Vector3.zero && estaNoChao)
        {
            GO_Som.transform.GetComponent<AudioSource>().clip = Passos;
            GO_Som.transform.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GO_Som.transform.GetComponent<AudioSource>().Play();
        }
    }

    public void AnimAtirar()
    {
        Animador.SetTrigger("Tiro");
    }

    void Agachar()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            characterController.height = 1.2f;
            //characterController.height = characterController.height / 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height = 2.0f;
            //characterController.height = characterController.height * 2;
        }
    }

}
