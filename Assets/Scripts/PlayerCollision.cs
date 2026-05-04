using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private Vector3 _startPosition;

    private void Start() {
        _startPosition = transform.position;
        GameManager.Instance.onPlay.AddListener(ActivatePlayer);
    }

    private void ActivatePlayer() {
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (transform.position.y < -8f) {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Obstacle") {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
}