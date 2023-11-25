using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHitBlock : MonoBehaviour
{
    public PlayerMovement pm;
    private MeshRenderer blockRenderer;
    public float maxHP = 3;
    private float hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
        blockRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        blockRenderer.material.color = new Color(0, 0, hp / maxHP);
        if (hp <= 0) { hp = 0; Destroy(this.gameObject); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (pm.Attack >0f) { /*Debug.Log("Damage");*/ hp -= pm.Attack; pm.Attack = 0f; }

        }
    }
}
