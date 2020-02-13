using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public Sprite itemOutline;
    public float itemValue;
    public string itemName;
    public bool alreadyStolen = false;
    public bool canBePickedUp = false;

    public GameObject outline;
    public GameObject downArrow;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip pickup, drop;
    public SpriteRenderer spr;
    public SpriteRenderer outlineSpr;
    public Collider2D coll;
    public Noise noise;

    public Vector3 startPos;
    public Vector3 endPos;



    private void Start()
    {

        outlineSpr.sprite = itemOutline;
        itemName = name;
        spr.sprite = itemSprite;
        Shader.SetGlobalTexture("_MainTex", spr.sprite.texture);
        
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //if(transform.position.y < player.transform.position.y - .75f)
        //{
        //    spr.sortingOrder = 3;
        //}
        //else
        //{
        //    spr.sortingOrder = 1;
        //}
    }

    public IEnumerator ThrowItem()
    {
        anim.SetBool("Active", true);
        transform.parent = null;
        spr.enabled = true;

        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, 15f * Time.deltaTime);
            yield return null;
        }

        MakeNoise();

        if (alreadyStolen)
        {
            downArrow.SetActive(true);
        }

        coll.enabled = true;
    }

    public void CollectItem()
    {
        outline.SetActive(false);
        spr.enabled = false;
        coll.enabled = false;
        downArrow.SetActive(false);
        canBePickedUp = false;
        PickUpNoise();
    }

    void PickUpNoise()
    {
        audioSource.PlayOneShot(pickup);
    }

    void MakeNoise()
    {
        Instantiate(noise.gameObject, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(drop);
    }
}
