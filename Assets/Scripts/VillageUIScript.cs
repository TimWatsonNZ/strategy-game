using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageUIScript : MonoBehaviour
{
    [SerializeField]
    Text FoodTxt;

    [SerializeField]
    Text WoodTxt;

    [SerializeField]
    Text LabourTxt;

    [SerializeField]
    Text PopulationTxt;

    public delegate void OnClickStateChange(ClickState state);
    public event OnClickStateChange ClickStateChange;

    public SelectedControl _selectedControl { get; private set; } = SelectedControl.None;
    public void OnFarmBtnPressed()
    {
        _selectedControl = SelectedControl.Farm;
        ClickStateChange?.Invoke(ClickState.CreateFarm);
    }
    public void OnWoodcutterBtnPressed()
    {
        _selectedControl = SelectedControl.Woodcutter;
        ClickStateChange?.Invoke(ClickState.CreateWoodcutter);
    }
    public void OnVillageBtnPressed()
    {
        _selectedControl = SelectedControl.Village;
        ClickStateChange?.Invoke(ClickState.CreateVillage);
    }

    public void Reset()
    {
        _selectedControl = SelectedControl.None;
    }

    public void RegisterClickStateChange(OnClickStateChange listener)
    {
        ClickStateChange += listener;
    }

    public void UpdateResources(Dictionary<ResourceType, int> resources, int population)
    {
        FoodTxt.text = $"Food: {(resources.ContainsKey(ResourceType.Food) ? resources[ResourceType.Food].ToString() : "0")}";
        WoodTxt.text = $"Wood: {(resources.ContainsKey(ResourceType.Wood) ? resources[ResourceType.Wood].ToString() : "0")}";
        LabourTxt.text = $"Labour: {(resources.ContainsKey(ResourceType.Labour) ? resources[ResourceType.Labour].ToString() : "0")}";
        PopulationTxt.text = $"Pops: {population}";
    }
}

public enum SelectedControl
{
    None,
    Farm,
    Woodcutter,
    Village,
}
