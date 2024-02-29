using Assets.Scripts.MatchEnums;
using Assets.Scripts.TileEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Matches
{
    internal class IShapedMatchFinder : MatchFinder
    {
        public override List<Match> FindMatches(GameBoard Board)
        {
            
            List<Match> Matches = new List<Match>();
            
            for (int j = 0; j < Board.Height; j++)
            {
                for (int i = 0; i < Board.Width - 2;)
                {
                    GameObject Tile = Board.GetTile(i, j);
                    if (Tile != null && Tile.GetComponent<Candy>() != null)
                    {
                        CandyType Type = Tile.GetComponent<Candy>().Type;
                        if (Board.GetTile(i + 1, j) != null && Board.GetTile(i + 1, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i + 1, j).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i + 2, j) != null && Board.GetTile(i + 2, j).GetComponent<Candy>() != null &&
                            Board.GetTile(i + 2, j).GetComponent<Candy>().Type == Type)
                        {
                            Match Match = new Match();
                            Match.Type = MatchType.IShaped;
                            Match.StripeType = StripeType.Horizontal;
                            do
                            {
                                Match.AddTile(Board.GetTile(i, j));
                                i += 1;
                            } while (i < Board.Width && Board.GetTile(i, j) != null &&
                                     Board.GetTile(i, j).GetComponent<Candy>() != null &&
                                     Board.GetTile(i, j).GetComponent<Candy>().Type == Type);

                            Matches.Add(Match);
                            continue;
                        }
                    }
                    i += 1;
                }
            }

            for (int i = 0; i < Board.Width; i++)
            {
                for (int j = 0; j < Board.Height - 2;)
                {
                    GameObject Tile = Board.GetTile(i, j);
                    if (Tile != null && Tile.GetComponent<Candy>() != null)
                    {
                        CandyType Type = Tile.GetComponent<Candy>().Type;
                        if (Board.GetTile(i, j + 1) != null && Board.GetTile(i, j + 1).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j + 1).GetComponent<Candy>().Type == Type &&
                            Board.GetTile(i, j + 2) != null && Board.GetTile(i, j + 2).GetComponent<Candy>() != null &&
                            Board.GetTile(i, j + 2).GetComponent<Candy>().Type == Type)
                        {
                            Match Match = new Match();
                            Match.Type = MatchType.IShaped;
                            Match.StripeType = StripeType.Vertical;
                            do
                            {
                                Match.AddTile(Board.GetTile(i, j));
                                j += 1;
                            } while (j < Board.Height && Board.GetTile(i, j) != null &&
                                     Board.GetTile(i, j).GetComponent<Candy>() != null &&
                                     Board.GetTile(i, j).GetComponent<Candy>().Type == Type);

                            Matches.Add(Match);
                            continue;
                        }
                    }
                    j += 1;
                }
            }
            return Matches;
        }
    }
}
