using UnityEditor.Animations;
using UnityEngine;

namespace Game.Features.Levels.Components.Element.Models
{
    [CreateAssetMenu(menuName = "Create/Element", fileName = "Element")]
    public class ElementModel : ScriptableObject
    {
        public int Id;
        public Sprite Sprite;
        public AnimatorController AnimatorController;
    }
}