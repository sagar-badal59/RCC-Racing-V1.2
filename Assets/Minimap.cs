using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : SingletonMonoBehaviour<Minimap>
{
    public List<Material> materials;
    public Transform player;
    
    public void setVechicle(GameObject go)
    {
        player = go.transform;
        GameObject markingSphere = go.transform.Find("Marking-Sphere").gameObject;
        if (markingSphere != null) markingSphere.GetComponent<Renderer>().material = materials[1];
    }
    
    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
