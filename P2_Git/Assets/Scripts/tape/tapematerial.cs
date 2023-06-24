using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tapematerial : MonoBehaviour
{
    public TapeSettings tapesettings;
    public List<GameObject> go;
    public List<Material> tapemats;
    [SerializeField] ObjectSFX sfx;
    private float fillspeed;

    private void Awake()
    {
        if(sfx != null){
            sfx.InteractionTape();
        }
        fillspeed = tapesettings.fillspeed;
        go = new List<GameObject>();
        tapemats = new List<Material>();

        foreach (Transform child in this.transform)
        {
            go.Add(child.gameObject);
            tapemats.Add(child.GetComponent<MeshRenderer>().material);

        }
    }
    void Start()
    {
        foreach (Material mat in tapemats)
        {
            mat.DOFloat(1, "_Fillamount", fillspeed);
        }
    }

    void Update()
    {
    }
}
