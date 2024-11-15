using System.Threading.Tasks;
using distributed_system.Entities;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories.Intefaces;

public interface ILocationRepository
{
    ActionResult AddLocation(Location location);
    ActionResult UpdateLocation(Location location);
    Location GetLocationByName(string name);
    Location GetLocationByUsernameAndPassword(string username, string password);
    ActionResult DeleteLocation(int id, string name);
}