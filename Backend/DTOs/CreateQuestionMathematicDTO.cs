namespace Learnz.DTOs;
public class CreateQuestionMathematicDTO
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int Digits { get; set; }
    public List<CreateQuestionMathematicVariableDTO> Variables { get; set; }
}