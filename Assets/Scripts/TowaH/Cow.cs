using UnityEngine;

namespace TowaH {
    public class Cow : MonoBehaviour {
        public float timer = 0;
        public float rotation = 0;
        public float cd;

        public float mvm = 15;
        public float timer_dir = 0;
        public float cd_dir = 0;
        private float h = 0;
        private float v = 0;

        // Start is called before the first frame update
        private void Start() {
            h = Random.Range(0, 3);
            v = Random.Range(0, 3);
            timer = Random.Range(0f, 10f) / 10;
        }

        // Update is called once per frame
        private void Update() {
            timer += Time.deltaTime * 2;
            timer_dir += Time.deltaTime;
            if (timer_dir > cd_dir) {
                timer_dir = 0;
                h = Random.Range(0, 3);
                v = Random.Range(0, 3);
            }

            if (timer > cd) {
                timer = 0;
                rotation += 90;
                
                Vector3 rotationVector = transform.rotation.eulerAngles;
                float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg - 90f;
                rotationVector.y = angle;
                rotationVector.x = rotation;
                transform.rotation = Quaternion.Euler(rotationVector);
                var dir = new Vector3(h - 1, 0, v - 1);
                transform.position += dir.normalized * mvm;
            }
        }
    }
}
