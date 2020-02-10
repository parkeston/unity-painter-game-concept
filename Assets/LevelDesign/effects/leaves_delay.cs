using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaves_delay : MonoBehaviour
{
    //bool enableEmission;
    private bool inCoroutine;
    private ParticleSystem ps_leaves;

    // Start is called before the first frame update
    void Start()
    {
        //enableEmission = gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        inCoroutine = false;
        ps_leaves = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCoroutine) StartCoroutine(Fall());
    }

    private IEnumerator Fall() {
        inCoroutine = true;
        ps_leaves.Play();
        yield return new WaitForSeconds(10.0f);
        inCoroutine = false;
    }
}
