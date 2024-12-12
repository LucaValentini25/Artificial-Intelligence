

using System;
using NPC;
using UnityEngine;

public class MoveToClickPoint : MonoBehaviour
{
    public ColorBand color;
    public int MouseIndex;
    public Camera cam;
    public LayerMask floorLayerMask;
    public LayerMask ignoreLayerMask;
    public Vector3 offset;
    public float wallSeparation;
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(MouseIndex))
        {
            //cast raycast from screen to floor point
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            //si colisiona con layers que no puede ponerse la vandera retorna
            if (Physics.Raycast(ray,Mathf.Infinity,ignoreLayerMask))return;
            // Realiza el Raycast y verifica si golpea algo en la capa del piso
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask))
            {
                // Obtiene el punto de impacto
                Vector3 hitPoint = hit.point;
                if (Physics.CheckSphere(hitPoint, wallSeparation, ignoreLayerMask))
                {
                    return;
                }
                transform.position = hitPoint + offset;
                Debug.Log("Hit Point: " + hitPoint);
            }
        }
    }
}
