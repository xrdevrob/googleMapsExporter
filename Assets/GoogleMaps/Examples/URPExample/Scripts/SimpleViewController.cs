using System;
using UnityEngine;
using UnityEngine.Events;

namespace Google.Maps.Examples.Shared {
  /// <summary>
  /// A simple controller, to allow user-controlled movement of a view aspect (typically a rig
  /// object to which a <see cref="Camera"/> is attached).
  /// Movement is performed via WASD keys (transverse), QE (up/down) and arrow keys (rotation).
  /// Speed of movement is higher when the controller is at higher altitude to make traversing a
  /// map more practical.
  /// </summary>
  public class SimpleViewController : MonoBehaviour {
    /// <summary>
    /// Max movement speed when pressing movement keys (WASD for panning, QE for up/down).
    /// </summary>
    [Tooltip("Max movement speed when pressing movement keys (WASD for panning, QE for up/down).")]
    public float MaxSpeed = 200f;

    /// <summary>
    /// Min movement speed when pressing movement keys (WASD for panning, QE for up/down).
    /// </summary>
    [Tooltip("Min movement speed when pressing movement keys (WASD for panning, QE for up/down).")]
    public float MinSpeed = 50f;

    /// <summary>
    /// Rotation speed when pressing arrow keys.
    /// </summary>
    [Tooltip("Rotation speed when pressing arrow keys.")]
    public float RotationSpeed = 100f;

    /// <summary>
    /// Minimum height off the ground.
    /// </summary>
    [Tooltip("Minimum height off the ground.")]
    public float MinAltitude = 2f;

    /// <summary>
    /// Maximum height off the ground.
    /// </summary>
    [Tooltip("Maximum height off the ground.")]
    public float MaxAltitude = 600f;

    /// <summary>
    /// Minimum angle relative to the ground.
    /// </summary>
    [Tooltip("Minimum angle above ground.")]
    public float MinInclination = 0;

    /// <summary>
    /// Maximum angle relative to the ground.
    /// </summary>
    [Tooltip("Maximum angle above ground.")]
    public float MaxInclination = 90;

    /// <summary>
    /// The current desired rotation of the Camera around the Y-Axis. Applied in world space after
    /// Inclination is applied.
    /// </summary>
    public float Azimuth;

    /// <summary>
    /// The current desired rotation of the Camera around the X-Axis. Applied in world space before
    /// Azimuth is applied.
    /// </summary>
    public float Inclination;

    private void Update() {
      Vector3 positionDelta = Vector3.zero;
      Vector3 rotationDelta = Vector3.zero;

      // ASWD keys used for position
      if (Input.GetKey(KeyCode.A)) {
        positionDelta.x -= 1.0f;
      }

      if (Input.GetKey(KeyCode.D)) {
        positionDelta.x += 1.0f;
      }

      if (Input.GetKey(KeyCode.W)) {
        positionDelta.z += 1.0f;
      }

      if (Input.GetKey(KeyCode.S)) {
        positionDelta.z -= 1.0f;
      }

      // QE keys used for altitude.
      if (Input.GetKey(KeyCode.Q)) {
        positionDelta.y -= 1.0f;
      }

      if (Input.GetKey(KeyCode.E)) {
        positionDelta.y += 1.0f;
      }

      // Arrow keys used to change camera orientation.
      if (Input.GetKey(KeyCode.LeftArrow)) {
        rotationDelta.x -= 1.0f;
      }

      if (Input.GetKey(KeyCode.RightArrow)) {
        rotationDelta.x += 1.0f;
      }

      if (Input.GetKey(KeyCode.UpArrow)) {
        rotationDelta.y -= 1.0f;
      }

      if (Input.GetKey(KeyCode.DownArrow)) {
        rotationDelta.y += 1.0f;
      }

      // Calculate the new Azimuth and Inclination.
      Azimuth += rotationDelta.x * RotationSpeed * Time.deltaTime;
      Azimuth = Azimuth % 360.0f;
      Inclination += rotationDelta.y * RotationSpeed * Time.deltaTime;
      Inclination = Mathf.Clamp(Inclination, MinInclination, MaxInclination);

      // Speed is affected linearly by the current altitude, and clamped to min/max range.
      float speed = Mathf.Clamp(transform.position.y, MinSpeed, MaxSpeed);

      // Calculate the current forward and right directions from the Azimuth and Inclination.
      Vector3 forward = Quaternion.Euler(0, Azimuth, 0) * Vector3.forward;
      Vector3 right = Quaternion.Euler(0, Azimuth, 0) * Vector3.right;

      // Combine 3 direction axes into a single motion vector.
      // Note: Up and down are always absolute, not affected by current orientation.
      Vector3 direction =
          (right * positionDelta.x + forward * positionDelta.z + positionDelta.y * Vector3.up);
      Vector3 motion = direction * speed * Time.deltaTime;
      Vector3 position = transform.position + motion;

      // Set current transform rotation using the Inclination and Azimuth.
      Quaternion rotation = Quaternion.Euler(Inclination, Azimuth, 0);

      // Enforce min/max height.
      position.y = Mathf.Clamp(position.y, MinAltitude, MaxAltitude);
      transform.position = position;
      transform.rotation = rotation;
    }
  }
}

