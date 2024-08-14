
namespace Question;

public class Questions
{
    string Catagory;
    string Question;
    string Answer;

    public Questions()
    {
        Catagory = string.Empty;
        Question = string.Empty;
        Answer = string.Empty;
    }
    public Questions(string cata, string que, string ans)
    {
        Catagory = cata;
        Question = que;
        Answer = ans;
    }

    public string RevealQuestion()
    {
        return $"This question is about [{Catagory}]. {Question}";
    }

    public string RevealAnswer()
    {
        return $"The answer is: {Answer}";
    }

    internal string GetCatagory()
    {
        return Catagory;
    }
}