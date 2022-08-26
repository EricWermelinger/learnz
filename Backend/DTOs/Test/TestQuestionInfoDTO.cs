namespace Learnz.DTOs;
public class TestQuestionInfoDTO
{
    public string Name { get; set; }
    public DateTime End { get; set; }
    public List<TestQuestionDTO> Questions { get; set; }
}