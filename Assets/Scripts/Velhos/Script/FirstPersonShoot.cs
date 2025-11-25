using UnityEngine;

public class FirstPersonShoot : MonoBehaviour
{
    [Space]
    [Header("Alcance do disparo")]
    public float alcanceTiro = 10000f;
    [Header("Particulas")]
    public Transform particulaFuzzy;
    public Transform particulaImpacto;
    Transform AncoraFuzzy;
    Rigidbody rbAlvo;
    [Space]
    [Header("Audio")]
    public GameObject GO_Som;
    public GameObject GO_Hit;
    public AudioClip SomTiro1;
    public AudioClip SomTiro2;
    public AudioClip Recarregando;
    public AudioClip Impacto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AncoraFuzzy = GameObject.Find("Pistol/LocalFuzzle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Atirar();
        }
    }

    void Atirar()
    {
        if (GO_Som.transform.GetComponent<AudioSource>().isPlaying) return;

        Transform Fuzzleinstaciado = Instantiate(particulaFuzzy);
        Fuzzleinstaciado.position = AncoraFuzzy.position;
        Fuzzleinstaciado.rotation = AncoraFuzzy.rotation;
        Fuzzleinstaciado.localScale = AncoraFuzzy.localScale;

        //--------------------------------------
        transform.GetComponent<FirstPersonPlayer>().AnimAtirar();
        //--------------------------------------

        GO_Som.transform.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        //GO_Som.transform.GetComponent<AudioSource>().Play();
        int indiceTiro = Random.Range(0, 2);

        if (indiceTiro == 0)
            GO_Som.transform.GetComponent<AudioSource>().PlayOneShot(SomTiro2);
        else
            GO_Som.transform.GetComponent<AudioSource>().PlayOneShot(SomTiro1);


        Ray raio = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit atingido;

        if (Physics.Raycast(raio, out atingido, alcanceTiro))
        {
            Debug.Log("Acertou em: " + atingido.transform.name);

            Transform Hitinstaciado = Instantiate(particulaImpacto);
            Hitinstaciado.position = atingido.point;
            GO_Hit.transform.position = atingido.point;
            GO_Hit.transform.GetComponent<AudioSource>().PlayOneShot(Impacto);

            if (atingido.transform.TryGetComponent<Inimigo>(out Inimigo inimigo))
            {
               atingido.transform.TryGetComponent<Rigidbody>(out rbAlvo);

                inimigo.LevarDano(1);
               
                if (rbAlvo != null && inimigo.vida > 0)
                {
                     Vector3 direcaoImpacto = atingido.transform.position - transform.position;
                     rbAlvo.AddForce(direcaoImpacto.normalized * 500f);
                }
            }
        }

        GO_Som.transform.GetComponent<AudioSource>().pitch = 1;
    }
}
