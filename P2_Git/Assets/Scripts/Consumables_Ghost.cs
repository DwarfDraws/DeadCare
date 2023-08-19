using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables_Ghost : MonoBehaviour
{
    [SerializeField] Renderer radius_Renderer;
    [SerializeField] Material matWhite, matRed;

    string NoMoveArea_tag = "noMoveArea";

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(NoMoveArea_tag)) return;
        radius_Renderer.material = matRed;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(NoMoveArea_tag)) return;
        radius_Renderer.material = matWhite;
    }
}
