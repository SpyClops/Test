using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public LayerMask ClickableLayer;
    public EventVector3 OnClickEnvironment;
    
    //public EventVector3 OnClickEnvironment;
    
    public void Update() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, ClickableLayer.value)) {
            bool enemyHit = false;
            
            if (hit.collider.gameObject.tag == "enemy") {
                //Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                enemyHit = true;
            }
            else {
                //Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
            }
            
            if (Input.GetMouseButtonDown(0)) {
                if (enemyHit) {
                    Transform enemyPos = hit.collider.gameObject.transform;
                    OnClickEnvironment.Invoke(enemyPos.position);
                    Debug.Log("Clicked on enemy    ");
                }
                else {
                    OnClickEnvironment.Invoke(hit.point);
                    Debug.Log("floor");
                }
            
            }
        }
        else {
           // Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> {}

