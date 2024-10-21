using ClientSamgkOutputResponse.Interfaces.Schedule;
using System.Linq;

namespace Assets.Scripts.Extern
{
    public static class ScheduleExtern
    {
        public static bool IsDist(this IResultOutScheduleFromDate resultOutScheduleFromDate)
        {
            var cabs = resultOutScheduleFromDate.Lessons.Where(x => x.Cabs[0].Adress == "дист/дист");

            return cabs.Count() > 0 ? true : false;
        }
    }
}
