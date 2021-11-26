using UnityEngine;
using FigNet.Core;
using AgarIOCommon;
using System.Collections;
using AgarIOCommon.DataModel;

public class NetworkPlayerView : MonoBehaviour
{
    public AgarIOCommon.NetworkPlayer NetworkPlayer;
    public Color Color;
    private bool IsMine;
    public uint Score => NetworkPlayer.Score;
    public uint Rank => NetworkPlayer.Rank;

    [SerializeField] private TextMesh nameLable;
    private RingBuffer<System.Numerics.Vector2> positionBuffer;

    private const int SYNC_RATE = 20;
    private Vector3 itemPosition;
    private bool IsInit = false;

    private float TIME_DELTA;

    [SerializeField]
    private Collider2D[] Colliders;

    private Camera mainCam;

    private void EnableCollision() 
    {
        foreach (var col in Colliders)
        {
            col.enabled = true;
        }
    }

    public void Init(AgarIOCommon.NetworkPlayer networkPlayer)
    {
        TIME_DELTA = (float)SYNC_RATE / (1f / Time.fixedDeltaTime);

        NetworkPlayer = networkPlayer;
        IsMine = NetworkPlayer.IsMine;
        transform.position = new Vector3(networkPlayer.Position.X, networkPlayer.Position.Y, 0);
        transform.rotation = Quaternion.identity;
        var spriteRnd = GetComponent<SpriteRenderer>();
        spriteRnd.color = new Color(networkPlayer.Color.X, networkPlayer.Color.Y, networkPlayer.Color.Z);

        if (IsMine)
        {
            StartCoroutine(SendPositionUpdate());
            mainCam = Camera.main;
        }
        else
        {
            IsInit = true;
            positionBuffer = new RingBuffer<System.Numerics.Vector2>(60);     
            int scoreToAdd = (int)NetworkPlayer.Score;
            for (int i = 0; i < scoreToAdd; i++)
            {
                EatFood();
            }
            Invoke(nameof(EnableCollision), 1f);
        }
        SetName(networkPlayer.Name);
    }
     
    float interval = 1.0f / SYNC_RATE;
    Vector3 previousPosition;
    IEnumerator SendPositionUpdate()
    {
        var wait = new WaitForSeconds(interval);
        for (; ; )
        {
            if (previousPosition != transform.position)
            {
                previousPosition = transform.position;

                var op = PositionSyncOperation.Get(NetworkPlayer.Id, new System.Numerics.Vector2(transform.position.x, transform.position.y));
                FN.Connections[0].SendMessage(op, DeliveryMethod.Unreliable);
            }

            yield return wait;
        }
    }

    public void SetName(string name)
    {
        nameLable.text = name;
    }

    public void UpdatePosition(PositionSyncData data) 
    {
        // todo: add queue for smooth movement
        positionBuffer.Enqueue(data.Position);
        //transform.position = new Vector3(data.Position.X, data.Position.Y, 0);
    }

    public void EatFood() 
    {
        transform.localScale += new Vector3(AppConstants.PLAYER_SIZE_INCREMENT, AppConstants.PLAYER_SIZE_INCREMENT, 0);
        NetworkPlayer.Score++;

        if (IsMine)
        {
            GameManager.GetInstance().UpdateScore(NetworkPlayer.Score);
            float size = Mathf.Clamp(5 + NetworkPlayer.Score / 50f, 5f, 60f);
            mainCam.orthographicSize = size;
        }
    }

    public void EatPlayer(uint score) 
    {
        for (int i = 0; i < score; i++)
        {
            EatFood();
        }
    }

    float moveUpdateTime;
    private void ApplyPositionUpdate()
    {
        moveUpdateTime += TIME_DELTA;

        if (!positionBuffer.IsEmpty && moveUpdateTime >= 1f)
        {
            var pos = positionBuffer.Dequeue();
            itemPosition.x = pos.X;
            itemPosition.y = pos.Y;
            itemPosition.z = transform.position.z;

            moveUpdateTime = 0;
        }
        else
        {
            moveUpdateTime = 1f;
        }

        this.transform.position = Vector3.Lerp(transform.position, itemPosition, moveUpdateTime);
    }

    void FixedUpdate() 
    {
        if (!IsMine && IsInit)
        {
            ApplyPositionUpdate();
        }
    }

    void OnDestroy() 
    {
        StopAllCoroutines();
    }

}
