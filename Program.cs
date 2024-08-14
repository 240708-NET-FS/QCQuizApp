using Question;

public class Program
{
    static List<string> lst_Catagories = new List<string>();
    public static void Main(string[] args)
    {
        bool NotQutting = true;
        List<Questions> lst_Questions = GatherQuestions();
       
        while(NotQutting)
        {
            Console.WriteLine("Quiz Time! Select a difficulty level!\n1:)Random Quiz\n2:)Focused Quiz\n3:)Comprehensive Quiz\n4:)Quit");
            int usr_inp = 0;
            int.TryParse(Console.ReadLine(), out usr_inp);
            switch(usr_inp)
            {
                case 1:
                {
                    RandomQuiz(lst_Questions);
                    break;
                }
                case 2:
                {
                    FocusedQuiz(lst_Questions);
                    break;
                }
                case 3:
                {
                    ComprehensiveQuiz(lst_Questions);
                    break;
                } 
                case 4:
                default:
                {
                    break;
                }
            }
            Console.WriteLine("Continue?(Y/N)");
            string? answer = Console.ReadLine();
            if(answer is not null)
            {
                if(!answer.Equals(string.Empty))
                {
                    if(answer.Equals('N') || answer.Equals('n'))
                    {
                        NotQutting = false;
                    }
                }
                else
                    NotQutting = false;
            }
        }
        Console.Write("Good luck with your QC!!");
        
    }

    public static List<Questions> GatherQuestions()
    {
        List<Questions> lst_Questions = new List<Questions>();
        int QuestionsAdded = 0;
        try
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectoryPath, "Questions\\Questions.txt");
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