using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GroupInteractiveObjects", menuName = "InteractiveObject")]
public class GroupInteractiveItems : ScriptableObject
{
    [SerializeField] private bool _SpriteInQuestion;
    [SerializeField] private InteractiveItem[] _interactiveItems;

    public InteractiveItem[] InteractiveItems => _interactiveItems;
}
