using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    private float GravityModifier = 1.8f;
    private float MinGroundNormalY = 0.65f;

    protected Vector2 TargetVelocity;
    protected bool IsGrounded;
    protected Vector2 GroundNormal;

    protected Vector2 Velocity;
    protected Rigidbody2D PlayerRB2D;
    protected ContactFilter2D CF2D;
    protected RaycastHit2D[] HitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> HitBufferList = new List<RaycastHit2D>(16);

    protected const float MinMoveDistance = 0.001f;
    protected const float ShellRadius = 0.01f;

    void OnEnable()
    {
        PlayerRB2D = GetComponentInChildren<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CF2D.useTriggers = false;
        CF2D.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        CF2D.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        TargetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        ManagePhysics();
    }

    void ManagePhysics()
    {
        Velocity += GravityModifier * Physics2D.gravity * Time.deltaTime;
        Velocity.x = TargetVelocity.x;

        IsGrounded = false;

        Vector2 DeltaPosition = Velocity * Time.deltaTime;

        Vector2 MoveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);

        Vector2 Move = MoveAlongGround * DeltaPosition.x;

        ManageMovement(Move, false);

        Move = Vector2.up * DeltaPosition.y;

        ManageMovement(Move, true);
    }

    void ManageMovement(Vector2 Move, bool YMovement)
    {
        float Distance = Move.magnitude;

        if (Distance > MinMoveDistance)
        {
            int Count = PlayerRB2D.Cast(Move, CF2D, HitBuffer, Distance + ShellRadius);
            HitBufferList.Clear();

            for (int i = 0; i < Count; i++)
            {
                HitBufferList.Add(HitBuffer[i]);
            }

            for (int i = 0; i < HitBufferList.Count; i++)
            {
                Vector2 CurrentNormal = HitBufferList[i].normal;
                if (CurrentNormal.y > MinGroundNormalY)
                {
                    IsGrounded = true;
                    if (YMovement)
                    {
                        GroundNormal = CurrentNormal;
                        CurrentNormal.x = 0;
                    }
                }

                float Projection = Vector2.Dot(Velocity, CurrentNormal);
                if (Projection < 0)
                {
                    Velocity = Velocity - Projection * CurrentNormal;
                }

                float ModifiedDistance = HitBufferList[i].distance - ShellRadius;
                Distance = ModifiedDistance > Distance ? ModifiedDistance : Distance;
            }
        }
        PlayerRB2D.position = PlayerRB2D.position + Move.normalized * Distance;
    }
}
