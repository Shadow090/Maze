using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float distance = 1f;
	private bool readyToMove = true;

	private RaycastHit2D rightHit;
	private RaycastHit2D forwardHit;
	private RaycastHit2D leftHit;

	public GameObject panel;

	private void CastRays()
	{
		LayerMask layerMask = LayerMask.GetMask("Maze");
	
		rightHit = Physics2D.Raycast(transform.position, transform.right, distance, layerMask);
		forwardHit = Physics2D.Raycast(transform.position, transform.up, distance, layerMask);
		leftHit = Physics2D.Raycast(transform.position, -transform.right, distance, layerMask);

		Debug.Log(rightHit.collider);
		Debug.Log(forwardHit.collider);
		Debug.Log(leftHit.collider);
	}

    void Update()
    {
        if (readyToMove)
        {
		StartCoroutine(Move());
        }
    }

	private IEnumerator Move()
	{
		//Wall follower, Right-hand Rule

		readyToMove = false;


		CastRays();

		if (rightHit.collider == null) //the right laser detected nothing to the right, meaning it is open
		{
			transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self); //turning right

			yield return new WaitForSeconds(1);

			CastRays();

			if (forwardHit.collider == null)
			{
				transform.Translate(Vector2.up);
			}
		}

		else if (forwardHit.collider == null) // the front laser detected nothing in front
		{	
			transform.Translate(Vector2.up); //going forward
		}

		else if (leftHit.collider == null) //the left laser detected nothing to the left
		{
			transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self); //turning left

			yield return new WaitForSeconds(1);

			CastRays();

			if (forwardHit.collider == null)
			{
				transform.Translate(Vector2.up); //Vector is what direction 
			}
		}

		else //turn around
		{
			transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);	

			yield return new WaitForSeconds(1);

			CastRays();

			if (forwardHit.collider == null)
			{
				transform.Translate(Vector2.up);
			}
		}
		yield return new WaitForSeconds(1);

		readyToMove = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<BoxCollider2D>() != null)
		{
			panel.SetActive(true);
			Time.timeScale = 0f;
		}
	}
}