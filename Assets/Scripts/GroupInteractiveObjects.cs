using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GroupInteractiveObjects", menuName = "InteractiveObject")]
public class GroupInteractiveObjects : ScriptableObject
{
    [SerializeField] private bool _SpriteInQuestion;
    [SerializeField] private InteractiveObject[] _interactiveObjects;

    public InteractiveObject[] InteractiveObjects => _interactiveObjects;
}
