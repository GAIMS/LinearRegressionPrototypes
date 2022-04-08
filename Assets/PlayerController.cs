using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private List<Vector3> clickedPos;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mouse = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            clickedPos.Add(mouse);
            float angle = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rb.AddForce(transform.right * speed);
        }
    }
}
