using UnityEngine;

// Ajoute ce composant sur le prefab de l'item DASH/COURSE
public class ItemGrantsPowers : MonoBehaviour
{
    [Tooltip("Coche pour que cet item accorde le dash et la course")]
    public bool grantsDashAndRun = true;
}
