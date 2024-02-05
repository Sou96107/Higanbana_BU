using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    // Inspector
    [SerializeField] public ParticleSystem particle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // çƒê∂
    public void Play()
    {
        particle.Play();
    }

    public void Stop()
    {
        particle.Stop();
    }
}
