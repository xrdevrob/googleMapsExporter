using System.Collections;
using System.Collections.Generic;
using Google.Maps.Event;
using Google.Maps.Feature.Style;
using Google.Maps.Terrain;
using UnityEngine;

namespace Google.Maps.Examples {
  /// <summary>
  /// Example script for Maps SDK for Unity terrain.
  /// </summary>
  public class TerrainExample : MonoBehaviour {
    /// <summary>
    /// The target resolution of the alpha map in meters per pixel, used to draw
    /// <see cref="TerrainLayer"/> masks.
    /// </summary>
    [Tooltip(
        "The target resolution of the alpha map for each generated Terrain tile in meters per " +
        "pixel. This is the resolution at which TerrainLayer mask painting will occur.")]
    public float AlphaMapResolutionMetersPerPixel = 0.5f;

    /// <summary>
    /// The target resolution in meters per pixel of the composite texture used on the terrain when
    /// viewed from a distance greater than the Basemap distance.
    /// </summary>
    [Tooltip(
        "The target resolution in meters per pixel of the composite texture used on the " +
        "terrain when viewed from a distance greater than the basemap distance.")]
    public float BaseMapResolutionMetersPerPixel = 1f;

    /// <summary>
    /// The maximum distance at which terrain textures will be displayed at full resolution. Beyond
    /// this distance, a lower resolution composite map will be used for efficiency.
    /// </summary>
    [Tooltip(
        "The maximum distance at which terrain textures will be displayed at full resolution. " +
        "Beyond this distance, a lower resolution composite map will be used for efficiency.")]
    public float BaseMapDistance = 4000;

    /// <summary>
    /// <see cref="Material"/> used to paint the control texture from the feature mask.
    /// </summary>
    public Material ControlTexturePaintMaterial;

    /// <summary>
    /// <see cref="Material"/> used to render segments into the feature mask.
    /// </summary>
    public Material SegmentMaterial;

    /// <summary>
    /// <see cref="Material"/> used to render area water features into the feature mask.
    /// </summary>
    public Material AreaWaterMaterial;

    /// <summary>
    /// <see cref="Material"/> used to render region features into the feature mask.
    /// </summary>
    public Material RegionMaterial;

    /// <summary>
    /// <see cref="Material"/> used to render line water features into the feature mask.
    /// </summary>
    public Material LineWaterMaterial;

    /// <summary>
    /// <see cref="Material"/> used to render intersection features into the feature mask.
    /// </summary>
    public Material IntersectionMaterial;

    /// <summary>
    /// <see cref="Material"/> used to render buildings into the scene.
    /// </summary>
    public Material BuildingMaterial;

    /// <summary>
    /// Reference to <see cref="MapsService"/> component.
    /// </summary>
    private MapsService MapsService;

    /// <summary>
    /// <see cref="TerrainLayers"/> used to style generated terrain. The first layer is applied by
    /// default as the base styling layer.
    /// </summary>
    [Tooltip(
        "Terrain layers used to style generated terrain. The first layer is applied by " +
        "default as the base styling layer.")]
    public TerrainLayer[] TerrainLayers;

    /// <summary>
    /// Backing field for <see cref="M:GameObjectOptions"/> property.
    /// </summary>
    private GameObjectOptions _GameObjectOptions;

    /// <summary>
    /// Property providing the <see cref="T:GameObjectOptions"/> to use for this scene. Regenerated
    /// whenever a property on this component changes and stored for use until the properties
    /// change again.
    /// </summary>
    private GameObjectOptions GameObjectOptions {
      get {
        if (_GameObjectOptions == null) {
          _GameObjectOptions = CreateGameObjectOptions();
        }
        return _GameObjectOptions;
      }
    }

    /// <summary>
    /// Regenerate <see cref="M:GameObjectOptions"/> and trigger preview refresh when any of the
    /// properties on this component change.
    /// </summary>
    public void OnValidate() {
      _GameObjectOptions = null;
      gameObject.GetComponent<MapsService>().RefreshPreview(); // Needed for edit mode
    }

