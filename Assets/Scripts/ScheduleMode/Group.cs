using System;

[Serializable]
public class Group
{
    public string Name;
    public long Id;

    public Group(long id, string name)
    {
        Id = id;
        Name = name;
    }
}
