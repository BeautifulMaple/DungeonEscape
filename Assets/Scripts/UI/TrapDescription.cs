using UnityEngine;

[CreateAssetMenu(fileName = "TrapDescription", menuName = "ScriptableObjects/TrapDescription", order = 1)]
public class TrapDescription : ScriptableObject
{
    [TextArea]
    public string description; // 설명 텍스트
}
