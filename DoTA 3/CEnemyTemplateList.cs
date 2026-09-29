using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DoTA_3
{
    public class CEnemyTemplateList
    {
        // ---------- ПОЛЯ ----------
        List<CEnemyTemplate> enemies;
        List<double> normalizedChances = new List<double>();

        // ---------- КОНСТРУКТОР ----------
        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>();
        }

        // ---------- ДОБАВЛЕНИЕ / УДАЛЕНИЕ ----------

        public void AddEnemy(string name, string iconName, int baseLife,
            double lifeModifier, int baseGold,
            double goldModifier, double spawnChance)
        {
            enemies.Add(new CEnemyTemplate(name, iconName, baseLife,
                lifeModifier, baseGold, goldModifier, spawnChance));
        }

        public void DeleteEnemyByName(string name)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Name == name)
                {
                    enemies.RemoveAt(i);
                    return;
                }
            }
        }

        public void DeleteEnemyByIndex(int id)
        {
            if (id < 0 || id >= enemies.Count) return;
            enemies.RemoveAt(id);
        }

        // ---------- ПОИСК ----------

        public CEnemyTemplate GetEnemyByName(string name)
        {
            foreach (CEnemyTemplate enemy in enemies)
            {
                if (enemy.Name == name) return enemy;
            }
            return null;
        }

        public CEnemyTemplate GetEnemyByIndex(int id)
        {
            if (id < 0 || id >= enemies.Count) return null;
            return enemies[id];
        }

        public List<string> GetListOfEnemyNames()
        {
            List<string> names = new List<string>();
            foreach (CEnemyTemplate e in enemies) names.Add(e.Name);
            return names;
        }

        // ---------- JSON ----------

        public void SaveToJson(string path)
        {
            string json = JsonSerializer.Serialize(enemies);
            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            string json = File.ReadAllText(path);
            JsonDocument doc = JsonDocument.Parse(json);
            enemies.Clear();

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
                    lifeModifier, baseGold, goldModifier, spawnChance));
            }
        }

        // ---------- НОРМАЛИЗАЦИЯ ШАНСОВ И ВЫБОР ВРАГА ----------

        public void NormalizeChances()
        {
            normalizedChances.Clear();

            if (enemies.Count == 0) return;

            double sum = 0;
            for (int i = 0; i < enemies.Count; i++)
                sum += enemies[i].SpawnChance;

            if (sum == 0) return;   // защита от деления на ноль

            for (int i = 0; i < enemies.Count; i++)
                normalizedChances.Add(enemies[i].SpawnChance / sum);
        }

        public CEnemyTemplate FindByChance(double chance)
        {
            if (enemies.Count == 0) return null;

            double sum = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                sum += normalizedChances[i];
                if (sum >= chance) return enemies[i];
            }
            return null;
        }
    }
}