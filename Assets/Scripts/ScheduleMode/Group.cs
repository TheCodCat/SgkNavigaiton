[System.Serializable]
public class Group
{
    public string Name;
    public int Id;

    public Group(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