    /// <summary>
    /// Returns a new <see cref="GameObjectOptions"/> instance reflecting the properties on
    /// this component.
    /// </summary>
    private GameObjectOptions CreateGameObjectOptions() {
      GameObjectOptions gameObjectOptions = new GameObjectOptions();

      // Terrain style
      TerrainStyle.Builder terrainStyleBuilder = new TerrainStyle.Builder() {
          AlphaMapResolutionMetersPerPixel = AlphaMapResolutionMetersPerPixel,
          BaseMapResolutionMetersPerPixel = BaseMapResolutionMetersPerPixel,
          BaseMapDistance = BaseMapDistance,
          TerrainLayers = new List<TerrainLayer>(TerrainLayers),
      };
      gameObjectOptions.TerrainStyle = terrainStyleBuilder.Build();

      // Segment style
      SegmentStyle.Builder segmentStyleBuilder = new SegmentStyle.Builder() {
          Material = SegmentMaterial,
          IntersectionMaterial = IntersectionMaterial,
          GameObjectLayer = gameObjectOptions.TerrainStyle.TerrainPaintingLayer
      };
      gameObjectOptions.SegmentStyle = segmentStyleBuilder.Build();

      // Area water style
      AreaWaterStyle.Builder areaWaterStyleBuilder = new AreaWaterStyle.Builder() {
          FillMaterial = AreaWaterMaterial,
          GameObjectLayer = gameObjectOptions.TerrainStyle.TerrainPaintingLayer
      };
      gameObjectOptions.AreaWaterStyle = areaWaterStyleBuilder.Build();

      // Region style
      RegionStyle.Builder regionStyleBuilder = new RegionStyle.Builder() {
          FillMaterial = RegionMaterial,
          GameObjectLayer = gameObjectOptions.TerrainStyle.TerrainPaintingLayer
      };
      gameObjectOptions.RegionStyle = regionStyleBuilder.Build();

      // Line water style
      LineWaterStyle.Builder lineWaterStyleBuilder = new LineWaterStyle.Builder() {
          Material = LineWaterMaterial,
          GameObjectLayer = gameObjectOptions.TerrainStyle.TerrainPaintingLayer
      };
      gameObjectOptions.LineWaterStyle = lineWaterStyleBuilder.Build();

      // Extruded structure style
      ExtrudedStructureStyle.Builder extrudedStructureStyleBuilder =
          new ExtrudedStructureStyle.Builder() {
              RoofMaterial = BuildingMaterial,
              WallMaterial = BuildingMaterial
          };
      gameObjectOptions.ExtrudedStructureStyle = extrudedStructureStyleBuilder.Build();

      // Modeled structure style
      ModeledStructureStyle.Builder modeledStructureStyleBuilder =
          new ModeledStructureStyle.Builder() {
              Material = BuildingMaterial
          };
      gameObjectOptions.ModeledStructureStyle = modeledStructureStyleBuilder.Build();

      return gameObjectOptions;
    }

    /// <summary>
    /// Coroutine to paint control texture.
    /// </summary>
    /// <param name="args">Arguments from painting event</param>.
    private IEnumerator PaintControlTexture(AlphaMapsNeedPaintArgs args) {
      // Get a temporary render texture to be painted onto.
      Texture2D destinationTexture = args.Terrain.terrainData.GetAlphamapTexture(0);
      RenderTextureDescriptor renderTextureDescriptor = new RenderTextureDescriptor(
          destinationTexture.width, destinationTexture.height, RenderTextureFormat.ARGB32, 0);
      RenderTexture temporaryRenderTexture = RenderTexture.GetTemporary(renderTextureDescriptor);
      args.RegisterFinalizer(delegate {
        RenderTexture.ReleaseTemporary(temporaryRenderTexture);
      });

      // Yield to allow work to be split across frames if necessary.
      yield return null;

      // Make the active render texture our temporary one (storing the current one for restoration
      // once we're done).
      //
      // Note that this is done before Graphics.Blit, which appears to set RenderTexture.active as
      // well, but this doesn't appear to be documented behavior so isn't relied upon.
      // https://docs.unity3d.com/ScriptReference/Graphics.Blit.html
      RenderTexture renderTextureToRestore = RenderTexture.active;
      RenderTexture.active = temporaryRenderTexture;

      // Paint the control texture from the feature mask using the configured material/shader.
      Graphics.Blit(
          args.FeatureMaskRenderTexture,
          temporaryRenderTexture,
          ControlTexturePaintMaterial);

      destinationTexture.ReadPixels(
          new Rect(0, 0, destinationTexture.width, destinationTexture.height), 0, 0);
      destinationTexture.Apply();
      args.Terrain.terrainData.SetBaseMapDirty();

      // Restore the previously active render texture.
      RenderTexture.active = renderTextureToRestore;
    }

