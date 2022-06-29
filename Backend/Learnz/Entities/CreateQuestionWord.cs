namespace Learnz.Entities;
public class CreateQuestionWord
{
    public Guid Id { get; set; }
    public string LanguageSubjectMain { get; set; }
    public string LanguageSubjectSecond { get; set; }
    public Guid SetId { get; set; }
    public CreateSet Set { get; set; }
}
