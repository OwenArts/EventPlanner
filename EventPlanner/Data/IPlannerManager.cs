using EventPlanner.Data.AbstractClasses;

namespace EventPlanner.Data;

public interface IPlannerManager
{
    void ValidateFestival(Festival festival);
    Festival PlanFestival(Festival festival);
}