using Google.Maps;
using Google.Maps.Coord;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller to update the Lat/Lng display in the HUD based on the world position of this
/// <see cref="GameObject"/> translated to a <see cref="LatLng"/> through the
/// <see cref="MapsService"/>.
/// </summary>
public class SateliteHudUpdater : MonoBehaviour {
  /// <summary>
  /// The <see cref="MapsService"/> used to translate world position to <see cref="LatLng"/>.
  /// </summary>
  [Tooltip("The MapsService used to translate world position to latlng.")]
  public MapsService MapsService;

  /// <summary>
  /// The <see cref="Text"/> UI element used to display <see cref="LatLng"/>.
  /// </summary>
  [Tooltip("The Text UI element used to display latlng")]
  public Text LatLngDisplay;

  void Update() {
    LatLng latlng = MapsService.Projection.FromVector3ToLatLng(transform.position);
    LatLngDisplay.text = string.Format("Lat/Lng: {0:F4},{1:F4}", latlng.Lat, latlng.Lng);
  }
}
