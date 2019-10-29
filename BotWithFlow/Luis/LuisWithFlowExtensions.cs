using System;
using System.Linq;
using Luis;

namespace BotWithFlow.Luis
{
    public static class LuisWithFlowExtensions
    {
        public static bool TryGetGroupName(this LuisWithFlow luisResult, out string groupName)
        {
            var found = false;
            groupName = luisResult.Entities?.GroupName?.FirstOrDefault()?.FirstOrDefault().ToString();
            return !string.IsNullOrEmpty(groupName);
        }

        internal static bool TryGetPersonName(this LuisWithFlow luisResult, out string personName)
        {
            var found = false;
            personName = luisResult.Entities?.Persons?.FirstOrDefault();
            return !string.IsNullOrEmpty(personName);

        }
    }
}