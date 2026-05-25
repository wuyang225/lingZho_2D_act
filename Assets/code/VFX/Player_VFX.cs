using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [SerializeField] private float imageEchotime = 0.05f;
    [SerializeField] private GameObject vFX_ImageEcho;
    private Coroutine imageEchoCor;

    public void DoImageEcho(float duration)
    {
        if (imageEchoCor != null)
            StopCoroutine(imageEchoCor);
        imageEchoCor = StartCoroutine(ImageEchoEffectcor(duration));
    }
    private IEnumerator ImageEchoEffectcor(float duration)
    {
        float tagetendtime = duration;
        while (tagetendtime >0)
        {
            CreatImageEcho();
            yield return new WaitForSeconds(imageEchotime);
            tagetendtime -= imageEchotime;
        }
    }

    private void CreatImageEcho()
    {
       GameObject ima= Instantiate(vFX_ImageEcho, this.transform.position, transform.rotation);
        ima.GetComponentInChildren<SpriteRenderer>().sprite = sprite.sprite;
    }
}
