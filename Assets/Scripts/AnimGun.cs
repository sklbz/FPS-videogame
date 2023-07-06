using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimGun : MonoBehaviour
{
    Fire FireScript;
    Animator Anim;
    void Start()
    {
        FireScript = GetComponent<Fire>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FireScript._state == WeaponState.STATE_AIMING && Input.GetKeyDown(KeyCode.Mouse0) && FireScript.cartouches > 0)
            Anim.SetTrigger("Fire");

        if (Input.GetAxis("Vertical") != 0 && !Input.GetKey(KeyCode.LeftShift))
            Anim.SetBool("isWalking", true);
        else
            Anim.SetBool("isWalking", false);

        if (Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.LeftShift))
            Anim.SetBool("isRunning", true);
        else
            Anim.SetBool("isRunning", false);

        if (FireScript._state == WeaponState.STATE_RELOADING)
        {
            Debug.Log($"Reload at time: {Time.time}");
            Anim.SetTrigger("Reload");
            FireScript._state = WeaponState.STATE_PAUSED;
        }
    }
}
