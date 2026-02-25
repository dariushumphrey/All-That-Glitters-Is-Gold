using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTwoVisualSetupScript : MonoBehaviour
{
    private int materialIndex = 4; //Index used to find Material
    private Material phaseTwoSwitch;
    private Renderer subject;
    private Material[] materials;
    private GameObject activation; //VFX used to convey activity

    // Start is called before the first frame update
    void Start()
    {
        subject = GetComponent<Renderer>();
        materials = subject.materials;

        phaseTwoSwitch = Resources.Load<Material>("Materials/Enemies/ReplevinKeystonePhaseTwoGraft");
        materials[materialIndex] = phaseTwoSwitch;
        subject.materials = materials;

        activation = Resources.Load<GameObject>("Particles/LucentThreatHardShatterEffect");
        Instantiate(activation, transform.position, Quaternion.identity);

        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
