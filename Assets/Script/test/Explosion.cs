using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Inspector
    public ParticleSystem[] particle = new ParticleSystem[30];
    [SerializeField] ParticleSystem explosopn;

    private void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            particle[i] = Instantiate(explosopn, new Vector3(-6 + (2 * (i % 6)), 3.5f - (2 * (i / 6)), -2.0f), Quaternion.identity);
        }
    }
}
