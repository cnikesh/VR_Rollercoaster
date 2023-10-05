using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{

    public ParticleSystem explosion1;
    public ParticleSystem explosion2;
    public ParticleSystem flame1;
    public ParticleSystem flame2;
    // Start is called before the first frame update
    void Start()
    {
        //explosions.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        explosion1.Play();
        explosion2.Play();

        flame1.Play();
        flame2.Play();
    }
}
