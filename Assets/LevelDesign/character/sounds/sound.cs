using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class sound : MonoBehaviour
{
    public AudioClip[] sounds;
    public bool isWalking;
    private bool isAttacking;
    private AudioSource s;

    private bool corInProcess;
    // Start is called before the first frame update
    void Awake()
    {
        isWalking = false;
        s = GetComponent<AudioSource>();
        corInProcess = false;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking) {          
            if(!corInProcess)StartCoroutine(Rate());
        }
    }

    IEnumerator Rate() {
        corInProcess = true;
        yield return new WaitForSeconds(0.35f);
        if (isWalking && !isAttacking)
        {
            int i = Random.Range(0, 2);
            s.clip = sounds[0];
            s.Play();
        }
        isAttacking = false;
        corInProcess = false;
    }

    public void Swing() {
        isAttacking = true;
        s.clip = sounds[2];
        s.Play();
    }
}
