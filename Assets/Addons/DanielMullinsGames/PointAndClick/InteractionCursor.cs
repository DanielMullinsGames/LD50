using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum CursorType
{
    Default,
    NUM_CURSOR_TYPES,
}

public class InteractionCursor : Singleton<InteractionCursor>
{
    public bool OSCursorShown { get; set; }

    public bool CursorTypeIsForced { get { return forceCursorType; } }
    private bool forceCursorType;

    public CursorType CursorType { get; private set; }

    public Interactable CurrentInteractable => currentInteractable;
    private Interactable currentInteractable;
    private Interactable cursorDownInteractable;

    public ReferenceSetToggle DisableInput = new ReferenceSetToggle();
    private bool inputWasDisabled = false;

    public ReferenceSetToggle HideCursor = new ReferenceSetToggle();
    private bool cursorWasHidden = false;

    [SerializeField]
    private Camera rayCamera = default;

    [SerializeField]
    private CursorDisplayer displayer = default;

    [SerializeField]
    private CursorType defaultCursorType = default;

    private Vector2 pullPoint;
    private float pullStrength;

    private List<string> excludedLayers = new List<string>
    {
        "NonInteractable",
    };

    private void Start()
    {
        SetCursorType(defaultCursorType);
    }

    public override void ManagedUpdate()
    {
        UpdateState();
        UpdatePosition();
        displayer.SetCursorDown(Input.GetMouseButton(0), CursorType);
        UpdateMainInput();
        UpdateDragInput();
    }

    public void SetHidden(bool hidden)
    {
        if (hidden)
        {
            gameObject.SetActive(false);
            Cursor.visible = false;
        }
        else
        {
            gameObject.SetActive(true);
            ManagedFixedUpdate();
            ManagedUpdate();
        }
    }

    public void ForceCursorType(CursorType type)
    {
        ClearForcedCursorType();
        SetCursorType(type);
        forceCursorType = true;
    }

    public void ClearForcedCursorType()
    {
        forceCursorType = false;
    }

    public void SetCursorPullPoint(Vector2 point, float strength)
    {
        pullPoint = point;
        pullStrength = strength;
    }

    public void ResetPullPoint()
    {
        pullStrength = 0f;
    }

    public void UpdatePosition()
    {
        Vector2 mousePos = Input.mousePosition;

        Cursor.visible = OSCursorShown;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
#endif
        float heightModifier = ReferenceResolutionSetter.Instance.ReferenceHeight / (float)Screen.height;
        float widthModifier = ReferenceResolutionSetter.Instance.ReferenceWidth / (float)Screen.width;
        Vector2 screenPoint = new Vector2(mousePos.x * widthModifier, mousePos.y * heightModifier);
        Vector2 cursorPos = rayCamera.ScreenToWorldPoint(screenPoint);
        cursorPos = Vector2.Lerp(cursorPos, pullPoint, pullStrength);
        transform.position = new Vector3(cursorPos.x, cursorPos.y, 0f);
    }

    private void SetCursorType(CursorType type)
    {
        if (!forceCursorType)
        {
            CursorType = type;
        }
    }

    private void UpdateState()
    {
        if (cursorWasHidden != HideCursor.True)
        {
            SetHidden(HideCursor.True);
        }
        cursorWasHidden = HideCursor.True;

        if (!inputWasDisabled && DisableInput.True)
        {
            if (currentInteractable != null)
            {
                currentInteractable.CursorExit();
                currentInteractable = null;
            }
            SetCursorType(defaultCursorType);
        }
        inputWasDisabled = DisableInput.True;
    }

    private void UpdateMainInput()
    {
        currentInteractable = UpdateCurrentInteractable(currentInteractable, excludedLayers.ToArray());

        if (!DisableInput.True)
        {
            if (currentInteractable != null)
            {
                currentInteractable.CursorStay();
                SetCursorType(currentInteractable.CursorType);
            }
            else
            {
                SetCursorType(defaultCursorType);
            }

            if (currentInteractable != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentInteractable.CursorSelectStart();
                    cursorDownInteractable = currentInteractable;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    currentInteractable.CursorSelectEnd();
                }
            }
        }
    }

    private void UpdateDragInput()
    {
        if (cursorDownInteractable != null && !DisableInput.True)
        {
            if (currentInteractable != cursorDownInteractable)
            {
                cursorDownInteractable.CursorDragOff();
                cursorDownInteractable = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            cursorDownInteractable = null;
        }
    }

    private Interactable UpdateCurrentInteractable(Interactable current, string[] excludeLayers)
    {
        var hitInteractable = RaycastForInteractable(~LayerMask.GetMask(excludeLayers), transform.position);

        if (hitInteractable != current)
        {
            if (current != null)
            {
                if (current.CollisionEnabled)
                {
                    current.CursorExit();
                }
            }

            if (hitInteractable != null && !DisableInput.True)
            {
                hitInteractable.CursorEnter();
            }
            else
            {
                return null;
            }
        }

        return hitInteractable;
    }

    // NOTE: ONLY applies to 2D interaction for now.
    private Interactable RaycastForInteractable(int layerMask, Vector3 cursorPosition)
    {
        Interactable hitInteractable = null;

        var rayHits = Physics2D.RaycastAll(cursorPosition, Vector2.zero, 1000f, layerMask);
        if (rayHits.Length > 0)
        {
            var hitInteractables = GetInteractablesFromRayHits(rayHits);
            if (hitInteractables.Count > 0)
            {
                hitInteractables.Sort((Interactable2D a, Interactable2D b) =>
                {
                    return b.SortOrder - a.SortOrder;
                });
                hitInteractable = hitInteractables[0];
            }
        }
        return hitInteractable;
    }

    private List<Interactable2D> GetInteractablesFromRayHits(RaycastHit2D[] rayHits)
    {
        var hitInteractables = new List<Interactable2D>();
        for (int i = 0; i < rayHits.Length; i++)
        {
            var interactable = rayHits[i].transform.GetComponent<Interactable2D>();
            if (interactable != null)
            {
                hitInteractables.Add(interactable);
            }
        }
        return hitInteractables;
    }
}
