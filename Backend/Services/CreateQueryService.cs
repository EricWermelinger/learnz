namespace Learnz.Services;
public class CreateQueryService : ICreateQueryService
{
    public int NumberOfWords(CreateSet crs)
    {
        int numberOfWords = (crs.QuestionDistributes == null ? 0 : crs.QuestionDistributes.Count())
                                + (crs.QuestionMathematics == null ? 0 : crs.QuestionMathematics.Count())
                                + (crs.QuestionMultipleChoices == null ? 0 : crs.QuestionMultipleChoices.Count())
                                + (crs.QuestionOpenQuestions == null ? 0 : crs.QuestionOpenQuestions.Count())
                                + (crs.QuestionTextFields == null ? 0 : crs.QuestionTextFields.Count())
                                + (crs.QuestionTrueFalses == null ? 0 : crs.QuestionTrueFalses.Count())
                                + (crs.QuestionWords == null ? 0 : crs.QuestionWords.Count());
        return numberOfWords;
    }
}
