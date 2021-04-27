using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Examples.Shared;
using Google.Maps.Feature.Style.Settings;
using UnityEngine;

namespace Google.Maps.Examples {
  /// <summary>
  /// This example demonstrates a basic usage of the Maps SDK for Unity with the Lightweight
  /// Render Pipeline.
  /// </summary>
  [RequireComponent(typeof(MapsService))]
  public class URPExample : MonoBehaviour {
    /// <summary>
    /// Settings used to style regions.
    /// </summary>
    [Tooltip("Settings used to style regions.")]
    public RegionStyleSettings RegionStyleSettings;

    /// <summary>
    /// Settings used to style roads.
    /// </summary>
    [Tooltip("Settings used to style roads.")]
    public SegmentStyleSettings RoadStyleSettings;

    /// <summary>
    /// Settings used to style are water.
    /// </summary>
    [Tooltip("Settings used to style area water.")]
    public AreaWaterStyleSettings WaterStyleSettings;

    /// <summary>
    /// Settings used to style extruded structures.
    /// </summary>
    [Tooltip("Settings used to style extruded structures.")]
    public ExtrudedStructureStyleSettings ExtrudedStructureStyleSettings;

    /// <summary>
    /// Settings used to style modeled structures.
    /// </summary>
    [Tooltip("Settings used to style modeled structures.")]
    public ModeledStructureStyleSettings ModeledStructureStyleSettings;

    /// <summary>
    /// <see cref="LatLng"/> of the initial load position.
    /// </summary>
    [Tooltip("LatLng to load (must be set before hitting play).")]
    public LatLng LatLng = new LatLng(40.6892199, -74.044601);

    /// <summary>
    /// Load range, in meters, of map around <see cref="LatLng"/> above.
    /// </summary>
    [Tooltip("Map load range in meters.")]
    public float LoadRange = 500;

    /// <summary>
    /// Use <see cref="MapsService"/> to load geometry.
    /// </summary>
    private void Start() {
      // Get required MapsService component on this GameObject.
      MapsService mapsService = GetComponent<MapsService>();

      // Set real-world location to load.
      mapsService.InitFloatingOrigin(LatLng);

      // Configure Map Styling.
      GameObjectOptions options = new GameObjectOptions();
      options.RegionStyle =
        RegionStyleSettings.Apply(options.RegionStyle);
      options.SegmentStyle =
        RoadStyleSettings.Apply(options.SegmentStyle);
      options.AreaWaterStyle =
        WaterStyleSettings.Apply(options.AreaWaterStyle);
      options.ExtrudedStructureStyle =
        ExtrudedStructureStyleSettings.Apply(options.ExtrudedStructureStyle);
      options.ModeledStructureStyle =
        ModeledStructureStyleSettings.Apply(options.ModeledStructureStyle);
      // Load map with default options.
      Bounds bounds = new Bounds(Vector3.zero, Vector3.one * LoadRange);
      mapsService.LoadMap(bounds, options);
    }
  }
}
