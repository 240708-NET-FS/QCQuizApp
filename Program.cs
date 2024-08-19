using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Question;

public class Program
{
    static List<string> lst_Catagories = new List<string>();
    static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Questions\\Questions.txt");
    public static void Main(string[] args)
    {
        bool NotQutting = true;
        List<Questions> lst_Questions = GatherQuestions();
       
        while(NotQutting)
        {
            Console.WriteLine("Quiz Time! Select a difficulty level!\n1:)Random Quiz\n2:)Focused Quiz\n3:)Comprehensive Quiz\n4:)Create Questions\n5:)Quit");
            int usr_inp = 0;
            int.TryParse(Console.ReadLine(), out usr_inp);
            switch(usr_inp)
            {
                case 1:
                {
                    RandomQuiz(lst_Questions);
                    Console.WriteLine("Continue?(Y/N)");
                    break;
                }
                case 2:
                {
                    FocusedQuiz(lst_Questions);
                    Console.WriteLine("Continue?(Y/N)");
                    break;
                }
                case 3:
                {
                    ComprehensiveQuiz(lst_Questions);
                    Console.WriteLine("Continue?(Y/N)");
                    break;
                }
                case 4:
                {
                    CreateQuestions();
                    Console.WriteLine("Continue?(Y/N)");
                    break;
                } 
                case 5:
                default:
                {
                    NotQutting = false;
                    break;
                }
            }
            if(NotQutting)
            {
                string? answer = Console.ReadLine();
                if(answer is not null)
                {
                    if(!answer.Equals(string.Empty))
                    {
                        if(answer.Equals("N") || answer.Equals("n"))
                        {
                            NotQutting = false;
                        }
                    }
                    else
                        NotQutting = false;
                }
            }
        }
        Console.Write("Good luck with your QC!!");
        
    }

