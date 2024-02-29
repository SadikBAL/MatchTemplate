using Assets.Scripts.Game.Blast;
using Assets.Scripts.Game.Matches;
using Assets.Scripts.MatchEnums;
using Assets.Scripts.TileEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using MatchType = Assets.Scripts.MatchEnums.MatchType;

public enum GameState
{
    None,
    Init,
    WaitingInput,
    Draging,
    InAnimation
}

public class GameBoard : MonoBehaviour
{
    public AnimationManager AnimationManager;
    event Action OnSwapComplated;
    event Action OnExplodeComplated;
    event Action OnFallComplated;
    event Action OnFillComplated;
    event Action OnFailSwapComplated;
    public TilePool TilePool;
    public List<GameObject> Tiles;
    public List<MatchType> MatchPriority;
    public int Width = 5;
    public int Height = 3;
    public GameState CurrentState = GameState.None;
    private GameObject SelectedTile;
    private GameObject SwapingTile;
    public Transform BoardCenter;
    private float StartX = 0;
    private float StartY = 0;
    private float TileW = 1.5f;
    private float TileH = 1.5f;

    private readonly MatchFinder IShapeMatchFinder = new IShapedMatchFinder();
    private readonly MatchFinder LShapeMatchFinder = new LShapedMatchFinder();
    private readonly MatchFinder TShapeMatchFinder = new TShapedMatchFinder();

    private readonly BlastFinder BlastDoubleColorBombFinder = new BlastDoubleColorBomb();
    private readonly BlastFinder BlastColorBombWrapFinder   = new BlastColorBombWrap();
    private readonly BlastFinder BlastColorBombStripeFinder = new BlastColorBombStripe();
    private readonly BlastFinder BlastColorBombCandyFinder  = new BlastColorBombCandy();
    private readonly BlastFinder BlastDoubleWrapFinder      = new BlastDoubleWrap();
    private readonly BlastFinder BlastDoubleStripeFinder    = new BlastDoubleStripe();
    private readonly BlastFinder BlastStripeWrapFinder      = new BlastStripeWrap();


