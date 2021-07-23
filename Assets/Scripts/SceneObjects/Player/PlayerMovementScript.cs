using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    // BASED ON: https://drive.google.com/drive/folders/0B6SCkaV_VaTyaG1DZ2phOU1Yekk


    public float moveSpeed = 10.0f;
    public float jumpSpeed = 4.0f;
    public float gravity = 9.8f;
    public float terminalVelocity = 100f;

    private CharacterController _charCont;
    private Vector3 _moveDirection = Vector3.zero;

    public AudioSource playerWalkAS;


    void Start()
    {
        _charCont = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (LabSceneManager.instance.playerActive)
        {
            HandlePlayerMove();
        }
        else
        {
            HandlePlayerInactiveMove();
            this.playerWalkAS.Stop();
        }
    }

    private void HandlePlayerMove()
    {
        float calcMoveSpeed = 0f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            calcMoveSpeed = this.moveSpeed * 0.5f;
        }
        else
        {
            calcMoveSpeed = this.moveSpeed;
        }
        // Move direction directly from axes
        float deltaX = Input.GetAxis("Horizontal") * calcMoveSpeed;
        float deltaZ = Input.GetAxis("Vertical") * calcMoveSpeed;
        _moveDirection = new Vector3(deltaX, _moveDirection.y, deltaZ);
        // Accept jump input if grounded
        if (_charCont.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = jumpSpeed;
            }
            else
            {
                _moveDirection.y = 0f;
            }
            if (deltaX != 0 || deltaZ != 0)
            {
                // Handle movement processes, such as footsteps SFX
                if (!this.playerWalkAS.isPlaying)
                {
                    this.playerWalkAS.Play();
                }
            }
        }
        else
        {
            // Handle movement stop processes, such as footsteps SFX
            this.playerWalkAS.Stop();
        }
        ApplyMovement();
    }

    private void HandlePlayerInactiveMove()
    {
        _moveDirection = Vector3.zero;
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        _moveDirection = transform.TransformDirection(_moveDirection);
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, 
        // and once below when the moveDirection is multiplied by deltaTime). 
        // This is because gravity should be applied as an acceleration (ms^-2)
        _moveDirection.y -= this.gravity * Time.deltaTime;
        // Move the controller
        _charCont.Move(_moveDirection * Time.deltaTime);
    }


}
