using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxFx : MonoBehaviour
{
    [SerializeField] private float length, startPos;
    public GameObject cam;
    [SerializeField] private float parralaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
