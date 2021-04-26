using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    PlayerInputManager input;
    PlayerStateManager state;

    Rigidbody2D rb;

    //Movement
    public float m_Speed;
    public float m_RotationDampeningFactor;
    public AnimationCurve m_AccelerationCurve;
    public float m_AccelerationTime;

    private float accelerationFactor;
    private Vector2 moveDir;
    public bool facingRight;

    //Dash
    public float m_DashDistance;
    public float m_DashDuration;
    public AnimationCurve m_DashCurve;
    public float m_DashCooldown;
    public float m_DashReboundDistance;
    public float m_DashReboundDuration;

    //Pick up / Drop
    private CarryableDetector carryableDetector;
    private GameObject carriedObject;
    private SortingGroup carriedObjectSortingGroup;
    private SortingGroup playerSortingGroup;
    public Transform carriedPos;
    public Transform placedPos;
    public SpriteRenderer CarriedObjectShadow;

    //Interact
    private InteractableDetector interactableDetector;

    void Start()
    {
        input = PlayerInputManager.instance;
        state = PlayerStateManager.instance;

        rb = GetComponent<Rigidbody2D>();
        carryableDetector = GetComponentInChildren<CarryableDetector>();
        interactableDetector = GetComponentInChildren<InteractableDetector>();

        moveDir = new Vector2(1.0f, 0.0f);

        playerSortingGroup = GetComponentInChildren<SortingGroup>();
    }

    void Update()
    {
        if (state.CanMove())
            Move();
        else
            state.EndWalk();

        if (input.GetDashInput() && state.CanDash())
            StartCoroutine(Dash());

        if (input.GetPickUpInput()) {
            if (state.Carrying)
                PutDown();
            else
                PickUp();
        }

        if (state.Carrying) {
            if (carriedPos.localPosition.x > 0.0f != facingRight) {
                carriedPos.localPosition = new Vector3(-carriedPos.localPosition.x, carriedPos.localPosition.y, carriedPos.localPosition.z);
                CarriedObjectShadow.transform.localPosition = new Vector3(-carriedPos.localPosition.x, CarriedObjectShadow.transform.localPosition.y, CarriedObjectShadow.transform.localPosition.z);
                placedPos.localPosition = new Vector3(-placedPos.localPosition.x, placedPos.localPosition.y, placedPos.localPosition.z);
            }
            else {
                CarriedObjectShadow.transform.localPosition = new Vector3(carriedPos.localPosition.x, CarriedObjectShadow.transform.localPosition.y, CarriedObjectShadow.transform.localPosition.z);
            }
            carriedObject.transform.position = carriedPos.position;
            carriedObject.transform.rotation = carriedPos.rotation;
            carriedObjectSortingGroup.sortingOrder = playerSortingGroup.sortingOrder + 1;
        }

        if (input.GetInteractInput()) {
            Interact();
        }
    }

    private void Move() {
        if (input.Move.magnitude != 0.0f) {
            state.StartWalk();
            //Increase acceleration factor
            accelerationFactor = Mathf.Min(accelerationFactor + Time.deltaTime / m_AccelerationTime, input.Move.magnitude);

            Vector2 zero = Vector2.zero;
            moveDir = Vector2.SmoothDamp(moveDir, input.Move.normalized, ref zero, m_RotationDampeningFactor * m_AccelerationCurve.Evaluate(accelerationFactor));
        }
        else {
            Vector2 zero = Vector2.zero;
            state.EndWalk();
            //Decrease acceleration factor
            accelerationFactor = Mathf.Max(accelerationFactor - Time.deltaTime / m_AccelerationTime, 0.0f);

            Vector2 defaultDir = moveDir.x > 0.01f ? Vector2.right : Vector2.left;
            moveDir = Vector2.SmoothDamp(moveDir, defaultDir, ref zero, m_RotationDampeningFactor * m_AccelerationCurve.Evaluate(accelerationFactor));
        }

        //Facing
        if (moveDir.x > 0.01f) {
            facingRight = true;
        }
        else if (moveDir.x < -0.01f) {
            facingRight = false;
        }

        rb.velocity = moveDir * m_Speed * m_AccelerationCurve.Evaluate(accelerationFactor);
    }

    private IEnumerator Dash() {
        state.StartDash();
        gameObject.layer = LayerMask.NameToLayer("Dash");

        if (state.Carrying)
            DropCarriedObject();

        Vector2 dashStartPos = rb.position;
        Vector2 dashDir = moveDir.normalized;
        Vector2 dashEndPos = dashStartPos + dashDir * m_DashDistance;

        bool collision = false;
        Vector2 collisionPoint = Vector2.zero;

        int layerMask = LayerMask.GetMask("Impassable");
        RaycastHit2D hit = Physics2D.BoxCast(dashStartPos, new Vector2(1.0f, 1.0f), 0.0f, dashDir, m_DashDistance, layerMask);
        if(hit.collider != null) {
            collision = true;
            collisionPoint = hit.point - dashDir * 0.5f;
        }

        Vector2 startVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        float dashTimer = 0.0f;
        while(dashTimer < m_DashDuration) {
            float fraction = m_DashCurve.Evaluate(dashTimer / m_DashDuration);
            if(collision && fraction >= hit.fraction) { 
                break;
            }
            else {
                rb.MovePosition(Vector2.Lerp(dashStartPos, dashEndPos, fraction));
            }
            dashTimer += TimeManager.instance.deltaTime;
            yield return new WaitForLaundromatSeconds(0.0f);
        }

        if (collision) {
            AudioManager.instance.PlaySound(SoundName.Collision);
            Vector2 reflected = Vector2.Reflect(dashDir, hit.normal);
            dashEndPos = collisionPoint + reflected * m_DashReboundDistance;
            float reboundTimer = 0.0f;
            while(reboundTimer < m_DashReboundDuration) {
                rb.MovePosition(Vector2.Lerp(collisionPoint, dashEndPos, m_DashCurve.Evaluate(reboundTimer / m_DashReboundDuration)));
                reboundTimer += TimeManager.instance.deltaTime;
                yield return new WaitForLaundromatSeconds(0.0f);
            }
        }

        state.EndDash();
        gameObject.layer = LayerMask.NameToLayer("Player");

        StartCoroutine(state.DashCooldown(m_DashCooldown));

        rb.MovePosition(dashEndPos);
        if(!collision) rb.velocity = startVelocity;
    }

    private void StartCarrying(GameObject obj) {
        state.StartCarry();
        AudioManager.instance.PlaySound(SoundName.PickUpBasket);
        carriedObject = obj;
        carriedObject.GetComponent<Collider2D>().enabled = false;
        Carryable carryable = carriedObject.GetComponent<Carryable>();
        if (carryable != null) carryable.ShadowRenderer.enabled = false;
        CarriedObjectShadow.enabled = true;
        LaundromatSpriteSort spriteSort = obj.GetComponentInChildren<LaundromatSpriteSort>();
        if (spriteSort != null) {
            spriteSort.enabled = false;
            carriedObjectSortingGroup = obj.GetComponentInChildren<SortingGroup>();
        }
    }

    private void DropCarriedObject() {
        state.EndCarry();
        AudioManager.instance.PlaySound(SoundName.DropBasket);
        carriedObject.GetComponent<Collider2D>().enabled = true;
        carriedObject.transform.position = placedPos.position;
        LaundromatSpriteSort spriteSort = carriedObject.GetComponentInChildren<LaundromatSpriteSort>();
        if (spriteSort != null) {
            spriteSort.enabled = true;
        }
        Carryable carryable = carriedObject.GetComponent<Carryable>();
        if (carryable != null) carryable.ShadowRenderer.enabled = true;
        CarriedObjectShadow.enabled = false;
        carriedObject = null;
    }

    private void DestroyCarriedObject() {
        state.EndCarry();
        CarriedObjectShadow.enabled = false;
        Destroy(carriedObject);
        carriedObject = null;
    }

    public void PickUp() {
        if(state.CurrentRoomIndex == 0) {
            GameObject targetedObject = carryableDetector.GetNearestCarryable();
            if (targetedObject != null) {
                Carryable carryable = targetedObject.GetComponent<Carryable>();
                if (carryable != null) {
                    GameObject carryableObject = carryable.GetCarryableObject();
                    if(carryableObject != null)
                        StartCarrying(carryableObject);
                }
                LaundromatBasket laundromatBasket = targetedObject.GetComponent<LaundromatBasket>();
                if (laundromatBasket != null) {
                    if (CustomerManager.CustomerServed != null) CustomerManager.CustomerServed(laundromatBasket);
                }
            }
        }
    }

    public void Take(GameObject obj) {
        if (carriedObject != null) DropCarriedObject();
        StartCarrying(obj);
        AudioManager.instance.PlaySound(SoundName.PickUpBasket);

        LaundromatBasket laundromatBasket = obj.GetComponent<LaundromatBasket>();
        if (laundromatBasket != null) {
            if (CustomerManager.CustomerServed != null) CustomerManager.CustomerServed(laundromatBasket);
        }
    }

    private void PutDown() {
        if(carriedObject != null) {
            ContainedBasketIndicator indicator = carryableDetector.GetNearestBasketIndicator();
            if(indicator != null) {
                LaundromatBasket laundromatBasket = carriedObject.GetComponent<LaundromatBasket>();
                if(laundromatBasket != null) {
                    if (indicator.ReceiveBasket(laundromatBasket.basket)) {
                        DestroyCarriedObject();
                        AudioManager.instance.PlaySound(SoundName.DropBasket);
                    }
                    else {
                        DropCarriedObject();
                    }
                }
                else {
                    DropCarriedObject();
                }
            }
            else {
                DropCarriedObject();
            }
        }
    }

    private void Interact() {
        Interactable interactable = interactableDetector.GetNearestInteractable();
        if(interactable != null) {
            interactable.OnInteract();
        }
    }

    private void OnEnable() {
        WorkStation.RequestCarriedBasket += PutDown;
    }

    private void OnDisable() {
        WorkStation.RequestCarriedBasket -= PutDown;

        rb.velocity = Vector3.zero;
        state.EndWalk();
    }
}
