using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DecalSystem;

public class wondClousre : MonoBehaviour
{
    public Sprite wondClousreSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            Decal decalComponent = transform.GetComponent<Decal>();
            decalComponent.Sprite = wondClousreSprite;
        }
    }
}
