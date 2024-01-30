
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    public AudioSource hit;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pie"))
        {
            collision.gameObject.tag = "InactivePie";

            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                hit.pitch = 1f;
                hit.Play();

            }
            else
            {
                audioSource.Play();
                hit.pitch = 1.20f;
                hit.Play();
            }
        }
    }
}
