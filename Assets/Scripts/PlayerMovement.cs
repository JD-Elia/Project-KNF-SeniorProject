using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetpos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;

    [SerializeField] private float crouchHeight = 0.5f;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;


    private void Update() {
        isGrounded = Physics2D.OverlapCircle(feetpos.position, groundDistance, groundLayer);

//jumping thangs
        if (isGrounded && Input.GetButtonDown("Jump")) {
            isJumping = true;
            rb.linearVelocity = Vector2.up * jumpforce;
        }

        if (isJumping && Input.GetButton("Jump")) {
            if (jumpTimer < jumpTime) {
                rb.linearVelocity = Vector2.up * jumpforce;
            } else {
                isJumping = false;
            }
            jumpTimer += Time.deltaTime;
        }
        
        if (Input.GetButtonUp("Jump")) {
            isJumping = false;
            jumpTimer = 0;
        }
/* sliding thangs borrowed from a tutorial to be turned later into a slide mechanic
        if (isGrounded && Input.GetButtonDown("Crouch")) {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);
        }

        if (Input.GetButtonUp("Crouch")) {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
        */
    }
}
