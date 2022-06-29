namespace Learnz.Entities;
public class CreateQuestionMathematicVariable
{
    public Guid Id { get; set; }
    public string Display { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public int Digits { get; set; }
    public double Interval { get; set; }
    public Guid QuestionMathematicId { get; set; }
    public CreateQuestionMathematic QuestionMathematic { get; set; }
}
