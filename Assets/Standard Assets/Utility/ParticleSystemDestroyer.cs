using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Utility
{
    public class ParticleSystemDestroyer : MonoBehaviour
    {
        // allows a particle system to exist for a specified duration,
        // then shuts off emission, and waits for all particles to expire
        // before destroying the gameObject

        private float m_MaxLifetime = 100;


        private void Start()
        {
            var systems = GetComponentsInChildren<ParticleSystem>();

            // find out the maximum lifetime of any particles in this effect
            foreach (var system in systems)
            {
                m_MaxLifetime = Mathf.Max(system.main.startLifetime.constant, m_MaxLifetime);
            }

        }


        public void Stop()
        {
            // stops the particle system early
            Destroy(gameObject);
        }
    }
}
