using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticleSystem : MonoBehaviour
{
    public ParticleSystem starSystem;

    public int maxParticles = 100;
    public TextAsset starCSV;
    
    void Awake () {
        starSystem = GetComponent<ParticleSystem>();
        starSystem.maxParticles = maxParticles;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
        bursts[0].minCount = (short)maxParticles;
        bursts[0].maxCount = (short)maxParticles;
        bursts[0].time = 0.0f;
        starSystem.emission.SetBursts(bursts, 1);
    }

    void Start()
    {
        string[] lines = starCSV.text.Split('\n');

        ParticleSystem.Particle[] particleStars = new ParticleSystem.Particle[maxParticles];
        GetComponent<ParticleSystem>().GetParticles(particleStars);

        for (int i = 0; i < maxParticles; i++)
        {
            string[] components = lines[i].Split(',');
            particleStars[i].position = new Vector3(float.Parse(components[1]),
                                                    float.Parse(components[3]),
                                                    float.Parse(components[2])).normalized * Camera.main.farClipPlane * 0.9f;
            
            particleStars[i].remainingLifetime = Mathf.Infinity;
            particleStars[i].startColor = Color.white * (1.0f - ((float.Parse(components[0]) + 1.44f) / 8));
        }

        GetComponent<ParticleSystem>().SetParticles(particleStars, maxParticles);
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class StarParticleSystem : MonoBehaviour
// {
//     public ParticleSystem starSystem;

//     public int maxParticles = 100;
//     public TextAsset starCSV;
    
//     void Awake () {
//         starSystem = GetComponent<ParticleSystem>();
//         starSystem.maxParticles = maxParticles;
//         ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
//         bursts[0].minCount = (short)maxParticles;
//         bursts[0].maxCount = (short)maxParticles;
//         bursts[0].time = 0.0f;
//         starSystem.emission.SetBursts(bursts, 1);
//     }

//     void Start()
//     {
//         string[] lines = starCSV.text.Split('\n');

//         ParticleSystem.Particle[] particleStars = new ParticleSystem.Particle[maxParticles];
//         GetComponent<ParticleSystem>().GetParticles(particleStars);

//         for (int i = 0; i < maxParticles; i++)
//         {
//             string[] components = lines[i].Split(',');
//             particleStars[i].position = new Vector3(float.Parse(components[1]),
//                                                     float.Parse(components[3]),
//                                                     float.Parse(components[2])).normalized * Camera.main.farClipPlane * 0.9f;
            
//             particleStars[i].remainingLifetime = Mathf.Infinity;
//             particleStars[i].startColor = Color.white * (1.0f - ((float.Parse(components[0]) + 1.44f) / 8));
//         }

//         GetComponent<ParticleSystem>().SetParticles(particleStars, maxParticles);
//     }
// }