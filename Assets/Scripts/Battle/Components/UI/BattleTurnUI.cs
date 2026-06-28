using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fungus.DentedPixel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using NaughtyAttributes;

public class BattleTurnUI : MonoBehaviour
{
    [SerializeField]
    private Image entityImg;
    [SerializeField]
    private Transform playerEntityTurnParent, enemyEntityTurnParent;

    private List<Image> _playerEntityPool = new();
    private List<Image> _enemyEntityPool = new();

    public void InitializeEntityList(List<BattleEntity> entities)
    {
        StartCoroutine(SetEntityListCoroutine(entities));
    }

    private IEnumerator SetEntityListCoroutine(List<BattleEntity> entities)
    {
        var spacing = ((RectTransform)entityImg.transform).rect.width + 10f;
        var parentWidth = ((RectTransform)playerEntityTurnParent.transform).rect.width;
        var startingPos = -parentWidth;
        var targetSpacing = parentWidth / 2 - spacing / 2;
        
        for (int i = 0; i < entities.Count; i++)
        {
            if (_playerEntityPool.Count <= i)
                _playerEntityPool.Add(InstantiateTurnSprite(
                    playerEntityTurnParent, 
                    startingPos, 
                    targetSpacing - spacing * i));
            
            if (_enemyEntityPool.Count <= i)
                _enemyEntityPool.Add(InstantiateTurnSprite(
                    enemyEntityTurnParent, 
                    startingPos, 
                    targetSpacing - spacing * i));
            
            SetEntitySprite(entities[i].EntityData, i);
            
            yield return new WaitForSeconds(.2f);
        }
    }

    public void AdvanceTurnUI(List<BattleEntity> entities)
    {
        StartCoroutine(AdvanceTurnUICoroutine(entities));
    }

    private IEnumerator AdvanceTurnUICoroutine(List<BattleEntity> entities)
    {
        var spacing = ((RectTransform)entityImg.transform).rect.width + 10f;
        var target = playerEntityTurnParent.localPosition;
        target.x += spacing;
        LeanTween.moveLocal(playerEntityTurnParent.gameObject, target, .5f);
        LeanTween.moveLocal(enemyEntityTurnParent.gameObject, target, .5f);
        
        yield return new WaitForSeconds(.5f);
        
        target.x -= spacing;
        playerEntityTurnParent.localPosition = target;
        enemyEntityTurnParent.localPosition = target;
        
        for (var i = 0; i < entities.Count; i++)
        {
            SetEntitySprite(entities[i].EntityData, i);
        }
    }

    private Image InstantiateTurnSprite(Transform parent, float startingPos, float targetPos)
    {
        var go = Instantiate(entityImg, parent);
        go.transform.localPosition = Vector3.right * startingPos;
        LeanTween.moveLocal(go.gameObject, Vector3.right * targetPos,.5f);
        return go;
    }

    private void SetEntitySprite(BattleEntityScriptableObject entity, int index)
    {
        switch (entity.Side)
        {
            case EntitySide.Ally:
                _playerEntityPool[index].sprite = entity.Sprite;
                _playerEntityPool[index].color = Color.white;
                _enemyEntityPool[index].color = Color.clear;
                break;
            case EntitySide.Enemy:
                _enemyEntityPool[index].sprite = entity.Sprite;
                _enemyEntityPool[index].color = Color.white;
                _playerEntityPool[index].color = Color.clear;
                break;
        }
    }
}
