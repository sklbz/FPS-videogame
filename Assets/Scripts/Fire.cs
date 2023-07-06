using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    AudioClip SoundShoot, SoundReload, SoundEmpty;
    AudioSource AudioSource;
    Ray ray;
    RaycastHit hit;
    Vector2 ScreenCenter;
    public int cartouches, chargeurs, maxCartouches, _power;
    public WeaponState _state = WeaponState.STATE_AIMING;
    [SerializeField]
    GameObject SparksPrefab, Crosshair, BulletPrefab;
    Transform Barrel;
    [SerializeField]
    bool isAutomatic;
    [SerializeField]
    float shootingRate = 1, nextTime = 0;
    Animator anim;

    void Start() {
        maxCartouches = cartouches;
        AudioSource = GetComponent<AudioSource>();
        Barrel = transform.GetChild(0);
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("reload") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("in") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("out") ||
            _state == WeaponState.STATE_RELOADING)
        {
            Crosshair.SetActive(false);
            _state = WeaponState.STATE_PAUSED;
        } else
        {
            Crosshair.SetActive(true);
            _state = WeaponState.STATE_AIMING;
        }

        switch (_state)
        {
            case WeaponState.STATE_AIMING:
                if (isAutomatic && Input.GetKey(KeyCode.Mouse0) && Time.time > nextTime)
                {
                    Shoot();
                    nextTime = Time.time + 1 / shootingRate;
                }
                else if (Input.GetKeyDown(KeyCode.Mouse0))
                    Shoot();
                
                    

                if (Input.GetKeyDown(KeyCode.R))
                    Reload();

                break;

            case WeaponState.STATE_RUNNING:
                Crosshair.SetActive(false);

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    Crosshair.SetActive(true);
                    _state = WeaponState.STATE_AIMING;
                }

                break;

            case WeaponState.STATE_RELOADING:

                break;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Crosshair.SetActive(false);
            _state = WeaponState.STATE_RUNNING;
        }

    }

    public void Shoot() {
        if (cartouches == 0)
        {
            AudioSource.PlayOneShot(SoundEmpty);
            return;
        }

        cartouches--;

        AudioSource.PlayOneShot(SoundShoot);

        

        ScreenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);

        

        ray = Camera.main.ScreenPointToRay (ScreenCenter);

        if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
        {
            Vector3 _bulletDirection = hit.point - Barrel.position;

            // launch a bullet toward the obj
            Instantiate(BulletPrefab, Barrel.position, Quaternion.LookRotation(_bulletDirection));

            string Tag = hit.transform.tag;

            // handle the touched object reaction
            switch (Tag)
            {
                case ("target"):
                    Vector3 force = hit.point - gameObject.transform.position;

                    force.y = 1;

                    if (force.sqrMagnitude > 1)
                        force.Normalize();

                    Debug.Log(force);

                    hit.transform.Rotate(new Vector3(10,10,10));

                   hit.transform.GetComponent<Rigidbody>().AddForce(force * _power);

                    break;
            }

            // add sparks to the impact if touche something with a collider
            if (!hit.collider || Tag == "border" || Tag == "Player")
                return;

            GameObject Sparks;

            Sparks = Instantiate(SparksPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        }            
    }

    public void Reload() {
        if (chargeurs == 0 || cartouches > 0)
            return;

        Crosshair.SetActive(false);

        _state = WeaponState.STATE_RELOADING;

        AudioSource.PlayOneShot(SoundReload);

        chargeurs--;

        StartCoroutine(waitForReload());
    }

    IEnumerator waitForReload() {
        // wait until sound has finished playing
        while (AudioSource.isPlaying)
        {
            yield return null;
        }

        cartouches = maxCartouches;

        while (anim.GetCurrentAnimatorStateInfo(0).IsName("reload"))
            yield return null;

        _state = WeaponState.STATE_AIMING;
    }
}
