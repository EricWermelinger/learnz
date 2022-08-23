namespace Learnz.DTOs;
public class TestQuestionSettingDTO
{
    public GeneralQuestionQuestionDTO Question { get; set; }
    public int PointsPossible { get; set; }
    public bool Visible { get; set; }
}