using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    [Header("Границы экрана")]
    [SerializeField] private float screenRight = 9.4f;
    [SerializeField] private float screenLeft = -9.4f;
    [SerializeField] private float screenTop = 5.5f;
    [SerializeField] private float screenBottom = -5.5f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x > screenRight) pos.x = screenLeft;
        else if (pos.x < screenLeft) pos.x = screenRight;

        if (pos.y > screenTop) pos.y = screenBottom;
        else if (pos.y < screenBottom) pos.y = screenTop;

        transform.position = pos;
    }
}
