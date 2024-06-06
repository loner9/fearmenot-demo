using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRate : MonoBehaviour
{
    private ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setHealtLevel(string level)
    {
        var mainModule = particleSystem.main;

        if (level == "high")
        {
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.green);
            mainModule.simulationSpeed = 1.0f;
        }
        else if (level == "mid")
        {
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
            mainModule.simulationSpeed = 1.5f;
        }
        else
        {
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.red);
            mainModule.simulationSpeed = 2.0f;
        }

    }
}
