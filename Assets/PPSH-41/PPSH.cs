using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPSH : MonoBehaviour
{
    //properties
    ParticleSystem emission;
    Animator anim;
    int ammunition;
    bool fire;


    // Start is called before the first frame update
    void Start()
    {
        //find casing emitter
        GameObject emissionCone = transform.Find("ppsh").gameObject.transform.Find("Casing Particle System").gameObject;
        if (emissionCone == null)
        {
            Debug.Log("EMITTER NOT FOUND");
        }
        anim = transform.Find("ppsh").gameObject.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.Log("ANIMATOR NOT FOUND");
        }
        
        
        emission = emissionCone.GetComponent<ParticleSystem>();
        var emit = emission.emission;
        emit.enabled = false;

        ammunition = 72;
        fire = false;

    }

    // Update is called once per frame
    void Update()
    {   
        //need to set ParticleSystem.emission as local variable to use, per unity docs
        var emit = emission.emission;
        if (Input.GetKeyDown("r") && ammunition <=0)
        {
            anim.SetTrigger("Reload");
            anim.SetBool("Empty Fire", false);
            ammunition = 72;
        }

        fire = Input.GetMouseButton(0);

        if(!fire) {
            emit.enabled = false;
        }

        
    }

    void FixedUpdate()
    {
        var emit = emission.emission;
        if (fire)
        {   
            if (ammunition <=0)
            {
                anim.SetBool("Empty Fire", true);
                emit.enabled = false;
            }
            else
            {
                anim.ResetTrigger("Empty Fire");
                anim.SetTrigger("Fire");
                emit.enabled = true;
                ammunition -= 1;
                //Debug.Log(ammunition);
            }
        }
    }
}
