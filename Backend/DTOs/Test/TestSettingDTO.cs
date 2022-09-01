namespace Learnz.DTOs;
public class TestSettingDTO
{
    public Guid TestId { get; set; }
    public string Name { get; set; }
    public string SetName { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public int MaxTime { get; set; }
    public bool Visible { get; set; }
    public bool Active { get; set; }
    public List<TestQuestionSettingDTO> Questions { get; set; }
}