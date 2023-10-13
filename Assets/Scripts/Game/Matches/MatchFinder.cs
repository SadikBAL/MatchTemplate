using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.Game.Matches
{
    public abstract class MatchFinder
    {
        public abstract List<Match> FindMatches(GameBoard Board);
    }
}
