using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    

    public enum HitType
    {
        Wheel,
        Head,
        Torso,
        LeftFoot,
        RightFoot,
        LeftHand,
        RightHand
    }

    [Header("Settings")]
    public HitType expectedHitType = HitType.Head;
    public TMPro.TextMeshProUGUI scoreText;
    public int scoreMultiplierStep = 3;
    public Vector3 defaultPosition = new(0, 5, 10.8000002f);
    public Quaternion defaultRotation = Quaternion.identity;
    public GameObject particlesHit;
    public AudioClip gaspAudio;
    public AudioClip[] laughAudios;
    public AudioClip booAudio;
    public Heart heart;
    public Animator animator;
    public GameObject pie;
    public Thrower Throw;
    public bool canDance = false;

    [Header("Level 0: Z Rotation")]
    public float zRotationSpeed = 20f;
    public int zRotationSpeedStep = 1;
    public float extraZRotationPercentage = 0.15f;

    [Header("Level 1: Y Rotation")]
    public int yRotationStart = 10;
    public int yRotationSpeedStep = 1;
    public float yRotationSpeed = 15f;
    public float extraYRotationPercentage = 0.15f;

    [Header("Level 2: X Rotation")]
    public int xRotationStart = 20;
    public int xRotationSpeedStep = 1;
    public float xRotationSpeed = 15f;
    public float extraXRotationPercentage = 0.15f;


    [Header("Player")]
    public int startLives = 5;
    public int restoreLifeComboLength = 5;

    public int lives;
    public bool isGameOver = false;
    public int score = 0;
    int hitCounter = 0;
    AudioSource audioSource;

    

    void Start()
    {
        RestartGame();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        
        float zSpeed = zRotationSpeed * (1 + extraZRotationPercentage * score / zRotationSpeedStep);
        float ySpeed = 0f;
        float xSpeed = 0f;
        if (score >= yRotationStart)
        {
            ySpeed = yRotationSpeed * (extraYRotationPercentage * (score - yRotationStart) / yRotationSpeedStep);
        }
        if (score >= xRotationStart)
        {
            xSpeed = xRotationSpeed * (extraXRotationPercentage * (score - xRotationStart) / xRotationSpeedStep);
        }
        transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.R))
        {
            RestartGame();
        }
        if (score >= PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    void PlayLaugh()
    {
        canDance = true;
        audioSource.pitch = 1;
        int clipId = Random.Range(0, laughAudios.Length);
        audioSource.PlayOneShot(laughAudios[clipId]); 
    }

    void PlayGasp() {
        audioSource.pitch = Random.Range(0.6f, 1.5f);
        audioSource.PlayOneShot(gaspAudio);
        canDance = false;
    }

    void PlayBoo() {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(booAudio);
    }

    void ShowParticles(ContactPoint contactPoint) {
        ParticleSystem ps = particlesHit.GetComponent<ParticleSystem>();
        ps.Clear();
        particlesHit.transform.position = contactPoint.point;
        ps.Play();
        AudioSource audio = particlesHit.GetComponent<AudioSource>();
        audio.Play();        
    }

    public void OnHit(HitType hitType, ContactPoint contactPoint)
    {
        ShowParticles(contactPoint);
        if (hitType == expectedHitType)
        {
            PlayLaugh();
            hitCounter += 1;
            Debug.Log(hitCounter);
            score += (int) Mathf.Pow(2,(int)(hitCounter / scoreMultiplierStep));
            SelectNewTarget();
            RestoreHealth();
        } else
        {
            PlayGasp();
            lives -= 1;
            hitCounter = 0;
        }
        if (hitType == HitType.Wheel)
        { 
            if(score > 0)
            {
                score -= 1;
            }
            

        }
        scoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        scoreText.text += "\nSCORE: " + score.ToString();
        scoreText.text += "\nLIVES: " + lives;
        scoreText.text += "\nMULTIPLIER: " + Mathf.Pow(2, hitCounter / scoreMultiplierStep);

        heart.SetHearts(hitCounter % restoreLifeComboLength);
        if (lives <= 0)
        {
            GameOver();
        }
    }

    

    public HitType CurrentTarget()
    {
        return expectedHitType;
    }

    void SelectNewTarget()
    {
        HitType oldType = expectedHitType;
        int newType = UnityEngine.Random.Range(1, 7);
        if (newType == (int) oldType)
        {
            newType = Mathf.Clamp((newType + 1) % 6, 1, 6);
        }
        expectedHitType = (HitType)newType;
    }

    void RestoreHealth()
    {
        if (hitCounter % restoreLifeComboLength == 0)
        {
            lives += 1;
        }
    }
    void GameOver()
    {
        PlayBoo();
        transform.localScale = Vector3.one;
        scoreText.text = "GAME OVER\nSCORE: " + score.ToString() + "\n Press R to restart";
        animator.SetBool("Defeat", true);
        Throw.HideAimAssist();
        isGameOver = true;
        pie.SetActive(false);
    }
    public void RestartGame()
    {
        scoreText.text = "Clown ROULETTE\n\nUse MB1 to aim.\nHit the HIGHLIGHTED body part!";
        score = 0;
        lives = startLives;
        animator.SetBool("Defeat", false);
        isGameOver = false;
        transform.SetPositionAndRotation(defaultPosition, defaultRotation);
        
        StartCoroutine(RespawnPie());
    }

    private IEnumerator RespawnPie()
    {

        yield return new WaitForSeconds(1f);
        pie.SetActive(true);
        Throw.DrawAimAssist();
        Throw.TrowCoroutine();

        

    }
}
