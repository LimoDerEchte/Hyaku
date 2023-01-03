using Atto;

namespace Hyaku.GameManagement
{
    public static class ProgressionUtils
    {
        private static IProgressionService Progression => Core.Get<IProgressionService>();

        public static void AddEnding(EndingTypes ending)
        {
            Progression.UnlockEnding(ending);
        }
        
        public static void AddHint(EndingTypes ending)
        {
            Progression.CollectHint((int)ending);
        }
    }
}