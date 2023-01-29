namespace InvoicesManager.Core
{
    public class TemplateSystem
    {
        public void Init()
        {
            try
            {
                EnvironmentsVariable.AllTemplates.Clear();

                string json = File.ReadAllText(EnvironmentsVariable.PathTemplates + EnvironmentsVariable.TemplatesJsonFileName);

                if (!(json.Equals("[]") || String.IsNullOrWhiteSpace(json) || json.Equals("null")))
                    EnvironmentsVariable.AllTemplates = JsonConvert.DeserializeObject<List<TemplateModel>>(json);
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Template_System, ex.Message);
            }
        }

        public void AddTemplate(TemplateModel newTemplate)
        {
            try
            {
                EnvironmentsVariable.AllTemplates.Add(newTemplate);
                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Template_System, $"A new template has been added. [{newTemplate.Name}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Template_System, ex.Message);
            }
        }

        public void EditTemplate(string oldTemplateName, TemplateModel newTemplate)
        {
            try
            {
                //get template by name 
                TemplateModel oldTemplate = EnvironmentsVariable.AllTemplates.Where(x => x.Name.Equals(oldTemplateName)).FirstOrDefault();
                EnvironmentsVariable.AllTemplates.Remove(oldTemplate);
                EnvironmentsVariable.AllTemplates.Add(newTemplate);

                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Template_System, $"A template has been edited. [{newTemplate.Name}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Template_System, ex.Message);
            }
        }

        public void RemoveTemplate(TemplateModel oldTemplate)
        {
            try
            {
                EnvironmentsVariable.AllTemplates.Remove(oldTemplate);

                SaveIntoJsonFile();
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Info, Classes.Enums.LogPrefixEnum.Template_System, $"A template has been deleted. [{oldTemplate.Name}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(Classes.Enums.LogStateEnum.Error, Classes.Enums.LogPrefixEnum.Template_System, ex.Message);
            }
        }
        
        private void SaveIntoJsonFile()
        {
            File.WriteAllText(EnvironmentsVariable.PathTemplates + EnvironmentsVariable.TemplatesJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.AllTemplates, Formatting.Indented));
        }
    }
}
