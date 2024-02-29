﻿using Assets.Scripts.MatchEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Matches
{
    internal class LShapedMatchFinder : MatchFinder
    {
        public override List<Match> FindMatches(GameBoard Board)
        {
            var Matches = new List<Match>();
            
            for (var j = 0; j < Board.Height; j++)
            {
                for (var i = 0; i < Board.Width - 2;)
                {
                    var Tile = Board.GetTile(i, j);
                    if (Tile != null && Tile.GetComponent<Candy>() != null)
                    {
                        var Type = Tile.GetComponent<Candy>().Type;
                        if (Board.GetTile(i + 1, j) != null && Board.GetTile(i + 1, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i + 1, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i + 2, j) != null && Board.GetTile(i + 2, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i + 2, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j + 1) != null && Board.GetTile(i, j + 1).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j + 1).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j + 2) != null && Board.GetTile(i, j + 2).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j + 2).GetComponent<Candy>().Type == Type)
                        {
                            var Match = new Match();
                            Match.Type = MatchType.LShaped;
                            Match.AddTile(Board.GetTile(i, j));
                            Match.AddTile(Board.GetTile(i + 1, j));
                            Match.AddTile(Board.GetTile(i + 2, j));
                            Match.AddTile(Board.GetTile(i, j + 1));
                            Match.AddTile(Board.GetTile(i, j + 2));
                            Matches.Add(Match);

                            var k = i + 3;
                            while (k < Board.Width && Board.GetTile(k, j) != null &&
                                   Board.GetTile(k, j).GetComponent<Candy>() != null &&
                                   Board.GetTile(k, j).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(k, j));
                                k += 1;
                            }

                            k = j + 3;
                            while (k < Board.Height && Board.GetTile(i, k) != null &&
                                   Board.GetTile(i, k).GetComponent<Candy>() != null &&
                                   Board.GetTile(i, k).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(i, k));
                                k += 1;
                            }
                        }

                        if (Board.GetTile(i + 1, j) != null && Board.GetTile(i + 1, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i + 1, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i + 2, j) != null && Board.GetTile(i + 2, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i + 2, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j - 1) != null && Board.GetTile(i, j - 1).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j - 1).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j - 2) != null && Board.GetTile(i, j - 2).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j - 2).GetComponent<Candy>().Type == Type)
                        {
                            var Match = new Match();
                            Match.Type = MatchType.LShaped;
                            Match.AddTile(Board.GetTile(i, j));
                            Match.AddTile(Board.GetTile(i + 1, j));
                            Match.AddTile(Board.GetTile(i + 2, j));
                            Match.AddTile(Board.GetTile(i, j - 1));
                            Match.AddTile(Board.GetTile(i, j - 2));
                            Matches.Add(Match);

                            var k = i + 3;
                            while (k < Board.Width && Board.GetTile(k, j) != null &&
                                   Board.GetTile(k, j).GetComponent<Candy>() != null &&
                                   Board.GetTile(k, j).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(k, j));
                                k += 1;
                            }

                            k = j - 3;
                            while (k >= 0 && Board.GetTile(i, k) != null && Board.GetTile(i, k).GetComponent<Candy>() != null &&
                                   Board.GetTile(i, k).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(i, k));
                                k -= 1;
                            }
                        }

                        if (Board.GetTile(i - 1, j) != null && Board.GetTile(i - 1, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i - 1, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i - 2, j) != null && Board.GetTile(i - 2, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i - 2, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j + 1) != null && Board.GetTile(i, j + 1).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j + 1).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j + 2) != null && Board.GetTile(i, j + 2).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j + 2).GetComponent<Candy>().Type == Type)
                        {
                            var Match = new Match();
                            Match.Type = MatchType.LShaped;
                            Match.AddTile(Board.GetTile(i, j));
                            Match.AddTile(Board.GetTile(i - 1, j));
                            Match.AddTile(Board.GetTile(i - 2, j));
                            Match.AddTile(Board.GetTile(i, j + 1));
                            Match.AddTile(Board.GetTile(i, j + 2));
                            Matches.Add(Match);

                            var k = i - 3;
                            while (k >= 0 && Board.GetTile(k, j) != null && Board.GetTile(k, j).GetComponent<Candy>() != null &&
                                   Board.GetTile(k, j).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(k, j));
                                k -= 1;
                            }

                            k = j + 3;
                            while (k < Board.Height && Board.GetTile(i, k) != null &&
                                   Board.GetTile(i, k).GetComponent<Candy>() != null &&
                                   Board.GetTile(i, k).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(i, k));
                                k += 1;
                            }
                        }

                        if (Board.GetTile(i - 1, j) != null && Board.GetTile(i - 1, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i - 1, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i - 2, j) != null && Board.GetTile(i - 2, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i - 2, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j - 1) != null && Board.GetTile(i, j - 1).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j - 1).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j - 2) != null && Board.GetTile(i, j - 2).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j - 2).GetComponent<Candy>().Type == Type)
                        {
                            var Match = new Match();
                            Match.Type = MatchType.LShaped;
                            Match.AddTile(Board.GetTile(i, j));
                            Match.AddTile(Board.GetTile(i - 1, j));
                            Match.AddTile(Board.GetTile(i - 2, j));
                            Match.AddTile(Board.GetTile(i, j - 1));
                            Match.AddTile(Board.GetTile(i, j - 2));
                            Matches.Add(Match);

                            var k = i - 3;
                            while (k >= 0 && Board.GetTile(k, j) != null && Board.GetTile(k, j).GetComponent<Candy>() != null &&
                                   Board.GetTile(k, j).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(k, j));
                                k -= 1;
                            }

                            k = j - 3;
                            while (k >= 0 && Board.GetTile(i, k) != null && Board.GetTile(i, k).GetComponent<Candy>() != null &&
                                   Board.GetTile(i, k).GetComponent<Candy>().Type == Type)
                            {
                                Match.AddTile(Board.GetTile(i, k));
                                k -= 1;
                            }
                        }
                    }

                    i += 1;
                }
            }
            
            return Matches;
        }
    }
}
