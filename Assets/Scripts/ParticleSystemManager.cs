using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ParticleSystemManager : MonoBehaviour
{
    public static ParticleSystemManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public List<Particles> particleDictionary;

    [System.Serializable]
    public class Particles
    {
        public ParticleName particleName;
        public ParticleSystem particleSystem;
    }

    public ParticleSystem GetParticleSystem (ParticleName particleName)
    {
        return particleDictionary.Where(x => x.particleName == particleName).FirstOrDefault().particleSystem;
    }

    public void MakeParticles(ParticleName particleName, Vector3 position, Quaternion rotation, float timeUntilDestroy = 0, GameObject parentObject = null)
    {
        ParticleSystem particles = GetParticleSystem(particleName);
        if (particles != null)
        {
            MakeParticles(particles, position, rotation, timeUntilDestroy, parentObject);
        }
    }

    public void MakeParticles(ParticleSystem particleSystem, Vector3 position, Quaternion rotation, float timeUntilDestroy = 0, GameObject parentObject = null)
    {
        ParticleSystem particles = Instantiate(particleSystem, position, rotation, parentObject.transform);
        if (!particles.isPlaying)
        {
            particles.Play();
        }
        if (timeUntilDestroy != 0)
        {
            Destroy(particles, timeUntilDestroy);
        }
    }

    public ParticleSystem GetParticleInstance(ParticleSystem particleSystem, Vector3 position, Quaternion rotation, float timeUntilDestroy = 0, GameObject parentObject = null)
    {
        ParticleSystem particles = Instantiate(particleSystem, position, rotation, parentObject.transform);
        if (timeUntilDestroy != 0)
        {
            Destroy(particles, timeUntilDestroy);
        }
        return particles;
    }

}

public enum ParticleName
{
    ParticleName1 = 0,
    ParticleName2 = 1,
    ParticleName3 = 2,
}
