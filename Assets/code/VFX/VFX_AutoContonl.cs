using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_AutoContonl : MonoBehaviour
{
    private SpriteRenderer sr;
    public float destroytime = 2f;
    public bool rodomPositon = true;
    public bool rodomRotation = true;

    [Header("Fade effect")]
    [SerializeField] private bool canFade ;
    [SerializeField] private float fadeSpeed = 1f;

    [Header("Random position")]
    [SerializeField] private float minXoffset = -0.3f;
    [SerializeField] private float maxXoffset = 0.3f;
    [Space]
    [SerializeField] private float minYoffset = -0.3f;
    [SerializeField] private float maxYoffset = 0.3f;
    [Space]
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 360f;
    // Start is called before the first frame update
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        if (canFade)
            StartCoroutine(Fadeco());
        ApplyRandomOffset();
        ApplyRandomRotation();
        Destroy(this.gameObject, destroytime);
    }

    private IEnumerator Fadeco()
    {
        Color targetCorlor = Color.white;
        while(targetCorlor.a>0)
        {
            targetCorlor.a -= Time.deltaTime * fadeSpeed;
            sr.color = targetCorlor;
            yield return null;
        }
        sr.color = targetCorlor;

    }
    private void ApplyRandomOffset()
    {
        if (rodomPositon == false)
            return;
        Vector3 pos = new Vector3(this.transform.position.x + Random.Range(minXoffset, maxXoffset),
            this.transform.position.y + Random.Range(minYoffset, maxYoffset), 0);
        this.transform.position = pos;
    }
    private void ApplyRandomRotation()
    {
        if (rodomRotation == false)
            return;
        this.transform.Rotate(0, 0, Random.Range(minRotation, maxRotation));
    }
}
