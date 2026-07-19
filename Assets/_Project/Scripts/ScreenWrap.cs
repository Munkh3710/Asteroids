using UnityEngine;

namespace AsteroidsClone
{
    public class ScreenWrap : MonoBehaviour
    {
        [SerializeField] private float _screenRight = 9.4f;
        [SerializeField] private float _screenLeft = -9.4f;
        [SerializeField] private float _screenTop = 5.5f;
        [SerializeField] private float _screenBottom = -5.5f;

        private void Update()
        {
            Vector3 pos = transform.position;

            if (pos.x > _screenRight)
            {
                pos.x = _screenLeft;
            }
            else if (pos.x < _screenLeft)
            {
                pos.x = _screenRight;
            }

            if (pos.y > _screenTop)
            {
                pos.y = _screenBottom;
            }
            else if (pos.y < _screenBottom)
            {
                pos.y = _screenTop;
            }

            transform.position = pos;
        }
    }
}
