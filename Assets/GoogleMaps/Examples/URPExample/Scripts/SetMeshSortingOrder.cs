using UnityEngine;

/// <summary>
/// A simple component to set the MeshRenderer.sortingOrder on Start.
/// The SortingGroup component used in other GoogleMaps Standard Pipeline examples does not work
/// with the Lightweight Render Pipeline, so we need to add a script to explicitly set the
/// <see cref="MeshRenderer.sortingOrder"/>.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class SetMeshSortingOrder : MonoBehaviour {
  // The Sorting Order. Set once at Start
  [Tooltip("MeshRenderer.sortingOrder value. Set once at Start")]
  public int SortingOrder;

  void Start() {
    MeshRenderer renderer = GetComponent<MeshRenderer>();
    renderer.sortingOrder = SortingOrder;
  }
}
