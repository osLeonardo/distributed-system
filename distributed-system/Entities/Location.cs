using System;

namespace distributed_system.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public bool IsMatriz { get; set; }
}