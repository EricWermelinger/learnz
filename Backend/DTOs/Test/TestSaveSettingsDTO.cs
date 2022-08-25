namespace Learnz.DTOs;
public class TestSaveSettingsDTO
{
    public Guid TestId { get; set; }
    public string Name { get; set; }
    public int MaxTime { get; set; }
    public bool Visible { get; set; }
    public bool Active { get; set; }
    public List<TestSettingsSaveQuestionDTO> Questions { get; set; }
}