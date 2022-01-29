using UnityEngine;

public class GroundCheckModel : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform startPosition;

    [Header("Tilemap Grid Dimensions")]
    [SerializeField] int gridWidth = 32;
    [SerializeField] int gridHeight = 18;

    [Space]
    [SerializeField] int textureGranularity = 4;

    [Header("Texture Drawing Settings")]
    [SerializeField] Vector2Int extrudeWidth;
    [SerializeField] Vector2Int extrudeHeight;

    [Header("Debug")]
    [SerializeField] bool drawTextureGUI;
    
    public Texture2D groundCheckTexture { get; private set; }

    int textureWidth;
    int textureHeight;

    [SerializeField] Vector2 cameraMin;
    [SerializeField] Vector2 cameraMax;

    Vector2Int prevCoords;
    
    public bool IsDeadly(Vector2 position, PlayerType playerType)
    {
        var coords = CalculateTextureCoordinates(position);
        var color = groundCheckTexture.GetPixel(coords.x, coords.y);

        return color.IsDeadly(playerType);
    }

    public void UpdateGround(Vector2 position, PlayerType playerType, bool continuous)
    {
        var coords = CalculateTextureCoordinates(position);

        if (coords == prevCoords) {
            return;
        }

        var baseRect = continuous ? GetBaseDimensions(coords) : GetBasePoint(coords);
        var drawingRect = GetExtendedDimensions(baseRect);

        DrawRectangleToTexture(drawingRect, playerType.ToColor32());

        prevCoords = coords;
    }

    protected void Start()
    {
        CreateAndClearTexture();

        prevCoords = CalculateTextureCoordinates(startPosition.position);
    }

    protected void OnGUI()
    {
        if (!drawTextureGUI) {
            return;
        }

        GUI.DrawTexture(new Rect(0.0f, 0.0f, textureWidth, textureHeight), groundCheckTexture);
    }

    private RectInt GetExtendedDimensions(RectInt rect)
    {
        var x = Mathf.Max(rect.x - extrudeWidth.x, 0);
        var y = Mathf.Max(rect.y - extrudeHeight.x, 0);
        var xMax = Mathf.Min(rect.xMax + extrudeWidth.y, textureWidth - 1);
        var yMax = Mathf.Min(rect.yMax + extrudeHeight.y, textureHeight - 1);

        return new RectInt(x, y, xMax - x, yMax - y);
    }

    private RectInt GetBaseDimensions(Vector2Int coords)
    {
        var startX = prevCoords.x < coords.x ? prevCoords.x : coords.x;
        var startY = prevCoords.y < coords.y ? prevCoords.y : coords.y;
        var endX = prevCoords.x < coords.x ? coords.x : prevCoords.x;
        var endY = prevCoords.y < coords.y ? coords.y : prevCoords.y;

        return new RectInt(startX, startY, endX - startX, endY - startY);
    }

    private RectInt GetBasePoint(Vector2Int coords)
    {
        return new RectInt(coords.x, coords.y, 1, 1);
    }

    private void DrawRectangleToTexture(RectInt rect, Color32 color)
    {
        var colors = new Color32[rect.width * rect.height];
        for (var i = 0; i < rect.width * rect.height; i++) {
            colors[i] = color;
        }

        groundCheckTexture.SetPixels32(rect.x, rect.y, rect.width, rect.height, colors);

        groundCheckTexture.Apply();
    }

    private void CreateAndClearTexture()
    {
        textureWidth = gridWidth * textureGranularity;
        textureHeight = gridHeight * textureGranularity;

        groundCheckTexture = new Texture2D(textureWidth, textureHeight);

        cameraMin = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, mainCamera.nearClipPlane));
        cameraMax = mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, mainCamera.nearClipPlane));

        var colors = new Color32[textureWidth * textureHeight];
        for (var i = 0; i < colors.Length; i++) {
            colors[i] = Color.black;
        }

        groundCheckTexture.SetPixels32(colors);
        groundCheckTexture.Apply();
    }

    private Vector2Int CalculateTextureCoordinates(Vector2 worldPosition) {

        return new Vector2Int(
            Mathf.RoundToInt(Mathf.Lerp(0, textureWidth, Mathf.InverseLerp(cameraMin.x, cameraMax.x, worldPosition.x))),
            Mathf.RoundToInt(Mathf.Lerp(0, textureHeight, Mathf.InverseLerp(cameraMin.y, cameraMax.y, worldPosition.y)))
        );
    }
}
