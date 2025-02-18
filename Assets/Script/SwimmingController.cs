using UnityEngine;

public class SwimmingController : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;

    public float swimSpeed = 3f;
    public float swimUpSpeed = 2f;
    public float gravity = -9.81f;
    public float waterSurfaceY = 0f;

    private Vector3 moveDirection;
    private bool isInWater = false;
    private bool isSwimming = false;


    void Update()
    {
        if (isInWater)
        {
            Swim();
        }
        else
        {
            MoveOnLand();
        }
    }

    void MoveOnLand()
    {
        // Di chuyển trên cạn
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Xác định hướng di chuyển
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            // Di chuyển trên cạn
            moveDirection = transform.forward * swimSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
        }

        // Thêm trọng lực khi ở trên cạn
        moveDirection.y += gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void Swim()
    {
        // Input cho bơi
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Xác định hướng bơi
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            // Di chuyển trong nước
            moveDirection = transform.forward * swimSpeed;
            characterController.Move(moveDirection * Time.deltaTime);

            isSwimming = true;
            animator.SetBool("isSwimming", true);
        }
        else
        {
            // Đứng nước
            isSwimming = false;
            animator.SetBool("isSwimming", false);
        }

        // Nổi lên khi nhấn Jump
        if (Input.GetButton("Jump"))
        {
            moveDirection.y = swimUpSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            moveDirection.y = 0;
        }

        // Không cho nhân vật nổi lên khỏi mặt nước
        if (transform.position.y > waterSurfaceY)
        {
            Vector3 newPos = transform.position;
            newPos.y = waterSurfaceY;
            transform.position = newPos;
        }
    }

    // Khi nhân vật vào nước
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            animator.SetBool("isInWater", true);
            isSwimming = false; // Đứng nước khi chỉ ở Water
            animator.SetBool("isSwimming", false);
        }

        if (other.CompareTag("SwimZone"))
        {
            isInWater = true;
            isSwimming = true; // Bơi khi vào SwimZone
            animator.SetBool("isInWater", true);
            animator.SetBool("isSwimming", true);
        }
    }

    // Khi nhân vật ra khỏi nước
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            isSwimming = false;
            animator.SetBool("isInWater", false);
            animator.SetBool("isSwimming", false);
        }

        if (other.CompareTag("SwimZone"))
        {
            isInWater = false;
            isSwimming = false;
            animator.SetBool("isInWater", false);
            animator.SetBool("isSwimming", false);
        }
    }
}
