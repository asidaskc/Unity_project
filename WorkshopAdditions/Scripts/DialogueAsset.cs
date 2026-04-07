using UnityEngine;

[CreateAssetMenu(menuName = "Workshop/Dialogue Asset")]
public class DialogueAsset : ScriptableObject
{
    [TextArea(2, 6)]
    public string[] lines;
}
