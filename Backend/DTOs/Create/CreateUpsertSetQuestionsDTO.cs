namespace Learnz.DTOs;
public class CreateUpsertSetQuestionsDTO
{
    public Guid SetId { get; set; }
    public List<CreateQuestionDistributeDTO> QuestionsDistribute { get; set; }
    public List<CreateQuestionMathematicDTO> QuestionsMathematic { get; set; }
    public List<CreateQuestionMultipleChoiceDTO> QuestionsMultipleChoice { get; set; }
    public List<CreateQuestionOpenQuestionDTO> QuestionsOpenQuestion { get; set; }
    public List<CreateQuestionTextFieldDTO> QuestionsTextField { get; set; }
    public List<CreateQuestionTrueFalseDTO> QuestionsTrueFalse { get; set; }
    public List<CreateQuestionWordDTO> QuestionsWord { get; set; }
}