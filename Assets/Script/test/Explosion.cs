using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Inspector
    [SerializeField] public ParticleSystem[] particle;

    private void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            particle[i].Stop();
        }
    }
}
