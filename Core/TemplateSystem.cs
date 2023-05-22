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

                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Template_System, "Template System Initialized Successfully.");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Template_System, $"Error initializing Template System: {ex.Message}");
            }
        }

        public void AddTemplate(TemplateModel newTemplate)
        {
            try
            {
                EnvironmentsVariable.AllTemplates.Add(newTemplate);
                SaveIntoJsonFile();
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Template_System, $"A new template has been added. [{newTemplate.Name}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Template_System, $"Error adding template: {ex.Message}");
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
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Template_System, $"A template has been edited. [{newTemplate.Name}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Template_System, $"Error editing template: {ex.Message}");
            }
        }

        public void RemoveTemplate(TemplateModel oldTemplate)
        {
            try
            {
                EnvironmentsVariable.AllTemplates.Remove(oldTemplate);

                SaveIntoJsonFile();
                LoggerSystem.Log(LogStateEnum.Info, LogPrefixEnum.Template_System, $"A template has been deleted. [{oldTemplate.Name}]");
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Template_System, $"Error deleting template: {ex.Message}");
            }
        }

        private void SaveIntoJsonFile()
        {
#if DEBUG
            LoggerSystem.Log(LogStateEnum.Debug, LogPrefixEnum.Template_System, "SaveIntoJsonFile() has been called");
#endif
            try
            {
                File.WriteAllText(EnvironmentsVariable.PathTemplates + EnvironmentsVariable.TemplatesJsonFileName, JsonConvert.SerializeObject(EnvironmentsVariable.AllTemplates, Formatting.Indented));
            }
            catch (Exception ex)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.Template_System, $"Error saving changes to the template file: {ex.Message}");
            }
        }
    }
}
