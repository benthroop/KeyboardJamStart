using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    private Transform[] keys;
    public Dictionary<Transform, Vector3> keyStartingLocalPositions;
    public Dictionary<KeyCode, Transform> keyKeyCodes;
    private KeyCode[] allKeyCodes;
    public Vector3 keyOffset;

    //we walk the children and parse names (which have been set to match KeyCode)
    //to set up a dictionary mapping KeyCode to transform
    void Start()
    {
        keys = new Transform[transform.childCount];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = transform.GetChild(i);
        }

        allKeyCodes = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];
        keyKeyCodes = new Dictionary<KeyCode, Transform>();
        keyStartingLocalPositions = new Dictionary<Transform, Vector3>();

        for (int i = 0; i < keys.Length; i++)
        {
            keyStartingLocalPositions.Add(keys[i], keys[i].localPosition);
        }

        string[] trimmedKeyNames = new string[keys.Length];
        for (int i = 0; i < trimmedKeyNames.Length; i++)
        {
            var spl = keys[i].name.Split('_');
            if (spl.Length == 2)
            {
                trimmedKeyNames[i] = spl[1];
            }
            else
            {
                Debug.LogError("Malformed key name. Expected Key_<name>. Using entire name " + keys[i].name + "as fallback.");
                trimmedKeyNames[i] = keys[i].name;
            }
        }

        string[] n = System.Enum.GetNames(typeof(KeyCode));
        int e = n.Length;
        for (int i = 0; i < e; i++)
        {
            for (int k = 0; k < trimmedKeyNames.Length; k++)
            {
                if (trimmedKeyNames[k] == n[i])
                {
                    keyKeyCodes.Add(allKeyCodes[i], keys[k]);
                }
            }
        }
    }

    private Transform tmpKey;
    void Update()
    {
        for (int i=0; i < allKeyCodes.Length; i++)
        {
            if (Input.GetKeyDown(allKeyCodes[i]))
            {
                if (keyKeyCodes.TryGetValue(allKeyCodes[i], out tmpKey))
                {
                    tmpKey.localPosition = keyStartingLocalPositions[tmpKey] + keyOffset;
                }
            }
            if (Input.GetKeyUp(allKeyCodes[i]))
            {
                if (keyKeyCodes.TryGetValue(allKeyCodes[i], out tmpKey))
                {
                    tmpKey.localPosition = keyStartingLocalPositions[tmpKey];
                }
            }
        }
    }
}