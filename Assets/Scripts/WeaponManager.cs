using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapon;
    public int currentWeapon = 0, previousWeapon;
    public float delay = .5f;

    void Start()
    {
        for (int i = 0; i < weapon.Length; i++)
            weapon[i].SetActive(i == currentWeapon);
    }

    void Update()
    {
        // handle the mouse scroll
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;

            currentWeapon += (int)scroll;
            currentWeapon %= weapon.Length;
            currentWeapon = math.abs(currentWeapon);

            Debug.Log($"current: {currentWeapon}\t scroll: {scroll}\t input: {Input.GetAxis("Mouse ScrollWheel")}\t weapon.Length: {weapon.Length}");
        }
        
        // handle the alpha keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 7;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 9;
        
        // change the active weapon
        if (currentWeapon >= 0 && currentWeapon < weapon.Length && currentWeapon != previousWeapon)
            for (int i = 0; i < weapon.Length; i++)
                weapon[i].SetActive(i ==  currentWeapon); 

        previousWeapon = currentWeapon;
    }
}
