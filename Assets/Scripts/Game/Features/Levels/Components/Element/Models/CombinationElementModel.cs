using System.Collections.Generic;
using System.Linq;
using Game.Features.Levels.Components.Element.Views;
using InputSystem.Models;

namespace Game.Features.Levels.Components.Element.Models
{
    public class CombinationElementModel
    {
        public readonly IElementView Element;
        public readonly Directions StartDirection;
        public readonly List<CombinationElementModel> Neighbors;

        private List<IElementView> _allElements;

        public List<IElementView> AllElements => GetAll();

        public CombinationElementModel(IElementView element, Directions direction)
        {
            element.TakeInBusiness();
            
            Element = element;
            StartDirection = direction;

            Neighbors = new List<CombinationElementModel>();
        }
        
        private List<IElementView> GetAll()
        {
            _allElements ??= new List<IElementView> { Element };
            Neighbors.ForEach(n => _allElements.AddRange(n.AllElements));
            
            return _allElements;
        }
    }
}