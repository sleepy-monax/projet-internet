using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Attacks;
using Model.Effets;

namespace Model.Creature
{
    public class Creature : ICreature
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public Stereotype Stereotype { get; set; }
        
        public int Xp { get; set; }

        public int Level
        {
            get
            {
                var level = 1;
                var xpLeft = Xp;

                do
                {
                    var xpNeeded = Math.Pow(level, 1.1) + 100;
                    xpLeft -= (int)xpNeeded;

                    if (xpLeft > 0)
                    {
                        level++;
                    }
                } while (xpLeft > 0);

                return level;
            }
        }

        public bool Pickable { get; set; }

        public Stats Stats => Effects.Aggregate(Stereotype.Stats, (current, effect) => effect.Apply(current));

        public bool Alive => Stats.Health > 0;
        public bool Defeated => !Alive;

        public List<IEffect> Effects { get; set; } = new List<IEffect>();

        public List<UnLockableAction> AllActions => Stereotype.Actions;

        public List<Attack> AvaillableActions => AllActions
            .FindAll(unlockable => unlockable.Level <= Level)
            .FindAll(unlockable => unlockable.Action.PowerPoint > 0)
            .Select(unlockable => unlockable.Action)
            .ToList();
    }
}