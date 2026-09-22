using System.Text.Json;
using System.IO;

namespace DoTA_3
{
    public class CEnemyTemplateList
    {
        //Список противников из класса CEnemyTemplate
        List<CEnemyTemplate> enemies;
        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>();
        }
        public void SaveToJson(string path)
        {
            string jsonString = JsonSerializer.Serialize(enemies);
            File.WriteAllText(path, jsonString);
        }

        public void AddEnemy(string name, string iconName, int baseLife,
                     double lifeModifier, int baseGold,
                     double goldModifier, double spawnChance)
        {
            enemies.Add(new CEnemyTemplate(name, iconName, baseLife,
                                           lifeModifier, baseGold,
                                           goldModifier, spawnChance));
        }
        public List<string> GetListOfEnemyNames()
        {
            List<string> names = new List<string>();
            foreach (CEnemyTemplate enemy in enemies)
            {
                names.Add(enemy.Name);
            }
            return names;
        }

        public void LoadFromJson(string path)
        {
            string jsonFromFile = File.ReadAllText(path);
            JsonDocument doc = JsonDocument.Parse(jsonFromFile);

            enemies.Clear();  // очищаем текущий список, чтобы не дублировать

            foreach (JsonElement element in doc.RootElement.EnumerateArray())
            {
                string name = element.GetProperty("Name").GetString();
                string iconName = element.GetProperty("IconName").GetString();
                int baseLife = element.GetProperty("BaseLife").GetInt32();
                double lifeModifier = element.GetProperty("LifeModifier").GetDouble();
                int baseGold = element.GetProperty("BaseGold").GetInt32();
                double goldModifier = element.GetProperty("GoldModifier").GetDouble();
                double spawnChance = element.GetProperty("SpawnChance").GetDouble();

                enemies.Add(new CEnemyTemplate(name, iconName, baseLife,
                                               lifeModifier, baseGold,
                                               goldModifier, spawnChance));
            }
        }
        public void DeleteEnemyByName(string name)
        {
            // Ищем противника с таким именем
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Name == name)
                {
                    enemies.RemoveAt(i);
                    return;   // нашли и удалили — выходим
                }
            }
        }
        public CEnemyTemplate GetEnemyByName(string name)
        {
            foreach (CEnemyTemplate enemy in enemies)
            {
                if (enemy.Name == name)
                {
                    return enemy;
                }
            }
            return null;   // не нашли
        }
    }
}
