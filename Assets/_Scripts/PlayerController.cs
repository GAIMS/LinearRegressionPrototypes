using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private List<Vector2> clickedPos;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        clickedPos = new List<Vector2>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetPos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            clickedPos.Add(targetPos);
            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rb.AddForce(transform.right * speed);
        }
    }
}
