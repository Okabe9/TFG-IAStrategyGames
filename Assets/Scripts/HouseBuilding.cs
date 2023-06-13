public class HouseBuilding : Building
{
    HouseBuilding()
    {
        Actions.Add(new BuildingGenerateAction());
        Actions.Add(new BuildingProcessAction());
    }
    // Start is called before the first frame update
    void Start()
    {

    }


}
