using UnityEngine;
using System.Collections;

public interface IJSON
{
    string ToJSON();
    void FromJSON(string JSON);
}
