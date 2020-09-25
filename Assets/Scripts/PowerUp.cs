using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float stopPoint = -6f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= stopPoint)
        {
            transform.position = new Vector2(transform.position.x, stopPoint);
        }
    }
}