    void Start()
    {
        UpdateState(GameState.Init);
        OnSwapComplated += OnSwap;
        OnExplodeComplated += OnExplode;
        OnFallComplated += OnFall;
        OnFillComplated += OnFill;
        OnFailSwapComplated += OnFailSwap;

        Tiles = new List<GameObject>(Width * Height);
        MatchPriority = new List<MatchType>(Width * Height);
        StartX = BoardCenter.position.x - (((Width-1) * TileW) * 0.5f);
        StartY = BoardCenter.position.y + (((Height-1) * TileH) * 0.5f);
        foreach (PrefabPool Pool in TilePool.GetComponentsInChildren<PrefabPool>())
        {
            Pool.Reset();
        }

        InitBoard();
        UpdateState(GameState.WaitingInput);
    }
    private void OnFailSwap()
    {
        UpdateState(GameState.WaitingInput);
    }
    private void OnFill()
    {
        OnSwap();
    }
    private void OnFall()
    {
        FillEmptyTiles();
    }
    private void OnExplode()
    {
       FallToEmptyTiles();
    }
    private void OnSwap()
    {
        CheckMatches();
    }
    void Update()
    {
        //Cheting
        if (CheckState(GameState.WaitingInput) && Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit2D Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Hit.collider != null && Hit.collider.gameObject.CompareTag("Tile"))
            {
                int CandyType = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CandyType)).Length);
                int StripeType = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CandyStripeType)).Length);
  
                GameObject RandomObject = TilePool.GetStripeCandy((CandyType)CandyType, (CandyStripeType)StripeType);
                Cheat(Hit.collider.gameObject, RandomObject);
            }
            return;
        }

        if (CheckState(GameState.WaitingInput) && Input.GetKeyDown(KeyCode.W))
        {
            RaycastHit2D Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Hit.collider != null && Hit.collider.gameObject.CompareTag("Tile"))
            {
                int CandyType = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CandyType)).Length);

                GameObject RandomObject = TilePool.GetWrappedCandy((CandyType)CandyType);
                Cheat(Hit.collider.gameObject, RandomObject);
            }
            return;
        }

        if (CheckState(GameState.WaitingInput) && Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Hit.collider != null && Hit.collider.gameObject.CompareTag("Tile"))
            {
                GameObject RandomObject = TilePool.GetColorBomb();
                Cheat(Hit.collider.gameObject, RandomObject);
            }
            return;
        }

        //Gameplay Control
        if (CheckState(GameState.Draging) && Input.GetMouseButtonUp(0))
        {
            UpdateState(GameState.WaitingInput);
        }

        if (CheckState(GameState.WaitingInput) && Input.GetMouseButtonDown(0))
        {

            RaycastHit2D Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Hit.collider != null && Hit.collider.gameObject.CompareTag("Tile"))
            {
                if (Hit.collider.gameObject.GetComponent<Tile>().Moveable())
                {
                    SelectedTile = Hit.collider.gameObject;
                    UpdateState(GameState.Draging);
                }
            }
            return;
        }

        if (CheckState(GameState.Draging))
        {
            var Hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Hit.collider != null && Hit.collider.gameObject != SelectedTile)
            {
                if (!Hit.collider.GetComponent<Tile>().Moveable())
                {
                    return;
                }
                UpdateState(GameState.InAnimation);
                SwapingTile = Hit.collider.gameObject;
                Swap(SelectedTile, SwapingTile, OnSwapComplated);
            }
        }
    }
    public bool CheckState(GameState State)
    {
        return State == CurrentState;
    }
    public void UpdateState(GameState State)
    {
        CurrentState = State;
    }
    private void Swap(GameObject TileA, GameObject TileB, Action OnCompleted)
    {
        int IdA = Tiles.FindIndex(x => x == TileA);
        int IdB = Tiles.FindIndex(x => x == TileB);
        var i = IdA % Width;
        var j = IdA / Width;
        var k = IdB % Width;
        var l = IdB / Width;
        if (i - k > 1 || j - l > 1 || i - k < -1 || j-l < -1)
        {
            OnCompleted();
            return;
        }
        GameObject Temp = Tiles[IdA];
        Tiles[IdA] = Tiles[IdB];
        Tiles[IdB] = Temp;
        Tiles[IdA].GetComponent<Tile>().SetGameBoard(this);
        Tiles[IdB].GetComponent<Tile>().SetGameBoard(this);


        AnimationManager.Slide(new List<SlideObject>
                    {
                        new SlideObject{
                            EaseType = EaseType.Lerp,
                            AnimationType = AnimationType.Position,
                            Duration = .2f,
                            StartPosition = Tiles[IdA].gameObject.transform.position,
                            EndPosition = new Vector3(StartX + i * (TileW), -j * (TileH) + StartY,0),
                            GameObject =  Tiles[IdA].gameObject
                        },
                        new SlideObject{
                            EaseType = EaseType.Lerp,
                            AnimationType = AnimationType.Position,
                            Duration = .2f,
                            StartPosition = Tiles[IdB].gameObject.transform.position,
                            EndPosition = new Vector3(StartX + k * (TileW), -l * (TileH) + StartY,0),
                            GameObject =  Tiles[IdB].gameObject
                        }
                    }
        ,OnCompleted) ;
    }
    public GameObject GetTile(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return Tiles[x + (y * Width)];
        }
        return null;
    }
    private void ExplodeTiles(List<Match> ExplodeList,bool IsRecursive)
    {
        List<SlideObject> List = new List<SlideObject>();
        foreach (Match m in ExplodeList)
        {
            foreach (GameObject Tile in m.Tiles)
            {
                List<GameObject> ExplodeTileList = new List<GameObject>();
                ExplodeTile(Tile, ExplodeTileList, IsRecursive);
                foreach(GameObject t in ExplodeTileList)
                {
                    List.Add(new SlideObject
                    {
                        EaseType = EaseType.Lerp,
                        AnimationType = AnimationType.Scale,
                        Duration = .3f,
                        StartPosition = Vector3.one,
                        EndPosition = Vector3.zero,
                        GameObject = Tile
                    }
                    );
                    t.GetComponent<Tile>().Destroyed = true;
                }
                
                
            }
        }
        AnimationManager.Slide(List,OnExplodeComplated);
    }
    private void FallToEmptyTiles()
    {
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].GetComponent<Tile>().Destroyed)
            {
                Tiles[i].GetComponent<Tile>().Destroyed = false;
                Tiles[i].GetComponent<PooledPrefab>().Pool.ReturnObject(Tiles[i]);
                Tiles[i] = null;
            }
        }
        List<SlideObject> List = new List<SlideObject>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                int Index = x + (y * Width);
                if (GetTile(x, y) == null)
                {
                    continue;
                }
                int FallTo = -1;
                for (var k = y; k < Height; k++)
                {
                    int idx = x + (k * Width);
                    if (Tiles[idx] == null)
                    {
                        FallTo = k;
                    }
                }

                if (FallTo != -1)
                {
                    GameObject Tile = GetTile(x, y);
                    if (Tile != null)
                    {
                        int FallDist = FallTo - y;
                        Tiles[Index + (FallDist * Width)] = Tiles[Index];
                        Tiles[Index + (FallDist * Width)].GetComponent<Tile>().SetGameBoard(this);
                        List.Add(new SlideObject
                        {
                            EaseType = EaseType.Lerp,
                            AnimationType = AnimationType.Position,
                            Duration = .3f,
                            StartPosition = Tile.transform.position,
                            EndPosition = GetTilePosition(Index + (FallDist * Width)),
                            GameObject = Tile
                        }
                        );
                        Tiles[Index] = null;
                    }
                }
            }
        }
        AnimationManager.Slide(List, OnFallComplated);
    }
    private void FillEmptyTiles()
    {
        List<SlideObject> List = new List<SlideObject>();
        for (int x = 0; x < Width; x++)
        {
            int EmptyTileCount = 0;
            for (int y = 0; y < Height; y++)
            {
                int TileIndex = x + (y * Width);
                if (Tiles[TileIndex] == null)
                {
                    EmptyTileCount += 1;
                }
                else if (Tiles[TileIndex] != null)
                {
                    break;
                }
            }

            if (EmptyTileCount > 0)
            {
                for (int j = 0; j < Height; j++)
                {
                    var TileIndex = x + (j * Width);

                    if (Tiles[TileIndex] == null)
                    {
                        GameObject RndGameObject = TilePool.GetRandomCandy();
                        Vector3 SourcePosition = GetTilePosition(x);
                        Vector3 TargetPosition = GetTilePosition(TileIndex);
                        Vector3 SourcePositionWithFallHeight = SourcePosition;
                        SourcePositionWithFallHeight.y += (EmptyTileCount * (TileH));
                        --EmptyTileCount;
                        RndGameObject.transform.position = SourcePositionWithFallHeight;
                        List.Add(new SlideObject
                        {
                            EaseType = EaseType.Lerp,
                            AnimationType = AnimationType.Position,
                            Duration = .3f,
                            StartPosition = SourcePositionWithFallHeight,
                            EndPosition = TargetPosition,
                            GameObject = RndGameObject
                        }
                        );

                        Tiles[TileIndex] = RndGameObject;
                        Tiles[TileIndex].GetComponent<Tile>().SetGameBoard(this);
                    }
                }
            }
        }
        AnimationManager.Slide(List, OnFillComplated);
    }
    private Vector3 GetTilePosition(int Index)
    {
        int x = Index % Width;
        int y = Index / Width;
        return new Vector3(StartX + x * (TileW), -y * (TileH) + StartY, 0);
    }
    private void InitBoard()
    {
        for (int y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                GameObject RandomObject = TilePool.GetRandomCandy();
                if (RandomObject != null)
                {
                    RandomObject.transform.position = new Vector2(StartX + x * (TileW), StartY + (-y * (TileH)));
                }
                Tiles.Add(RandomObject);
                MatchPriority.Add(MatchType.None);
                RandomObject.GetComponent<Tile>().SetGameBoard(this);
            }
        }
        CheckBoard();
    }
    private void CheckBoard()
    {
        List<Match> Matches = new List<Match>();
        Matches.AddRange(GetMatches());
        if(Matches.Count > 0)
        {
            foreach (Match m in Matches)
            {
                foreach(GameObject g in m.Tiles)
                {
                    g.GetComponent<Tile>().Destroyed = true;
                }
            }
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].GetComponent<Tile>().Destroyed)
                {
                    Tiles[i].GetComponent<Tile>().Destroyed = false;
                    Tiles[i].GetComponent<PooledPrefab>().Pool.ReturnObject(Tiles[i]);
                    GameObject RandomObject = TilePool.GetRandomCandy();
                    if (RandomObject != null)
                    {
                        RandomObject.transform.position = GetTilePosition(i);
                    }
                    Tiles[i] = RandomObject;
                    Tiles[i].GetComponent<Tile>().SetGameBoard(this);
                }
            }
            CheckBoard();
        }
    }
    private List<Match> GetMatches()
    {
        List<Match> Matches = new List<Match>();
        for(int i = 0; i < MatchPriority.Count;i++)
        {
            MatchPriority[i] = MatchType.None;
        }

        List<Match> Temp = new List<Match>();

        Temp.AddRange(TShapeMatchFinder.FindMatches(this));
        foreach (Match m in Temp)
        {
            bool PriorityCheck = true;
            foreach(GameObject g in m.Tiles)
            {
                if(MatchPriority[g.GetComponent<Tile>().x + g.GetComponent<Tile>().y * Width] > MatchType.TShaped)
                {
                    PriorityCheck = false; break;
                }
            }
            if (PriorityCheck)
            {
                foreach (GameObject g in m.Tiles)
                {
                    MatchPriority[g.GetComponent<Tile>().x + g.GetComponent<Tile>().y * Width] = MatchType.TShaped;
                }
                Matches.Add(m);
            }
        }

        Temp.Clear();
        Temp.AddRange(LShapeMatchFinder.FindMatches(this));
        foreach (Match m in Temp)
        {
            bool PriorityCheck = true;
            foreach (GameObject g in m.Tiles)
            {
                if (MatchPriority[g.GetComponent<Tile>().x + g.GetComponent<Tile>().y * Width] > MatchType.LShaped)
                {
                    PriorityCheck = false; break;
                }
            }
            if (PriorityCheck)
            {
                foreach (GameObject g in m.Tiles)
                {
                    MatchPriority[g.GetComponent<Tile>().x + g.GetComponent<Tile>().y * Width] = MatchType.LShaped;
                }
                Matches.Add(m);
            }
        }

        Temp.Clear();
        Temp.AddRange(IShapeMatchFinder.FindMatches(this));
        foreach (Match m in Temp)
        {
            bool PriorityCheck = true;
            foreach (GameObject g in m.Tiles)
            {
                if (MatchPriority[g.GetComponent<Tile>().x + g.GetComponent<Tile>().y * Width] > MatchType.IShaped)
                {
                    PriorityCheck = false; break;
                }
            }
            if (PriorityCheck)
            {
                foreach (GameObject g in m.Tiles)
                {
                    MatchPriority[g.GetComponent<Tile>().x + g.GetComponent<Tile>().y * Width] = MatchType.IShaped;
                }
                Matches.Add(m);
            }
        }

        if(!CheckState(GameState.Init))
        {
            FindBonusTiles(Matches);
        }

        return Matches;
    }
    private List<Match> GetBlastes()
    {
        if (SelectedTile == null || SwapingTile == null)
            return new List<Match>();

        Match Match;
        Match = BlastDoubleColorBombFinder.Find(this, SelectedTile, SwapingTile);
        if (Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }
        Match = BlastColorBombWrapFinder.Find(this, SelectedTile, SwapingTile);
        if (Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }
        Match = BlastColorBombStripeFinder.Find(this, SelectedTile, SwapingTile);
        if (Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }
        Match = BlastColorBombCandyFinder.Find(this, SelectedTile, SwapingTile);
        if(Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }
        Match = BlastDoubleStripeFinder.Find(this, SelectedTile, SwapingTile);
        if (Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }
        Match = BlastDoubleWrapFinder.Find(this, SelectedTile, SwapingTile);
        if (Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }
        Match = BlastStripeWrapFinder.Find(this, SelectedTile, SwapingTile);
        if (Match.Tiles.Count > 0)
        {
            return new List<Match> { Match };
        }

        return new List<Match>();
    }
    private void ExplodeTile(GameObject TileObject, List<GameObject> TileList, bool IsRecursive)
    {
        if (TileObject != null && TileObject.GetComponent<Tile>() != null)
        {
            var ExplodeList = TileObject.GetComponent<Tile>().Explode();

            TileList.Add(TileObject);

            foreach (var t in ExplodeList)
            {
                if (t != null && t.GetComponent<Tile>() != null &&
                    !TileList.Contains(t))
                {
                    TileList.Add(t);
                    if(IsRecursive)
                        ExplodeTile(t, TileList,IsRecursive);
                }
            }

            foreach (var t in ExplodeList)
            {
                if (!ExplodeList.Contains(t))
                {
                    ExplodeList.Add(t);
                }
            }
        }
    }
    private void Cheat(GameObject TileObject, GameObject CheatObject)
    {
        int i = TileObject.GetComponent<Tile>().GetIndex();
        CheatObject.transform.position = Tiles[i].transform.position;
        Tiles[i].GetComponent<Tile>().Destroyed = false;
        Tiles[i].GetComponent<PooledPrefab>().Pool.ReturnObject(Tiles[i]);
        Tiles[i] = null;
        Tiles[i] = CheatObject;
        Tiles[i].GetComponent<Tile>().SetGameBoard(this);
       
    }
    public void FindBonusTiles(List<Match> Matches)
    {
        foreach (Match m in Matches)
        {
            if (m.Type == MatchType.LShaped)
            {
                if (m.Tiles.Contains(SelectedTile))
                {
                    UpgradeCandyToWrapCandy(SelectedTile);
                }
                else if (m.Tiles.Contains(SwapingTile))
                {
                    UpgradeCandyToWrapCandy(SwapingTile);
                }
                else
                {
                    int Index = UnityEngine.Random.Range(0, m.Tiles.Count - 1);
                    UpgradeCandyToWrapCandy(m.Tiles[Index]);
                }
            }
            if (m.Tiles.Count > 4 && m.Type == MatchType.IShaped)
            {
                if (m.Tiles.Contains(SelectedTile))
                {
                    UpgradeCandyToColorBomb(SelectedTile);
                }
                else if (m.Tiles.Contains(SwapingTile))
                {
                    UpgradeCandyToColorBomb(SwapingTile);
                }
                else
                {
                    int Index = UnityEngine.Random.Range(0, m.Tiles.Count - 1);
                    UpgradeCandyToColorBomb(m.Tiles[Index]);
                }
            }
            else if (m.Tiles.Count > 3 && m.Type == MatchType.IShaped)
            {
                if (m.Tiles.Contains(SelectedTile))
                {
                    UpgradeCandyToStripeCandy(SelectedTile,m.StripeType);
                }
                else if (m.Tiles.Contains(SwapingTile))
                {
                    UpgradeCandyToStripeCandy(SwapingTile, m.StripeType);
                }
                else
                {
                    int Index = UnityEngine.Random.Range(0, m.Tiles.Count - 1);
                    UpgradeCandyToStripeCandy(m.Tiles[Index], m.StripeType);
                }
            }
        }
    }
    public void UpgradeCandyToStripeCandy(GameObject Tile, StripeType Type)
    {
        if (Tile == null)
            return;
        CandyType CandyType = Tile.GetComponent<Candy>().Type;
        GameObject RandomObject;
        if (Type == StripeType.Horizontal)
        {
            RandomObject = TilePool.GetStripeCandy(CandyType, CandyStripeType.Horizontal);
        }
        else
        {
            RandomObject = TilePool.GetStripeCandy(CandyType, CandyStripeType.Vertical);
        }
        RandomObject.transform.position = Tiles[Tile.GetComponent<Tile>().GetIndex()].transform.position;
        Tiles[Tile.GetComponent<Tile>().GetIndex()] = RandomObject;
        RandomObject.GetComponent<Tile>().SetGameBoard(this);
        Tile.GetComponent<PooledPrefab>().Pool.ReturnObject(Tile);
    }
    public void UpgradeCandyToWrapCandy(GameObject Tile)
    {
        if (Tile == null)
            return;
        CandyType CandyType = Tile.GetComponent<Candy>().Type;
        GameObject RandomObject = TilePool.GetWrappedCandy(CandyType);
        RandomObject.transform.position = Tiles[Tile.GetComponent<Tile>().GetIndex()].transform.position;
        Tiles[Tile.GetComponent<Tile>().GetIndex()] = RandomObject;
        RandomObject.GetComponent<Tile>().SetGameBoard(this);
        Tile.GetComponent<PooledPrefab>().Pool.ReturnObject(Tile);
    }
    public void UpgradeCandyToColorBomb(GameObject Tile)
    {
        if (Tile == null)
            return;
        GameObject RandomObject = TilePool.GetColorBomb();
        RandomObject.transform.position = Tiles[Tile.GetComponent<Tile>().GetIndex()].transform.position;
        Tiles[Tile.GetComponent<Tile>().GetIndex()] = RandomObject;
        RandomObject.GetComponent<Tile>().SetGameBoard(this);
        Tile.GetComponent<PooledPrefab>().Pool.ReturnObject(Tile);
    }
    private void CheckMatches()
    {
        foreach (GameObject g in Tiles)
        {
            g.GetComponent<Tile>().SetGameBoard(this);
        }
        List<Match> Matches = new List<Match>();
        Matches.AddRange(GetBlastes());
        if (Matches.Count > 0)
        {
            ExplodeTiles(Matches, false);
            SelectedTile = null;
            SwapingTile = null;
            return;
        }
        Matches.AddRange(GetMatches());
        if (Matches.Count > 0)
        {
            ExplodeTiles(Matches, true);
            SelectedTile = null;
            SwapingTile = null;
        }
        else
        {
            if (SelectedTile != null && SwapingTile != null)
                Swap(SelectedTile, SwapingTile, OnFailSwapComplated);
            else
            {
                UpdateState(GameState.WaitingInput);
            }
        }
    }
}
