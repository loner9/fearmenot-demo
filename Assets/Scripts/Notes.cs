using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour, IItem
{
    public static event Action<string, string> onNotesCollect;
    public string nama;
    public string pesan;
    public void Collect()
    {
        onNotesCollect.Invoke(nama, pesan);
        Destroy(gameObject);
    }

}
