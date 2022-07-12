using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject ImprovementManager;

    [SerializeField]
    GameObject Grid;

    [SerializeField]
    GameObject VillageUIScript;

    [SerializeField]
    GameObject GameUIScript;

    InputHandlerScript _inputHandler;
    ImprovementManagerScript _improvementManager;

    GridScript _gridScript;
    VillageUIScript _villageUIScript;
    GameUIScript _gameUIScript;

    GameState _gameState = GameState.None;
    public ClickState ClickState = ClickState.None;

    void Awake()
    {
        _inputHandler = GetComponent<InputHandlerScript>();
        _villageUIScript = VillageUIScript.GetComponent<VillageUIScript>();
        _villageUIScript.RegisterClickStateChange(OnClickStateChange);

        _gridScript = Grid.GetComponent<GridScript>();
        _gameUIScript = GameUIScript.GetComponent<GameUIScript>();
    }

    void Deselect()
    {
        _gridScript.Deselect();
    }

    void Start()
    {
        _gridScript.RegisterOnVillageSelected(OnVillageSelected);
        _improvementManager = ImprovementManager.GetComponent<ImprovementManagerScript>();

        _inputHandler.RegisterOnEscape(Deselect);
    }

    void OnClickStateChange(ClickState state)
    {
        ClickState = state;
        _gridScript.ClickState = state;
    }

    public void EndTurn()
    {
        Debug.Log("End turn");
        _improvementManager.EndTurn();
    }

    public void OnVillageSelected(VillageCenterScript village)
    {
        _gameState = GameState.VillageSelected;
        _villageUIScript.UpdateResources(village.Resources, village.Pops);
        village.RegisterOnProduceListener(OnSelectedVillageProduce);
    }

    private void OnSelectedVillageProduce(VillageCenterScript village)
    {
        _villageUIScript.UpdateResources(village.Resources, village.Pops);
    }

    public void OnSelectedVillageResourcesUpdated(Dictionary<ResourceType, int> resources, int population)
    {
        _villageUIScript.UpdateResources(resources, population);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(pointerEventData.position.x);
        Debug.Log(pointerEventData.position.y);
    }
}

enum GameState
{
    None,
    VillageSelected,
}

public enum ClickState
{
    None,
    CreateFarm,
    CreateWoodcutter,
    CreateVillage,
}