using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    [SerializeField] Texture2D cursor;
    private Vector2 cursorHotspot;
    // Start is called before the first frame update
    void Start()
    {
        cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }


}
