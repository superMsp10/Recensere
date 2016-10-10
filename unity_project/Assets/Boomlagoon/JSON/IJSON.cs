using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public interface IJSON
{
    JSONObject ToJSON();
    void FromJSON(JSONObject JSON);
}
