using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isColliding = false;
    private Transform pointTransform; //hand position should be updated
    private HashSet<Rigidbody> collidingObjects = new HashSet<Rigidbody>();
    private Transform boxPosition;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "HandPoint")
        {
            collidingObjects.Add(collision.rigidbody); // �浹 �� �� ���� �浹�ϰ� �ִ��� Ȯ���ϱ� ���� hashset�̿�
                                                        // hash�� �̿��ϴ� ������ �ߺ��� ���� ���ؼ�
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "HandPoint")
        {
            isColliding = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "HandPoint")
        {
            collidingObjects.Remove(collision.rigidbody);
        }
        if(collidingObjects.Count <= 1)
        {
            isColliding =false;
        }
    }
    void Start()
    {
        boxPosition = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(collidingObjects.Count >= 2)
        {
            Vector3 BoxPoistion = Vector3.zero;
            foreach (Rigidbody handPoint in collidingObjects)
            {
                BoxPoistion += handPoint.position;
            }
            BoxPoistion /= collidingObjects.Count;
            this.gameObject.transform.position = BoxPoistion;
        } 
    }
    
}