    /// <summary>
    /// Handle <see cref="TerrainEvents.AlphaMapsNeedPaint"/> event by specifying setting the
    /// painting coroutine.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleAlphaMapsNeedPaint(AlphaMapsNeedPaintArgs args) {
      args.PaintingCoroutine = PaintControlTexture(args);
    }

    /// <summary>
    /// Handle <see cref="TerrainEvents.WillCreate"/> event by specifying the terrain styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateTerrain(WillCreateTerrainArgs args) {
      args.Style = GameObjectOptions.TerrainStyle;
    }

    /// <summary>
    /// Handle <see cref="SegmentEvents.WillCreate"/> event by specifying the segment styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateSegment(WillCreateSegmentArgs args) {
      args.Style = GameObjectOptions.SegmentStyle;
    }

    /// <summary>
    /// Handle <see cref="IntersectionEvents.WillCreate"/> event by specifying the intersection
    /// styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateIntersection(WillCreateIntersectionArgs args) {
      args.Style = GameObjectOptions.SegmentStyle;
    }

    /// <summary>
    /// Handle <see cref="AreaWaterEvents.WillCreate"/> event by specifying the area water styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateAreaWater(WillCreateAreaWaterArgs args) {
      args.Style = GameObjectOptions.AreaWaterStyle;
    }

    /// <summary>
    /// Handle <see cref="RegionEvents.WillCreate"/> event by specifying the region styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateRegion(WillCreateRegionArgs args) {
      args.Style = GameObjectOptions.RegionStyle;
    }

    /// <summary>
    /// Handle <see cref="LineWaterEvents.WillCreate"/> event by specifying the line water styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateLineWater(WillCreateLineWaterArgs args) {
      args.Style = GameObjectOptions.LineWaterStyle;
    }

    /// <summary>
    /// Handle <see cref="ExtrudedStructureEvents.WillCreate"/> event by specifying the extruded
    /// structure styles.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateExtrudedStructure(WillCreateExtrudedStructureArgs args) {
      args.Style = GameObjectOptions.ExtrudedStructureStyle;
    }

    /// <summary>
    /// Handle <see cref="ModeledStructureEvents.WillCreate"/> event by specifying the modeled
    /// structure styles.
    /// </summary>
    /// <param name="args"></param>
    public void HandleWillCreateModeledStructure(WillCreateModeledStructureArgs args) {
      args.Style = GameObjectOptions.ModeledStructureStyle;
    }

    /// <summary>
    /// Check if we have moved the camera to above the ground level. We only do this once, when
    /// first loading the scene.
    /// </summary>
    private bool HasMovedCamera = false;

    /// <summary>
    /// Handle <see cref="TerrainEvents.DidCreate"/> event by moving the camera above the terrain
    /// when the first terrain tile is loaded.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleDidCreateTerrain(DidCreateTerrainArgs args) {
        if (!HasMovedCamera)
        {
            // Work out the height of the center of this tile.
            float height = args.Terrain.GetPosition().y +
                           args.Terrain.terrainData.GetInterpolatedHeight(0.5f, 0.5f);
            if (height > 0 && Camera.main != null)
            {
                // Move the camera up by this amount to ensure it is above ground level.
                Camera.main.transform.Translate(0, height, 0, Space.World);
                HasMovedCamera = true;
            }
        }
    }

    /// <summary>
    /// Handle <see cref="TerrainEvents.FeatureMaskPreRender"/> event by enabling MSAA for
    /// feature mask rendering.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleFeatureMaskPreRender(FeatureMaskPreRenderArgs args) {
      args.FeatureMaskCamera.allowMSAA = true;
    }

    /// <summary>
    /// Handle <see cref="TerrainEvents.WillCreateFeatureMaskTexture"/> by setting the
    /// anti-aliasing level to 4x.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleWillCreateFeatureMaskTexture(WillCreateFeatureMaskTextureArgs args) {
      args.FeatureMaskRenderTextureDescriptor.msaaSamples = 4;
    }
  }
}
