using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Cupcake : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public int x, y; // griddeki yerim
    [HideInInspector] public RandomCupcakeGen board;
    private Vector2 dragStart;

    public void OnBeginDrag(PointerEventData eventdata)
    {
        dragStart = eventdata.position;
    }
    public void OnDrag(PointerEventData e) { }

    public void OnEndDrag(PointerEventData e)
    {
        Vector2 delta = e.position - dragStart;
        if (delta.magnitude < 30f) return;   // minicik kıpırdama = sayma

        // hangi eksende daha çok sürüklendi? sadece O yöne, sadece 1 hücre
        int dx = 0, dy = 0;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            dx = delta.x > 0 ? 1 : -1;   // sağ / sol
        else
            dy = delta.y > 0 ? 1 : -1;   // yukarı / aşağı

        board.TrySwap(x, y, dx, dy);
    }
}
