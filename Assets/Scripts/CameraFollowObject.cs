using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipRotationTime = 0.7f;
    private Coroutine turnCoroutine;
    private PlayerMovement player;
    private bool isFacingRight;
    // Start is called before the first frame update
    void Start()
    {
        player = playerTransform.GetComponent<PlayerMovement>();
        isFacingRight = player.IsFacingRight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
    }

    public void callTurn()
    {
        turnCoroutine = StartCoroutine(flipYLerp());
    }

    private IEnumerator flipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = determineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float determineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (!isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0;
        }
    }
}
