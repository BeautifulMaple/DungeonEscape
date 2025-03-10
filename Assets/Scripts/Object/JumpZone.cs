using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    public float jumpForce;
    public Vector3[] positions;
    public float speed;

    private int targetIndex = 0;
    private Rigidbody rb;

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (positions.Length == 0) return;
        // 
        transform.position = Vector3.MoveTowards(transform.position, positions[targetIndex], speed * Time.deltaTime);

        //
        if (Vector3.Distance(transform.position, positions[targetIndex]) < 0.1f)
        {
            targetIndex = (targetIndex + 1) % positions.Length;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                collision.transform.SetParent(transform); // 플레이어를 발판의 자식으로 설정
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // 플레이어의 부모를 원래대로 설정
        }
    }
}
