using UnityEngine;

public class InstrumentParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem noteParticles;

    public void StartPlaying()
    {
        if (noteParticles) noteParticles.Play();
    }

    public void StopPlaying()
    {
        if (noteParticles) noteParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}