using System;
using Google.Maps.Examples.Shared;
using UnityEngine;

/// <summary>
/// A controller that allows cycling through a collection of camera rigs.
/// </summary>
public class CameraSelector : MonoBehaviour {
  /// <summary>
  /// The collection of camera rigs to cycle through.
  /// </summary>
  [Tooltip("The collection of camera rigs to cycle through.")]
  public SimpleViewController[] Cameras;

  /// <summary>
  /// Key used to cycle through camera rigs.
  /// </summary>
  [Tooltip("Key used to cycle through camera rigs.")]
  public KeyCode CycleKey;

  /// <summary>
  /// Index of the active camera rig.
  /// Can be set in inspector to choose initial camera.
  /// </summary>
  [Tooltip("Index of the active camera rig.")]
  public int CurrentIndex;

  void Start() {
    for (int i = 0; i < Cameras.Length; i++) {
      SetRigActive(i, i == CurrentIndex);
    }
  }

  /// <summary>
  /// Sets the enabled state of the indexed camera rig.
  /// </summary>
  /// <param name="index">Index of rig.</param>
  /// <param name="enabled">Enabled state to set.</param>
  void SetRigActive(int index, bool enabled) {
    Cameras[index].gameObject.SetActive(enabled);
  }

  /// <summary>
  /// Cycles the active camera rig to the next camera in <see cref="Cameras"/>.
  /// </summary>
  void CycleCameras() {
    SetRigActive(CurrentIndex, false);
    CurrentIndex = (CurrentIndex + 1) % Cameras.Length;
    SetRigActive(CurrentIndex, true);
  }

  void Update() {
    // Cycle rigs when appropriate key is pressed.
    if (Input.GetKeyDown(CycleKey)) {
      CycleCameras();
    }
  }
}
