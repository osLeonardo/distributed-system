using System.Threading.Tasks;
using distributed_system.Entities;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories.Intefaces;

public interface ILocationRepository
{
    ActionResult AddLocation(Location location);
}