    private static void CreateQuestions()
    {
        bool enteringQuestions = true;
        int questionsAdded = 0;
        string? category = string.Empty;
        string? answer = string.Empty;
        string? newQuestion = string.Empty;
        List<string> lst_CompletedQuestions = new List<string>();
        while (enteringQuestions)
        {
            if(questionsAdded > 0)
            {
                Console.WriteLine("Same category?(Y/N)");
                char keyInfo = Console.ReadKey(intercept: true).KeyChar;
                    switch(keyInfo)
                    {
                        case 'Y':
                        case 'y':
                        {
                            answer = string.Empty;
                            newQuestion = string.Empty;
                            break;
                        }
                        case 'N':
                        case 'n':
                        {
                            Console.WriteLine("What category is this question for?");
                            category = Console.ReadLine();
                            answer = string.Empty;
                            newQuestion = string.Empty;
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
            }
            else
            {
                Console.WriteLine("What category is this question for?");
                category = Console.ReadLine();
            }
            if(category is not null)
            {
                Console.WriteLine("What is the question?");
                newQuestion = Console.ReadLine();
                if(newQuestion is not null)
                {   
                    Console.WriteLine("Do you know the answer?(Y/N)");
                    char keyInfo = Console.ReadKey(intercept: true).KeyChar;
                    switch(keyInfo)
                    {
                        case 'Y':
                        case 'y':
                        {
                            answer = Console.ReadLine();
                            break;
                        }
                        case 'N':
                        case 'n':
                        {
                            answer = "Unknown";
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
            if(string.IsNullOrEmpty(category) || string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(answer))
            {
                Console.Clear();
                Console.WriteLine("Category, question or answer was null / empty.");
                continue;
            }
            else
            {
                string completedLine = $"Category:{category};Question:{newQuestion};Answer:{answer}";
                lst_CompletedQuestions.Add(completedLine);
                Console.WriteLine("Add another question?(Y/N)");
                char keyInfo = Console.ReadKey(intercept: true).KeyChar;
                switch(keyInfo)
                {
                    case 'Y':
                    case 'y':
                    {
                        questionsAdded++;
                        Console.Clear();
                        break;
                    }
                    case 'N':
                    case 'n':
                    default:
                    {
                        File.AppendAllLines(filePath, lst_CompletedQuestions.ToArray());
                        enteringQuestions = false;
                        break;
                    }
                    
                }
            }
        }
    }

    public static List<Questions> GatherQuestions()
    {
        List<Questions> lst_Questions = new List<Questions>();
        int QuestionsAdded = 0;
        try
        {
            // Open the text file using a stream reader.
            using StreamReader reader = new(filePath);
            while(!reader.EndOfStream)
            {
                // Read the stream as a string.
                string? text = reader.ReadLine();

                // Write the text to the console.
                if(text is not null)
                {
                    string cata, question, answer;
                    string[] splitText = text.Split(';');
                    cata = splitText[0];
                    question = splitText[1];
                    answer = splitText[2];
                    cata = cata.Remove(0,cata.IndexOf(':')+1);
                    question = question.Remove(0,question.IndexOf(':')+1);
                    answer = answer.Remove(0, answer.IndexOf(':')+1);
                    lst_Questions.Add(new Questions(cata, question,answer));
                    if(!lst_Catagories.Contains(cata))
                    {
                        lst_Catagories.Add(cata);
                    }
                }
                QuestionsAdded++;
            }
            Console.WriteLine("Questions Retrieved: " + QuestionsAdded);
            reader.Close();
            
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        return lst_Questions;
    }

    //Get a number from the user, then display that many questions, randomly.
    private static void RandomQuiz(List<Questions> lst_Questions)
    {
        int QuestionsToAsk = 0;
        Console.WriteLine("How many questions do you want? (Min 1)");
        int.TryParse(Console.ReadLine(),out QuestionsToAsk);
        List<int> alreadyAsked = new List<int>();
        if(QuestionsToAsk >= 1)
        {
            Random rng = new();
            int rng_ndx = rng.Next(0,lst_Questions.Count);
            for (int ctr = 0; ctr <= QuestionsToAsk-1; ctr++)
            {
                while(alreadyAsked.Contains(rng_ndx))
                {
                    rng_ndx = rng.Next(0,lst_Questions.Count);
                }
                Questions questionToAsk = lst_Questions[rng_ndx];
                Console.WriteLine($"Question #{ctr+1}!\n{questionToAsk.RevealQuestion()}");
                Console.ReadKey();
                Console.WriteLine(questionToAsk.RevealAnswer());
                alreadyAsked.Add(rng_ndx);
            }
                    
        }
        else
        {
            Console.WriteLine("Questions to ask less than or equal to 0. Quitting.");
        }
    }

    //Get a catagory from the user, then display all questions with that catagory.
    private static void FocusedQuiz(List<Questions> lst_Questions)
    {
        int actualCount = 1;
        Console.WriteLine("What catagory do you wish to focus on?");
        for(int i = 0; i < lst_Catagories.Count; i++)
        {
            Console.WriteLine($"{i}: {lst_Catagories[i]}");
        }
        int usr_inp = -1;
        int.TryParse(Console.ReadLine(), out usr_inp);
        string Catagory = lst_Catagories[usr_inp];
        for (int ctr = 0; ctr <= lst_Questions.Count-1; ctr++)
        {
            Questions questionToAsk = lst_Questions[ctr];
            if(questionToAsk.GetCatagory() != Catagory)
                continue;
            Console.WriteLine($"Question #{actualCount}!\n{questionToAsk.RevealQuestion()}");
            Console.ReadKey();
            Console.WriteLine(questionToAsk.RevealAnswer());
            actualCount++;
        }
    }

    //Go from index 0 to number of questions gathered.
    private static void ComprehensiveQuiz(List<Questions> lst_Questions)
    {
        for (int ctr = 0; ctr <= lst_Questions.Count-1; ctr++)
        {
            Questions questionToAsk = lst_Questions[ctr];
            Console.WriteLine($"Question #{ctr+1}!\n{questionToAsk.RevealQuestion()}");
            Console.ReadKey();
            Console.WriteLine(questionToAsk.RevealAnswer());
        }
    }

   
}