namespace Learnz.Entities;
public class CreateSet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Subject SubjectMain { get; set; }
    public Subject? SubjectSecond { get; set; }
    public DateTime Created { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime Modified { get; set; }
    public Guid ModifiedById { get; set; }
    public User ModifiedBy { get; set; }
    public SetPolicy SetPolicy { get; set; }
    
    public ICollection<CreateQuestionDistribute> QuestionDistributes { get; set; }
    public ICollection<CreateQuestionMathematic> QuestionMathematics  { get; set; }
    public ICollection<CreateQuestionMultipleChoice> QuestionMultipleChoices { get; set; }
    public ICollection<CreateQuestionOpenQuestion> QuestionOpenQuestions { get; set; }
    public ICollection<CreateQuestionTextField> QuestionTextFields { get; set; }
    public ICollection<CreateQuestionTrueFalse> QuestionTrueFalses { get; set; }
    public ICollection<CreateQuestionWord> QuestionWords { get; set; }
    public ICollection<Challenge> Challenges { get; set; }
    public ICollection<LearnSession> LearnSessions { get; set; }
    public ICollection<Test> Tests { get; set; }
}
