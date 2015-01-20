using System.Collections.Generic;

namespace SGI.Core.GameInfo
{
    public class GameInfoMapper
    {
        public GameInfo TextToEntity(string infoline)
        {
            var values = infoline.Split('|');
            if (values.Length != 2)
                return new GameInfo();

            return new GameInfo
            {
                Name = NameFormatter(values[0]),
                ActivationInfo = ActivationInfoFormatter(values[1])
            };
        }

        private string NameFormatter(string name)
        {
            return name.Contains("Steam Key") ? name.Replace("Steam Key", string.Empty).Trim() : name.Trim();
        }

        private string ActivationInfoFormatter(string activationInfo)
        {
            return activationInfo.Trim();
        }

        public string EntitiyToText(GameInfo gameInfo)
        {
            return gameInfo.ToString();
        }
    }
}

