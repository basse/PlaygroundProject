using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowWithButtons : Physics2DObject
{
	[Header("Input keys")]

	// the key used to activate the push
	public KeyCode launchKey = KeyCode.Space;
	public Enums.KeyGroups spinControl = Enums.KeyGroups.ArrowKeys;

	[Header("Direction and strength")]

	// strength of the push, and the axis on which it is applied (can be X or Y)
	public float launchForceStrength = 0.2f;
	public float torqueStrength = 0.1f;
	private float torque;
	private float launchForceMagnitude;
	private bool launchKeyPressed;
	private bool launch;



	// Read the input from the player
	void Update()
	{
		var launchInput = Input.GetKey(launchKey);
		if (launchInput != false) {
			launchKeyPressed = true;
			launchForceMagnitude += launchForceStrength;
		} else if (launchKeyPressed) {
			launch = true;
			launchKeyPressed = false;
		} else {
			launchForceMagnitude = 0;
		}

		var torqueInput = (spinControl == Enums.KeyGroups.ArrowKeys) ? Input.GetAxis ("Horizontal") : Input.GetAxis ("Horizontal2");
		if (Mathf.Abs(torqueInput) < Mathf.Epsilon) {
			torque = 0;
		} else {
			torque += torqueInput * torqueStrength;
		}
	}


	// FixedUpdate is called every frame when the physics are calculated
	void FixedUpdate() {
		if (launch) {
			var v = new Vector2(torque * 0.05f, 1).normalized;
			rigidbody2D.AddForce(v * launchForceMagnitude, ForceMode2D.Impulse);
			rigidbody2D.AddTorque(-torque, ForceMode2D.Impulse);
			launchForceMagnitude = 0;
			torque = 0;
			// launch
			launch = false;
		}
	}
}
