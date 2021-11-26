using FigNet.Core;
using UnityEngine;
using AgarIOCommon;

public class PlayerController : MonoBehaviour
{
    private float movementSpeed = 50.0f;
    private float maxMovementSpeed = 3.0f;
    private float increase = 0.05f;
    
    public Vector2 movement;
    public Vector2 mouseDistance;


    private Rigidbody2D rigidBody2D;
    private GameManager gameManager;

    private NetworkPlayerView networkPlayer;

    void Awake() 
    {
        networkPlayer = GetComponent<NetworkPlayerView>();
    }

    public void EnableCameraFollow() 
    {
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        var cameraFollow = camera.GetComponent<SmoothFollow2DCamera>();
        cameraFollow.target = transform;
        cameraFollow.enabled = true;
    }

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();

        increase = AppConstants.PLAYER_SIZE_INCREMENT;
    }

    // FixedUpdate is used for physics
    private void FixedUpdate()
    {
        mouseDistance.x = (Input.mousePosition.x - Camera.main.WorldToScreenPoint(gameObject.transform.position).x) * 0.005f;
        mouseDistance.y = (Input.mousePosition.y - Camera.main.WorldToScreenPoint(gameObject.transform.position).y) * 0.005f;
        movement.x = Input.GetAxis("Horizontal") + mouseDistance.x;
        movement.y = Input.GetAxis("Vertical") + mouseDistance.y;
        movement.x = Mathf.Clamp(movement.x, -maxMovementSpeed, maxMovementSpeed);
        movement.y = Mathf.Clamp(movement.y, -maxMovementSpeed, maxMovementSpeed);
        rigidBody2D.velocity = movement * movementSpeed * Time.deltaTime;

        timer += Time.fixedDeltaTime;

        if (timer > 0.06f)  // wait for 3/4 frames
        {
            timer = 0;
            triggerEnterCount = 0;
        }
    }

    private int triggerEnterCount = 3;
    private float timer;
    // player prefab has 2 colliders 1 is istrigger and other is not, but this event is getting called twice
    void OnTriggerEnter2D(Collider2D other)
    {
        triggerEnterCount++;
        if (triggerEnterCount > 1) return;
        if (other.gameObject.tag == "Food")
        {
            var op = FoodEatenOperation.Get(other.GetComponent<NetworkFoodView>().NetworkFood.Id, networkPlayer.NetworkPlayer.Id);
            FN.Connections[0].SendMessage(op, DeliveryMethod.Reliable);

        }
        else if (other.gameObject.tag == "RemotePlayer")
        {
            // just pass ke ids of both players server will do the logic
            var oView = other.GetComponent<NetworkPlayerView>();
            uint otherPlayerId = oView.NetworkPlayer.Id;
            uint myPlayerId = networkPlayer.NetworkPlayer.Id;
            var op = PlayerKilledOperation.Get(otherPlayerId, myPlayerId);
            FN.Connections[0].SendMessage(op, DeliveryMethod.Reliable);
        }
    }
}
