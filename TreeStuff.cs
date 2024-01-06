using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DEBUG
using UnityEditor;
using UnityEngine;

namespace com.xaymar.lethalcompany
{
    [ExecuteInEditMode]
    class TreeStuff : MonoBehaviour
    {
        [MenuItem("Xaymar/Place and Randomize Objects")]
        static void PlaceAndRandomizeObjects()
        {
            var selected = Selection.gameObjects;
            Debug.Log($"Randomizing {selected.Length} objects...");

            Dictionary<GameObject, int> visibility = new Dictionary<GameObject, int>();

            // Record undo for the selected objects.
            Undo.RecordObjects(selected, "Place and Randomize Objects");

            // Store and change the visibility of selected objects.
            foreach(var obj in selected)
            {
                visibility[obj] = obj.layer;
                obj.layer = LayerMask.NameToLayer("Ignore Raycast"); // Ignore Raycast
            }

            RaycastHit hit;
            foreach (var obj in selected)
            {
                Vector3 origin = obj.transform.position;
                Debug.Log($"{obj.name}: {origin.ToString()}");
                origin += new Vector3(0, 100, 0);
                if (Physics.Raycast(origin, new Vector3(0, -1, 0), out hit, 10000000.0f, ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.UseGlobal))
                {
                    float ang = UnityEngine.Random.Range((float)-Math.PI * 2.0f, (float)Math.PI * 2.0f);
                    Vector3 tgt = new Vector3((float)Math.Cos(ang), 0, (float)Math.Sin(ang));
                    tgt.Normalize();

                    Quaternion nRot = Quaternion.LookRotation(tgt, hit.normal);
                    Quaternion vRot = Quaternion.LookRotation(tgt, Vector3.up);
                    Quaternion rot = Quaternion.Slerp(vRot, nRot, 0.5f);

                    float size = UnityEngine.Random.Range(3.0f, 5.0f);
                    Vector3 scl = new Vector3(size, size, size);

                    obj.transform.localScale = scl;
                    obj.transform.SetPositionAndRotation(hit.point, rot);
                    obj.transform.SetPositionAndRotation(hit.point - obj.transform.up * 1.5f, rot);

                    Debug.Log($"{obj.name}: {obj.transform.ToString()}");
                }
            }

            // Restore the visibility state of selected objects.
            foreach(var obj in selected)
            {
                obj.layer = visibility[obj];
            }
        }

        [MenuItem("Xaymar/Place on Ground")]
        static void PlaceOnGround()
        {
            var selected = Selection.gameObjects;
            Debug.Log($"Placing {selected.Length} objects...");

            Dictionary<GameObject, int> visibility = new Dictionary<GameObject, int>();

            // Record undo for the selected objects.
            Undo.RecordObjects(selected, "Place and Randomize Objects");

            // Store and change the visibility of selected objects.
            foreach (var obj in selected)
            {
                visibility[obj] = obj.layer;
                obj.layer = LayerMask.NameToLayer("Ignore Raycast"); // Ignore Raycast
            }

            RaycastHit hit;
            foreach (var obj in selected)
            {
                Vector3 origin = obj.transform.position;
                origin += new Vector3(0, 100, 0);
                if (Physics.Raycast(origin, new Vector3(0, -1, 0), out hit, 10000000.0f, ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.UseGlobal))
                {
                    obj.transform.SetPositionAndRotation(hit.point, obj.transform.rotation);
                }
            }

            // Restore the visibility state of selected objects.
            foreach (var obj in selected)
            {
                obj.layer = visibility[obj];
            }
        }
    }
}
#endif
