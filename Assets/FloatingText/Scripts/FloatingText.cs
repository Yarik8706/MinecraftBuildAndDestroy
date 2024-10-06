using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class FloatingText : MonoBehaviour
{
    public TextMesh textMesh;
    public float LifeTime = 1;
    public bool FadeEnd = false;
    public Color TextColor = Color.white;

    private float alpha = 1;
    private float timeTemp = 0;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    private void Start()
    {
        timeTemp = Time.time;
        Destroy(gameObject, LifeTime);
    }

    public void SetText(string text)
    {
        if (textMesh)
            textMesh.text = text;
    }

    private void Update()
    {
        if (FadeEnd)
        {
            if (Time.time >= ((timeTemp + LifeTime) - 1))
            {
                alpha = 1.0f - (Time.time - ((timeTemp + LifeTime) - 1));
            }
        }
        textMesh.color = new Color(TextColor.r, TextColor.g, TextColor.b, alpha);
    }
}
