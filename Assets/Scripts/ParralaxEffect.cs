using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    private float _startingPos;
    private float _lengthOfSprite;
    [SerializeField] float AmountOfParallax;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        _startingPos = transform.position.x;
        _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Position = cam.transform.position;
        float temp = Position.x * (1 - AmountOfParallax);
        float distance = Position.x * AmountOfParallax;

        Vector3 newPosition = new Vector3(_startingPos + distance, transform.position.y, transform.position.z);
        transform.position = newPosition;

        if (temp > _startingPos + (_lengthOfSprite / 2))
        {
            _startingPos += _lengthOfSprite;
        }
        else if (temp < _startingPos - (_lengthOfSprite / 2))
        {
            _startingPos -= _lengthOfSprite;
        }
    }
}
