using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if !DEBUG
using BepInEx;
using BepInEx.Logging;
#endif

public class LadderGenerator : MonoBehaviour
{
    private GameObject parent;
    private ArrayList ladderSegments = new ArrayList();

    public GameObject bottomPos;
    public GameObject topPos;
    public GameObject horizontalPos;
    public GameObject playerPos;
    public GameObject trigger;

    public GameObject ladderSegment;

#if !DEBUG
    protected ManualLogSource Logger = new ManualLogSource($"{PlugInInfo.Name}.{typeof(LadderGenerator).Name}");
#endif


    // Start is called before the first frame update
    void Start()
    {
        parent = this.gameObject;

        // Calculate the necessary segments.
        var height = parent.transform.localScale.y;
        var elHeight = ladderSegment.transform.localScale.y;
        long elCount = (long)System.Math.Ceiling(height / elHeight);
        var elSpaceMult = height / (elCount * elHeight);

        // Reset all scaling.
        parent.transform.localScale = new Vector3(1, 1, 1);

        // Generate the segments and place them.
        for (long i = 1; i < elCount; i++)
        {
            var obj = GameObject.Instantiate(ladderSegment, parent.transform);
            ladderSegments.Add(obj);

            Vector3 objPos = obj.transform.localPosition;
            objPos.y += (float)i * (elHeight * elSpaceMult);
            obj.transform.localPosition = objPos;

#if !DEBUG
            Logger.LogInfo($"<{typeof(LadderGenerator).Name}> Segment {i} generated.");
#endif
        }

        // Fix the position of the top.
        {
            var pos = topPos.transform.localPosition;
            pos.y += (float)height; // Modify so artists can somewhat control the final offset.
            topPos.transform.localPosition = pos;
        }

        { // Fix up the trigger.
            var triggerPos = trigger.transform.localPosition;
            triggerPos.y = (float)(height / 2.0 + elHeight / 2.0); // Offset by half a segment.
            trigger.transform.localPosition = triggerPos;
            var triggerScale = trigger.transform.localScale;
            triggerScale.y = (float)(height + elHeight); // Scale by an additional segment.
            trigger.transform.localScale = triggerScale;
            // Using the ladder without the additional segment is otherwise difficult.
        }


#if !DEBUG
        Logger.LogInfo("Completed");
#endif
    }
}

