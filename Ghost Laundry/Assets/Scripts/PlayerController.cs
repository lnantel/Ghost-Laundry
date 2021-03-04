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
    private GameObject laundromatBasketPrefab;

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

        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");
        playerSortingGroup = GetComponentInChildren<SortingGroup>();
    }

    void Update()
    {
        if (Time.timeScale != 0) {
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
                if (carriedPos.localPosition.x > 0.0f != facingRight) carriedPos.localPosition = new Vector3(-carriedPos.localPosition.x, carriedPos.localPosition.y, carriedPos.localPosition.z);
                carriedObject.transform.position = carriedPos.position;
                carriedObject.transform.rotation = carriedPos.rotation;
                carriedObjectSortingGroup.sortingOrder = playerSortingGroup.sortingOrder + 1;
            }

            if (input.GetInteractInput()) {
                Interact();
            }
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
            state.EndWalk();
            //Decrease acceleration factor
            accelerationFactor = Mathf.Max(accelerationFactor - Time.deltaTime / m_AccelerationTime, 0.0f);
        }

        //Facing
        if (moveDir.x > 0.01f) facingRight = true;
        else if (moveDir.x < -0.01f) facingRight = false;

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
            dashTimer += Time.deltaTime;
            yield return new WaitForSeconds(0.0f);
        }

        if (collision) {
            AudioManager.instance.PlaySoundAtPosition(Sounds.Collision, collisionPoint, 1.0f);
            Vector2 reflected = Vector2.Reflect(dashDir, hit.normal);
            dashEndPos = collisionPoint + reflected * m_DashReboundDistance;
            float reboundTimer = 0.0f;
            while(reboundTimer < m_DashReboundDuration) {
                rb.MovePosition(Vector2.Lerp(collisionPoint, dashEndPos, m_DashCurve.Evaluate(reboundTimer / m_DashReboundDuration)));
                reboundTimer += Time.deltaTime;
                yield return new WaitForSeconds(0.0f);
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
        carriedObject = obj;
        carriedObject.GetComponent<Collider2D>().enabled = false;
        LaundromatSpriteSort spriteSort = obj.GetComponentInChildren<LaundromatSpriteSort>();
        if (spriteSort != null) {
            spriteSort.enabled = false;
            carriedObjectSortingGroup = obj.GetComponentInChildren<SortingGroup>();
        }
    }

    private void DropCarriedObject() {
        state.EndCarry();
        carriedObject.GetComponent<Collider2D>().enabled = true;
        LaundromatSpriteSort spriteSort = carriedObject.GetComponentInChildren<LaundromatSpriteSort>();
        if (spriteSort != null) {
            spriteSort.enabled = true;
        }
        carriedObject = null;
    }

    private void DestroyCarriedObject() {
        state.EndCarry();
        Destroy(carriedObject);
        carriedObject = null;
    }

    private void PickUp() {
        GameObject targetedCarryable = carryableDetector.GetNearestCarryable();
        if (targetedCarryable != null) {
            StartCarrying(targetedCarryable);
            LaundromatBasket laundromatBasket = targetedCarryable.GetComponent<LaundromatBasket>();
            if (laundromatBasket != null) {
                if(CustomerManager.CustomerServed != null) CustomerManager.CustomerServed(laundromatBasket);
            }
        }
        else {
            //Pick up a basket from a work station
            Interactable interactable = interactableDetector.GetNearestInteractable();
            if(interactable != null) {
                if(interactable is WorkStation) {
                    WorkStation workStation = (WorkStation)interactable;
                    if(workStation.ContainsBasket()) {
                        Basket basket = workStation.OutputBasket();
                        GameObject basketObject = Instantiate(laundromatBasketPrefab, transform.position, transform.rotation);
                        basketObject.GetComponent<LaundromatBasket>().basket = basket;

                        StartCarrying(basketObject);
                    }
                }
            }
        }
    }

    public void Take(GameObject obj) {
        if (carriedObject != null) DropCarriedObject();
        StartCarrying(obj);
        LaundromatBasket laundromatBasket = obj.GetComponent<LaundromatBasket>();
        if (laundromatBasket != null) {
            if (CustomerManager.CustomerServed != null) CustomerManager.CustomerServed(laundromatBasket);
        }
    }

    private void PutDown() {
        if(carriedObject != null) {
            Interactable interactable = interactableDetector.GetNearestInteractable();
            if (interactable != null) {
                LaundromatBasket laundromatBasket = carriedObject.GetComponent<LaundromatBasket>();
                if (laundromatBasket != null && interactable is WorkStation) {
                    WorkStation workStation = (WorkStation)interactable;
                    if (workStation.InputBasket(laundromatBasket.basket)) {
                        DestroyCarriedObject();
                    }
                    else {
                        DropCarriedObject();
                        Debug.Log("Could not input basket");
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
            interactable.Interact();
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
