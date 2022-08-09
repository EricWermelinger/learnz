namespace Learnz.DTOs;
public class CreateQuestionMathematicVariableDTO
{
    public Guid Id { get; set; }
    public string Display { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public int Digits { get; set; }
    public double Interval { get; set; }
}