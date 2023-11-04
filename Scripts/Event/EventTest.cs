using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    public void TestEvent(EventData data)
    {
        EventManager.Instance.EventStart(data, null);
    }
}
