using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    BoxCollider2D boxCollider;
    Rigidbody2D rb2D;
    float inverseMoveTime;

	protected virtual void Start () {

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
		
	}

    protected bool Move(int xDir, int yDir, out RaycastHit2D Hit)
    {
        Vector2 Start = transform.position;
        Vector2 End = Start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        Hit = Physics2D.Linecast(Start, End, blockingLayer);
        boxCollider.enabled = true;

        if(Hit.transform==null)
        {
            StartCoroutine(SmoothMovement(End));
            return true;
        }

        return false;
    }
    
    protected IEnumerator SmoothMovement(Vector3 End)
    {
        float sqrRemainingDistance = (transform.position - End).sqrMagnitude;

        while(sqrRemainingDistance>float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, End, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - End).sqrMagnitude;
            yield return null;
        }
    }
    protected virtual void AttemptMove<T>(int xDir, int yDir)

        where T : Component
    {
        RaycastHit2D Hit;
        bool canMove = Move(xDir, yDir, out Hit);

        if (Hit.transform == null)
            return;

        T hitComponent = Hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }
	
    protected abstract void OnCantMove<T>(T Component)
    
        where T : Component;
    
}
