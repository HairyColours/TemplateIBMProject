using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelAudioScript : MonoBehaviour, IPointerEnterHandler
{
    public GameObject prefabToSpawn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject audio = Instantiate(prefabToSpawn, transform.position, transform.rotation);
        Destroy(audio, 3f);
    }
}
