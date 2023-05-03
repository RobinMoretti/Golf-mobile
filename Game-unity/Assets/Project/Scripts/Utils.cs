using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position){
        position.z = camera.nearClipPlane + 1;
        position = camera.ScreenToWorldPoint(position);
        return position;
    }
}
