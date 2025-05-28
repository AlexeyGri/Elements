using System.Collections.Generic;
using Game.Features.Levels.Components.Element.Models;
using Game.Features.Levels.Components.Grid.Models;
using UnityEngine;

namespace Game.Features.Levels.Models
{
    [CreateAssetMenu(menuName = "Create/Levels", fileName = "Level")]
    public class LevelModel : ScriptableObject
    {
        public GridModel GridModel;
        public List<ElementModel> ElementModels;

    }
}