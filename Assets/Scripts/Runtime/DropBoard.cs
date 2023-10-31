using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Agave
{
    public class DropBoard : BoardSystem2D<Drop>, IService
    {
        [SerializeField] private BoardDataSO boardData;
        [SerializeField] private GameObject tileBackground;

        private DropPool _dropPool;

        public List<Drop> LastMatchedDrops { get; private set; }

        public void InitializeBoard()
        {
            base.CreateBoard(boardData.dimensions);

            _dropPool = ServiceLocator.Get<DropPool>();

            CreateTileBackgrounds();
            StartCoroutine(PopulateBoard());
        }

        private IEnumerator PopulateBoard(bool allowMatches = false, bool checkForSpawners = false)
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                for (int x = 0; x < Dimensions.x; x++)
                {
                    if (!IsTileEmpty(x, y))
                    {
                        continue;
                    }

                    if (checkForSpawners && !boardData.spawnerColumnIndexes[x])
                    {
                        continue;
                    }

                    Drop drop = _dropPool.GetRandomDrop();
                    drop.SetPosition(x, y);

                    PutItemAt(drop, x, y);

                    var dropData = drop.Type;

                    while (IsPartOfMatch(drop) && !allowMatches)
                    {
                        var nextType = _dropPool.GetNextType(drop);
                        drop.Type = nextType;

                        if (nextType == dropData)
                        {
                            Debug.LogWarning($"Failed to find a drop type that didn't match at ({x}, {y})");
                            break;
                        }
                    }

                    var animationStartPosition = drop.transform.position + (Vector3.up * 15f);

                    drop.SetPosition((int)animationStartPosition.x, (int)animationStartPosition.y);
                    drop.gameObject.SetActive(true);

                    drop.IsMoving = true;
                    drop.transform.DOMove(new Vector2(x, y), 0.3f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => { drop.IsMoving = false; });

                    yield return new WaitForSeconds(0.05f);
                }
            }

            DOVirtual.DelayedCall(0.4f, ScanBoardForMatches);
        }

        private void RepopulateEmptyTiles()
        {
            StartCoroutine(PopulateBoard(true, true));
        }

        public bool IsPartOfMatch(Drop drop)
        {
            LastMatchedDrops = new List<Drop>();

            var horizontalMatches = new List<Drop>();
            var verticalMatches = new List<Drop>();

            horizontalMatches.AddRange(GetMatchesInDirection(drop, Vector2Int.left));
            horizontalMatches.AddRange(GetMatchesInDirection(drop, Vector2Int.right));

            if (horizontalMatches.Count >= 2)
            {
                LastMatchedDrops.AddRange(horizontalMatches);
            }

            verticalMatches.AddRange(GetMatchesInDirection(drop, Vector2Int.up));
            verticalMatches.AddRange(GetMatchesInDirection(drop, Vector2Int.down));

            if (verticalMatches.Count >= 2)
            {
                LastMatchedDrops.AddRange(verticalMatches);
            }

            if (LastMatchedDrops.Count < 2) return false;

            LastMatchedDrops.Add(drop);
            return true;
        }

        private void ScanBoardForMatches()
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                for (int x = 0; x < Dimensions.x; x++)
                {
                    if (IsTileEmpty(x, y))
                    {
                        continue;
                    }

                    var drop = GetItemAt(x, y);

                    if (drop.IsMoving)
                    {
                        continue;
                    }

                    if (IsPartOfMatch(drop))
                    {
                        ResolveMatch();
                    }
                }
            }
        }

        public void ResolveMatch()
        {
            foreach (var drop in LastMatchedDrops)
            {
                var dropTransform = drop.transform;
                var position = new Vector2Int((int)dropTransform.position.x, (int)dropTransform.position.y);
                
                RemoveItemAt(position.x, position.y);

                dropTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() => { _dropPool.ReturnDropToPool(drop); });
            }

            DOVirtual.DelayedCall(0.5f, RepopulateEmptyTiles);
        }

        private List<Drop> GetMatchesInDirection(Drop drop, Vector2Int direction)
        {
            var matches = new List<Drop>();

            var dropPosition = (Vector2)drop.transform.position;
            var position = direction + new Vector2Int((int)dropPosition.x, (int)dropPosition.y);

            while (CheckBounds(position.x, position.y)
                   && !IsTileEmpty(position.x, position.y)
                   && GetItemAt(position.x, position.y).Type == drop.Type)
            {
                matches.Add(GetItemAt(position.x, position.y));
                position += direction;
            }

            return matches;
        }

        private void CreateTileBackgrounds()
        {
            var backgroundParent = new GameObject
            {
                transform =
                {
                    parent = transform,
                    name = "TileBackgrounds"
                }
            };

            for (int x = 0; x < Dimensions.x; x++)
            {
                for (int y = 0; y < Dimensions.y; y++)
                {
                    var tileObject = Instantiate(tileBackground, backgroundParent.transform);
                    tileObject.transform.position = new Vector2(x, y);
                }
            }
        }
    }
}