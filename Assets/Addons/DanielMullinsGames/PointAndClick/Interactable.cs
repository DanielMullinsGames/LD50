using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : ManagedBehaviour
{
    public virtual CursorType CursorType { get { return CursorType.Default; } }
    public bool CollisionEnabled
    {
        get
        {
            if (CollisionIs2D)
            {
                return GetComponent<Collider2D>().enabled;
            }
            else
            {
                return GetComponent<Collider>().enabled;
            }
        }
    }

    public System.Action<Interactable> CursorEntered { get; set; }
    public System.Action<Interactable> CursorExited { get; set; }
    public System.Action<Interactable> CursorSelectStarted { get; set; }
    public System.Action<Interactable> CursorSelectEnded { get; set; }

    protected virtual bool CollisionIs2D { get { return false; } }

    private Collider coll;
    private Collider2D coll2D;

    public void SetCollisionEnabled(bool enabled)
    {
        if (enabled && !CanEnable())
        {
            return;
        }

        if (coll2D == null && CollisionIs2D)
        {
            coll2D = GetComponent<Collider2D>();
        }
        if (coll == null && !CollisionIs2D)
        {
            coll = GetComponent<Collider>();
        }
        if (coll != null)
        {
            coll.enabled = enabled;
        }
        if (coll2D != null)
        {
            coll2D.enabled = enabled;
        }

        if (enabled)
        {
            OnInteractionEnabled();
        }
        else
        {
            OnInteractionDisabled();
        }
    }

    public void CursorSelectStart()
    {
        if (CursorSelectStarted != null)
        {
            CursorSelectStarted(this);
        }

        OnCursorSelectStart();
    }

    public void CursorSelectEnd()
    {
        if (CursorSelectEnded != null)
        {
            CursorSelectEnded(this);
        }

        OnCursorSelectEnd();
    }

    public void CursorEnter() 
    {
        OnCursorEnter();
    }

    public void CursorStay() 
    {
        OnCursorStay();
    }

    public void CursorExit() 
    {
        OnCursorExit();
    }

    public void CursorDragOff() 
    {

    }

    public virtual void ClearDelegates() { }

    protected virtual bool CanEnable() { return true; }
    protected virtual void OnInteractionEnabled() { }
    protected virtual void OnInteractionDisabled() { }
    protected virtual void OnCursorEnter() { }
    protected virtual void OnCursorStay() { }
    protected virtual void OnCursorExit() { }
    protected virtual void OnCursorSelectStart() { }
    protected virtual void OnCursorSelectEnd() { }
    protected virtual void OnCursorDrag() { }
}
