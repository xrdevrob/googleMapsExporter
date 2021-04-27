using Google.Maps.Examples.Shared;
using UnityEngine;

/// <summary>
/// A controller that updates some onscreen HUD elements based on the status of a
/// SimpleViewController on a camera rig.
/// </summary>
public class CompassAltimeterUpdater : MonoBehaviour {
  /// <summary>
  /// The <see cref="SimpleViewController"/> from which to get altitude and Azimuth.
  /// </summary>
  [Tooltip("The controller from which to get altitude and Azimuth.")]
  public SimpleViewController SimpleViewController;

  /// <summary>
  /// <see cref="Compass"/> object to update based on Azimuth.
  /// </summary>
  [Tooltip("Compass object to update based on Azimuth.")]
  public GameObject Compass;

  /// <summary>
  /// <see cref="Altimeter"/> object to update based on transform.y of the SimpleViewController.
  /// </summary>
  [Tooltip("Altimeter object to update based on transform.y of the SimpleViewController.")]
  public GameObject Altimeter;

  /// <summary>
  /// Movement scale used when updating Altimeter object.
  /// </summary>
  /// <remarks>
  /// This value is configured based on the unity world scale size of the altimeter object. The
  /// current altitude (position.y of <see cref="SimpleViewController"/>) is multiplied by this
  /// scale factor to get the calibrated vertical movement of the altitude strip.
  /// </remarks>
  [Tooltip("Movement scale used when updating Altimeter object.")]
  public float AltimeterScale = 4.0f;

  void Update() {
    // Rotate the compass to align with Azimuth.
    if (Compass != null) {
      Compass.transform.localRotation =
          Quaternion.Euler(0, 0, SimpleViewController.Azimuth - 180);
    }

    // Move Altimeter based on y position of SimpleViewController and AltimeterScale.
    if (Altimeter != null) {
      float altHeight = -SimpleViewController.transform.position.y * AltimeterScale;
      Altimeter.transform.localPosition = new Vector3(0, altHeight, 0);
    }
  }
}
